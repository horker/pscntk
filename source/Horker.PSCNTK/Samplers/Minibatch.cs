using System;
using System.Collections.Generic;
using System.Linq;
using CNTK;

namespace Horker.PSCNTK
{
    public class Minibatch
    {
        private Dictionary<string, Value> _features;
        private bool _sweepEnd;

        public Value this[string name] => _features[name];

        public IReadOnlyDictionary<string, Value> Features => _features;
        public bool SweepEnd { get => _sweepEnd; set { _sweepEnd = value; } }

        public int SampleCount
        {
            get
            {
                var value = _features.First().Value;
                return value.Shape.Dimensions[value.Shape.Rank - 1];
            }
        }

        public Minibatch()
        {
            _features = new Dictionary<string, Value>();
        }

        public Minibatch(IDictionary<string, IDataSource<float>> dataSources, bool sweepEnd = false, DeviceDescriptor device = null)
            : this()
        {
            foreach (var entry in dataSources)
                Add(entry.Key, entry.Value, sweepEnd, device);
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
            _features.Add(name, data.data.DeepClone());
            _sweepEnd = data.sweepEnd;
        }

        public void Add(string name, Value value, bool sweepEnd)
        {
            _features.Add(name, value);
            _sweepEnd = sweepEnd;
        }

        public void Add(string name, IDataSource<float> dataSource, bool sweepEnd, DeviceDescriptor device)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            var shape = dataSource.Shape;
            var value = ValueMethods.SafeCreate(shape.Dimensions, dataSource.Data.ToArray(), device);
            _features.Add(name, value);
            _sweepEnd = sweepEnd;
        }
    }
}