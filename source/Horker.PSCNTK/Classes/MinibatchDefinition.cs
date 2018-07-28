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

    public class MinibatchDefinition
    {
        public Dictionary<string, DataSource<float>> Features;
        public DeviceDescriptor Device;

        public int MinibatchSize;
        public bool Randomized;

        public int Total;
        public int Current;

        private int _validationStart;
        private int[] _order;

        public MinibatchDefinition(Dictionary<string, DataSource<float>> features, int minibatchSize, double validationRate = .3, bool randomize = true, DeviceDescriptor device = null)
        {
            var f = features.Values.First();

            if (f.Shape.Rank < 3)
                throw new ArgumentException("Source data should have more than or equal to three dimensions");

            if (f.Shape[-1] < minibatchSize)
                throw new ArgumentException("Sample size is smaller than minibatch size");

            foreach (var entry in features)
                if (entry.Value.Shape[-1] != f.Shape[-1] || entry.Value.Shape[-2] != f.Shape[-2])
                    throw new ArgumentException("Features should have the same sequence length and sample size");

            _validationStart = (int)(f.Shape[-1] * (1 - validationRate));

            Features = features;
            Randomized = randomize;

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();
            Device = device;

            MinibatchSize = minibatchSize;

            Total = f.Shape[-1];
            Current = 0;

            // Randomize data

            _order = new int[f.Shape[-1]];
            for (var i = 0; i < f.Shape[-1]; ++i)
                _order[i] = i;

            Randomize();
        }

        private void Randomize()
        {
            if (!Randomized)
                return;

            var f = Features.Values.First();
            var randomEnd = f.Shape[-1];

            // for series data, validation data always keeps its order and is obtained from the latest portion
            if (f.Shape[-2] >= 2)
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

        private MinibatchData GetBatch(DataSource<float> feature, int cur, int batchSize, bool sweepEnd)
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

            var value = new Value(new NDArrayView(shape.Dimensions, buffer, Device, true));
            var batch = new MinibatchData(value, (uint)batchSize, (uint)(batchSize * feature.Shape[-2]), sweepEnd);

            return batch;
        }

        public Minibatch GetNextBatch()
        {
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
                var b = GetBatch(entry.Value, Current, MinibatchSize, sweepEnd);
                batch.Features.Add(entry.Key, b);
            }

            batch.SweepEnd = sweepEnd;

            Current += MinibatchSize;

            return batch;
        }

        public Minibatch GetValidationBatch()
        {
            var batchSize = Total - _validationStart;

            if (batchSize == 0)
                return null;

            var batch = new Minibatch();

            foreach (var entry in Features)
            {
                var b = GetBatch(entry.Value, _validationStart, batchSize, false);
                batch.Features.Add(entry.Key, b);
            }

            batch.SweepEnd = false;

            return batch;
        }
    }
}
