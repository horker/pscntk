using System.Collections.Generic;
using System.Linq;
using CNTK;

namespace Horker.PSCNTK
{
    /// <summary>Wraps CNTK MinibatchData class.</summary>
    /// <remarks>
    /// CNTK MinibatchData class does not keep track of the object assigned to its <c>data</c> property.
    /// Therefore when garbage collection occurred, the <c>data</c> object is
    /// retracted and its reference will become invalid.
    /// This class avoid this by holding the references of the data objects.
    /// </remarks>
    public class Minibatch
    {
        private Dictionary<string, MinibatchData> _features;
        private List<Value> _valueCache;

        public MinibatchData this[string name] => _features[name];

        public IReadOnlyDictionary<string, MinibatchData> Features => _features;

        public int SampleCount => (int)_features.First().Value.numberOfSamples;

        public bool SweepEnd
        {
            get => _features.Any(x => x.Value.sweepEnd);
            set
            {
                foreach (var entry in _features)
                    entry.Value.sweepEnd = value;
            }
        }

        public Minibatch()
        {
            _features = new Dictionary<string, MinibatchData>();
            _valueCache = new List<Value>();
        }

        public Minibatch(IDictionary<string, IDataSource<float>> dataSources, bool sweepEnd = false, DeviceDescriptor device = null)
            : this()
        {
            foreach (var entry in dataSources)
                AddNewMinibatch(entry.Key, entry.Value, sweepEnd, device);
        }

        public Minibatch(IEnumerable<Minibatch> minibatches)
            : this()
        {
            foreach (var m in minibatches)
                foreach (var f in m._features)
                    _features.Add(f.Key, f.Value);
        }

        public void Add(string name, MinibatchData data)
        {
            _features.Add(name, data);
            _valueCache.Add(data.data);
        }

        public void AddNewMinibatch(string name, Value value, int batchSize, int numSamples, bool sweepEnd)
        {
            var data = new MinibatchData(value, (uint)batchSize, (uint)numSamples, sweepEnd);
            Add(name, data);
        }

        public void AddNewMinibatch(string name, IDataSource<float> dataSource, bool sweepEnd, DeviceDescriptor device)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            var shape = dataSource.Shape;
            var value = new Value(NDArrayViewMethods.SafeCreate(shape.Dimensions, dataSource.Data.ToArray(), device));
            AddNewMinibatch(name, value, shape[-1], shape[-1] * shape[-2], sweepEnd);
        }
    }
}
