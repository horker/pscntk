using System;
using System.Collections.Generic;
using System.Linq;
using CNTK;

namespace Horker.PSCNTK
{
    public class DataSourceSampler : SamplerBase
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

        public bool Randomized
        {
            get => _randomize;
            set { ResetInternalState(); _randomize = value; }
        }

        public bool WithSequenceAxis { get; private set; }
        public int Total { get; private set; }
        public int Current { get; private set; }

        private Dictionary<string, IDataSource<float>> _features;

        private int _minibatchSize;
        private bool _randomize;

        private int[] _order;

        public DataSourceSampler(DataSourceSet features, int minibatchSize, bool randomize = true, bool withSequenceAxis = false)
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
            _randomize = randomize;

            WithSequenceAxis = withSequenceAxis;
        }

        protected override void Dispose(bool disposing)
        {
            // Nothing to do.
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

                _order = new int[f.Shape[-1]];
                for (var i = 0; i < f.Shape[-1]; ++i)
                    _order[i] = i;

                Randomize();
            }
        }

        private void Randomize()
        {
            if (!_randomize)
                return;

            var f = _features.Values.First();
            var randomEnd = f.Shape[-1];

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
                batch.Add(name, value, sweepEnd);
            }

            batch.SweepEnd = sweepEnd;

            return batch;
        }

        public override Minibatch GetNextMinibatch(DeviceDescriptor device = null)
        {
            InitializeOrder();

            bool sweepEnd = false;

            if (Current + _minibatchSize > _order.Length)
            {
                Randomize();
                Current = 0;
            }

            if (Current + _minibatchSize * 2 > _order.Length)
                sweepEnd = true;

            var batch = GetMinibatch(_minibatchSize, Current, device, sweepEnd);

            Current += _minibatchSize;

            return batch;
        }
    }
}
