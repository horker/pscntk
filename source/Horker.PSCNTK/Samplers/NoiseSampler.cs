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
        public int IterationsPerEpoch;
        public double Min;
        public double Max;

        private int Iterations;

        private System.Random _random;
        private int _dataSize;
        private float[] _data;
        private IDataSource<float> _samples;

        public NoiseSampler(string name, int[] shape, int minibatchSize, int iterationsPerEpoch, double min, double max, int? seed = null)
        {
            Name = name;
            Shape = shape;
            MinibatchSize = minibatchSize;
            IterationsPerEpoch = iterationsPerEpoch;
            Min = min;
            Max = max;

            _random = Random.GetInstance(seed);
            _dataSize = Shape.GetSize(-1);
            _data = new float[_dataSize * minibatchSize];

            var minibatchShape = new int[Shape.Rank + 2];
            shape.CopyTo(minibatchShape, 0);
            minibatchShape[minibatchShape.Length - 2] = 1; // sequence
            minibatchShape[minibatchShape.Length - 1] = minibatchSize; // sample
            _samples = DataSourceFactory.Create(_data, minibatchShape);

            Iterations = 0;
        }

        public Minibatch GetNextMinibatch(DeviceDescriptor device = null)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            for (var i = 0; i < _dataSize * MinibatchSize; ++i)
                _data[i] = (float)(_random.NextDouble() * (Max - Min) + Min);

            ++Iterations;
            var sweepEnd = (Iterations + 1) % IterationsPerEpoch == 0;

            var minibatch = new Minibatch(new Dictionary<string, IDataSource<float>>() { { Name, _samples } }, sweepEnd, device);
            return minibatch;
        }
    }
}
