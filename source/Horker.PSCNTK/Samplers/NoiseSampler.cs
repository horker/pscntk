using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class NoiseSampler : ISampler
    {
        public string Name;
        public Shape Shape;
        public int MinibatchSize;
        public double Min;
        public double Max;

        private int _dataSize;
        private Random _random;
        private float[] _data;
        private DataSource<float> _samples;

        public NoiseSampler(string name, int[] shape, int minibatchSize, double min, double max, int? seed = null)
        {
            Name = name;
            Shape = shape;
            MinibatchSize = minibatchSize;
            Min = min;
            Max = max;

            if (seed.HasValue)
                _random = new Random(seed.Value);
            else
                _random = new Random();

            _dataSize = Shape.GetSize(-1);
            _data = new float[_dataSize * minibatchSize];

            var minibatchShape = new int[Shape.Rank + 2];
            shape.CopyTo(minibatchShape, 0);
            minibatchShape[minibatchShape.Length - 2] = 1; // sequence
            minibatchShape[minibatchShape.Length - 1] = minibatchSize; // sample
            _samples = new DataSource<float>(_data, minibatchShape);
        }

        public Minibatch GetNextBatch(DeviceDescriptor device = null)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            for (var i = 0; i < _dataSize * MinibatchSize; ++i)
                _data[i] = (float)(_random.NextDouble() * (Max - Min) + Min);

            var minibatch = new Minibatch(new Dictionary<string, DataSource<float>>() { { Name, _samples } }, false, device);
            return minibatch;
        }

        public Minibatch GetValidationBatch(DeviceDescriptor device = null)
        {
            return null;
        }
    }
}
