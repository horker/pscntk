﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    [Serializable]
    public class OnMemorySampler : ISampler
    {
        public Dictionary<string, IDataSource<float>> Features
        {
            get => _features;
            private set { _features = value; }
        }

        public int MinibatchSize
        {
            get => _minibatchSize;
            set { ResetInternalState(); _minibatchSize = value; }
        }

        public double ValidationRate
        {
            get => _validationRate;
            set { ResetInternalState(); _validationRate = value; }
        }

        public bool Randomized
        {
            get => _randomized;
            set { ResetInternalState(); _randomized = value; }
        }

        public bool WithSequenceAxis { get; private set; }
        public int Total { get; private set; }
        public int Current { get; private set; }

        private Dictionary<string, IDataSource<float>> _features;

        private int _minibatchSize;
        private double _validationRate;
        private bool _randomized;

        private int _validationStart;
        private int[] _order;

        public OnMemorySampler(DataSourceSet features, int minibatchSize, double validationRate = .3, bool randomize = true, bool withSequenceAxis = false)
        {
            var f = features.Features.Values.First();

            if (f.Shape.Rank < 2)
                throw new ArgumentException("Source data should have a batch axis");

            if (f.Shape[-1] < minibatchSize)
                throw new ArgumentException("Sample size is smaller than minibatch size");

            if (minibatchSize < 1)
                throw new ArgumentException("Minibatch size should be greater than zero");

            foreach (var entry in features)
            {
                if (entry.Value.Shape[-1] != f.Shape[-1])
                    throw new ArgumentException("Features should have the same sample size");

                if (entry.Value.Shape[-2] > 1)
                    WithSequenceAxis = true;
            }

            _features = features;
            _minibatchSize = minibatchSize;
            _validationRate = validationRate;
            _randomized = randomize;

            WithSequenceAxis = withSequenceAxis;
        }

        public static OnMemorySampler Load(byte[] data, bool decompress = true)
        {
            return Serializer.Deserialize<OnMemorySampler>(data, decompress);
        }

        public static OnMemorySampler Load(string path, bool compress = true)
        {
            return Serializer.Deserialize<OnMemorySampler>(path, compress);
        }

        public byte[] Serialize(bool compress = true)
        {
            return Serializer.Serialize(this, compress);
        }

        public void Save(string path, bool compress = true)
        {
            Serializer.Serialize(this, path, compress);
        }

        private void ResetInternalState()
        {
            _order = null;
            Current = 0;
        }

        private void InitializeOrder()
        {
            if (_order == null)
            {
                var f = _features.Values.First();

                Total = f.Shape[-1];
                Current = 0;

                _validationStart = (int)(f.Shape[-1] * (1 - _validationRate));

                _order = new int[f.Shape[-1]];
                for (var i = 0; i < f.Shape[-1]; ++i)
                    _order[i] = i;

                Randomize();
            }
        }

        private void Randomize()
        {
            if (!_randomized)
                return;

            var f = _features.Values.First();
            var randomEnd = f.Shape[-1];

            // for series data, validation data always keeps its order and is obtained from the latest portion
            if (WithSequenceAxis)
                randomEnd = _validationStart;

            var random = Random.GetInstance();
            for (var i = 0; i < randomEnd; ++i)
            {
                var j = random.Next(randomEnd);
                var temp = _order[j];
                _order[j] = _order[i];
                _order[i] = temp;
            }
        }

        private Value GetValue(IDataSource<float> feature, int cur, int batchSize, DeviceDescriptor device)
        {
            var chunkSize = feature.Shape.GetSize(-2);

            var buffer = new float[chunkSize * batchSize];

            for (var i = 0; i < batchSize; ++i)
            {
                var index = _order[cur];
                ++cur;

                for (var j = 0; j < chunkSize; ++j)
                    buffer[i * chunkSize + j] = feature.Data[index * chunkSize + j];
            }

            var shape = feature.Shape.Clone();
            shape[-1] = batchSize;

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            return new Value(NDArrayViewMethods.SafeCreate(shape.Dimensions, buffer, device));
        }

        private Minibatch GetMinibatch(int batchSize, int position, DeviceDescriptor device, bool sweepEnd)
        {
            var batch = new Minibatch();

            foreach (var entry in _features)
            {
                var name = entry.Key;
                var feature = entry.Value;
                var value = GetValue(feature, position, batchSize, device);
                var numSamples = WithSequenceAxis ? batchSize * feature.Shape[-2] : batchSize;
                batch.AddNewMinibatch(name, value, batchSize, numSamples, sweepEnd);
            }

            batch.SweepEnd = sweepEnd;

            return batch;
        }

        public Minibatch GetNextMinibatch(DeviceDescriptor device = null)
        {
            InitializeOrder();

            bool sweepEnd = false;

            if (Current + _minibatchSize > _validationStart)
            {
                Randomize();
                Current = 0;
            }

            if (Current + _minibatchSize * 2 > _validationStart)
                sweepEnd = true;

            var batch = GetMinibatch(_minibatchSize, Current, device, sweepEnd);

            Current += _minibatchSize;

            return batch;
        }

        public Minibatch GetValidationMinibatch(DeviceDescriptor device = null)
        {
            InitializeOrder();

            var batchSize = Total - _validationStart;

            if (batchSize == 0)
                return null;

            return GetMinibatch(batchSize, _validationStart, device, false);
        }
    }
}
