using System.Collections.Generic;
using System.Linq;
using CNTK;

namespace Horker.PSCNTK
{
    public class Minibatch
    {
        public Dictionary<string, MinibatchData> Features;

        public MinibatchData this[string name] => Features[name];

        public int SampleCount => (int)Features.First().Value.numberOfSamples;

        public bool SweepEnd
        {
            get => Features.Any(x => x.Value.sweepEnd);
            set
            {
                foreach (var entry in Features)
                    entry.Value.sweepEnd = value;
            }
        }

        public Minibatch()
        {
            Features = new Dictionary<string, MinibatchData>();
        }

        public Minibatch(IDictionary<string, DataSource<float>> dataSources, bool sweepEnd = false, DeviceDescriptor device = null)
            : this()
        {
            foreach (var entry in dataSources)
            {
                var b = GetMinibatchData(entry.Value, sweepEnd, device);
                Features.Add(entry.Key, b);
            }
        }

        public Minibatch(IEnumerable<Minibatch> minibatches)
            : this()
        {
            foreach (var m in minibatches)
                foreach (var f in m.Features)
                    Features.Add(f.Key, f.Value);
        }

        public static MinibatchData GetMinibatchData(DataSource<float> dataSource, bool sweepEnd, DeviceDescriptor device)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            var shape = dataSource.Shape;
            var value = new Value(new NDArrayView(shape.Dimensions, dataSource.Data, device, true));
            var minibatchData = new MinibatchData(value, (uint)shape[-1], (uint)(shape[-1] * shape[-2]), sweepEnd);

            return minibatchData;
        }
    }
}
