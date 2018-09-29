using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Serializable]
    public class DataSourceSet : IEnumerable<KeyValuePair<string, IDataSource<float>>>
    {
        public Dictionary<string, IDataSource<float>> Features { get => _data; }

        internal Dictionary<string, IDataSource<float>> _data;

        public DataSourceSet()
        {
            _data = new Dictionary<string, IDataSource<float>>();
        }

        public DataSourceSet(IDictionary<string, IDataSource<float>> dataSources)
            : this()
        {
            foreach (var entry in dataSources)
                Add(entry.Key, entry.Value);
        }

        public DataSourceSet(Hashtable dataSet)
            : this()
        {
            foreach (DictionaryEntry entry in dataSet)
            {
                var value = entry.Value;
                if (value is PSObject psobj)
                    value = psobj.BaseObject;
                _data.Add((string)entry.Key, (IDataSource<float>)value);
            }
        }

        public int SampleCount
        {
            get => _data.First().Value.Shape[-1];
        }

        public IDataSource<float> this[string name]
        {
            get => _data[name];
            set
            {
                Add(name, value);
            }
        }

        public void Add(string name, IDataSource<float> data)
        {
            if (_data.Count > 0)
            {
                var f = _data.Values.First();
                if (f.Shape[-1] != data.Shape[-1])
                    throw new ArgumentException("Sample count should be the same among all data");
            }

            _data.Add(name, data);
        }

        public static DataSourceSet Load(byte[] data, bool decompress = true)
        {
            return Serializer.Deserialize<DataSourceSet>(data, decompress);
        }

        public static DataSourceSet Load(string path, bool decompress = true)
        {
            return Serializer.Deserialize<DataSourceSet>(path, decompress);
        }

        public byte[] Save(bool compress = true)
        {
            return Serializer.Serialize(this, compress);
        }

        public void Save(string path, bool compress = true)
        {
            Serializer.Serialize(this, path, compress);
        }

        public static implicit operator Hashtable(DataSourceSet dss)
        {
            var result = new Hashtable();
            foreach (var entry in dss._data)
                result.Add(entry.Key, entry.Value);

            return result;
        }

        public static implicit operator Dictionary<string, IDataSource<float>>(DataSourceSet dss)
        {
            var result = new Dictionary<string, IDataSource<float>>();
            foreach (var entry in dss._data)
                result.Add(entry.Key, entry.Value);

            return result;
        }

        public static implicit operator DataSourceSet(Hashtable hashtable)
        {
            return new DataSourceSet(hashtable);
        }

        public static implicit operator DataSourceSet(Dictionary<string, IDataSource<float>> dict)
        {
            return new DataSourceSet(dict);
        }

        public IEnumerator<KeyValuePair<string, IDataSource<float>>> GetEnumerator()
        {
            return new DataSourceSetEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DataSourceSetEnumerator(this);
        }
    }

    public class DataSourceSetEnumerator : IEnumerator<KeyValuePair<string, IDataSource<float>>>
    {
        private IEnumerator<KeyValuePair<string, IDataSource<float>>> _e;

        public DataSourceSetEnumerator(DataSourceSet dataSourceSet)
        {
            _e = dataSourceSet._data.GetEnumerator();
        }

        public KeyValuePair<string, IDataSource<float>> Current => _e.Current;

        object IEnumerator.Current => (object)_e.Current;

        public void Dispose()
        {
            _e.Dispose();
        }

        public bool MoveNext()
        {
            return _e.MoveNext();
        }

        public void Reset()
        {
            _e.Reset();
        }
    }
}
