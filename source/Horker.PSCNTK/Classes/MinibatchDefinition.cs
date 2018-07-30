using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class Minibatch
    {
        public Dictionary<string, MinibatchData> Features;
        public bool SweepEnd;

        public Minibatch()
        {
            Features = new Dictionary<string, MinibatchData>();
        }
    }

    [Serializable]
    public class MinibatchDefinition
    {
        public Dictionary<string, DataSource<float>> Features;

        // TODO: make changeable
        public int MinibatchSize { get; set; }
        public double ValidationRate { get; private set; }
        public bool Randomized { get; private set; }

        public bool IsSeriesData { get; private set; }
        public int Total { get; private set; }
        public int Current { get; private set; }

        private int _validationStart;
        private int[] _order;

        public MinibatchDefinition(Dictionary<string, DataSource<float>> features, int minibatchSize, double validationRate = .3, bool randomize = true)
        {
            var f = features.Values.First();

            if (f.Shape.Rank < 3)
                throw new ArgumentException("Source data should have more than or equal to three dimensions");

            if (f.Shape[-1] < minibatchSize)
                throw new ArgumentException("Sample size is smaller than minibatch size");

            if (minibatchSize < 1)
                throw new ArgumentException("Minibatch size should be greater than zero");

            foreach (var entry in features)
            {
                if (entry.Value.Shape[-1] != f.Shape[-1])
                    throw new ArgumentException("Features should have the same sample size");

                if (entry.Value.Shape[-2] > 1)
                    IsSeriesData = true;
            }

            _validationStart = (int)(f.Shape[-1] * (1 - validationRate));

            Features = features;
            MinibatchSize = minibatchSize;
            ValidationRate = validationRate;
            Randomized = randomize;

            Total = f.Shape[-1];
            Current = 0;
        }

        public static MinibatchDefinition Load(byte[] data)
        {
            return Serializer.Deserialize<MinibatchDefinition>(data);
        }

        public static MinibatchDefinition Load(string path)
        {
            return Serializer.Deserialize<MinibatchDefinition>(path);
        }

        public byte[] Serialize()
        {
            return Serializer.Serialize(this);
        }

        public void Save(string path)
        {
            Serializer.Serialize(this, path);
        }

        private void InitializeOrder()
        {
            if (_order == null)
            {
                var f = Features.Values.First();
                _order = new int[f.Shape[-1]];
                for (var i = 0; i < f.Shape[-1]; ++i)
                    _order[i] = i;

                Randomize();
            }
        }

        private void Randomize()
        {
            if (!Randomized)
                return;

            var f = Features.Values.First();
            var randomEnd = f.Shape[-1];

            // for series data, validation data always keeps its order and is obtained from the latest portion
            if (IsSeriesData)
                randomEnd = _validationStart;

            var random = new Random();
            for (var i = 0; i < randomEnd; ++i)
            {
                var j = random.Next(randomEnd);
                var temp = _order[j];
                _order[j] = _order[i];
                _order[i] = temp;
            }
        }

        private MinibatchData GetBatch(DataSource<float> feature, int cur, int batchSize, bool sweepEnd, DeviceDescriptor device)
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

            var value = new Value(new NDArrayView(shape.Dimensions, buffer, device, true));
            var batch = new MinibatchData(value, (uint)batchSize, (uint)(batchSize * feature.Shape[-2]), sweepEnd);

            return batch;
        }

        public Minibatch GetNextBatch(DeviceDescriptor device = null)
        {
            InitializeOrder();

            bool sweepEnd = false;

            if (Current + MinibatchSize > _validationStart)
            {
                Randomize();
                Current = 0;
            }

            if (Current + MinibatchSize * 2 > _validationStart)
                sweepEnd = true;

            var batch = new Minibatch();

            foreach (var entry in Features)
            {
                var b = GetBatch(entry.Value, Current, MinibatchSize, sweepEnd, device);
                batch.Features.Add(entry.Key, b);
            }

            batch.SweepEnd = sweepEnd;

            Current += MinibatchSize;

            return batch;
        }

        public Minibatch GetValidationBatch(DeviceDescriptor device = null)
        {
            var batchSize = Total - _validationStart;

            if (batchSize == 0)
                return null;

            InitializeOrder();

            var batch = new Minibatch();

            foreach (var entry in Features)
            {
                var b = GetBatch(entry.Value, _validationStart, batchSize, false, device);
                batch.Features.Add(entry.Key, b);
            }

            batch.SweepEnd = false;

            return batch;
        }
    }
}
