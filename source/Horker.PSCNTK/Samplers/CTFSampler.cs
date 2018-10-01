using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    [Serializable]
    public class CTFSampler : ISampler
    {
        private int _minibatchSize;
        private List<StreamConfiguration> _streamConfigurations;

        private MinibatchSource _minibatchSource;
        private Dictionary<string, StreamInformation> _streamInfos;

        private DataSourceSet _validationData;
        private Minibatch _validationMinibatch;

        public int MinibatchSize { get => _minibatchSize; }
        public IList<StreamConfiguration> StreamConfigurations { get => _streamConfigurations; }

        public CTFSampler(string path, int minibatchSize, bool randomize = true)
        {
            _minibatchSize = minibatchSize;

            // Build a stream configuration

            _streamConfigurations = new List<StreamConfiguration>();

            var elements = GuessDataFormat(path, 10);

            foreach (var e in elements)
            {
                if (e.Value == -1)
                    throw new ArgumentException("CTF file contains sparse data");

                var config = new StreamConfiguration(e.Key, e.Value, false);
                _streamConfigurations.Add(config);
            }

            _minibatchSource = MinibatchSource.TextFormatMinibatchSource(path, _streamConfigurations, MinibatchSource.InfinitelyRepeat, randomize);

            _streamInfos = new Dictionary<string, StreamInformation>();
            foreach (var name in elements.Keys)
                _streamInfos.Add(name, _minibatchSource.StreamInfo(name));
        }

        public static IDictionary<string, int> GuessDataFormat(string path, int readLineCount)
        {
            var elements = new Dictionary<string, int>();
            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                for (var l = 0; l < readLineCount && !reader.EndOfStream; ++l)
                {
                    var line = reader.ReadLine();
                    var samples = line.Split('|');
                    for (var i = 1; i < samples.Length; ++i)
                    {
                        var values = samples[i].Trim().Split();
                        var key = values[0];

                        // Comment
                        if (key[0] == '#')
                            continue;

                        int length;
                        if (values.Length >= 2 && values[1].Contains(':')) // Sparse
                            length = -1;
                        else
                            length = values.Length - 1;

                        if (elements.ContainsKey(key))
                        {
                            if (elements[key] != length)
                                throw new ApplicationException(string.Format("Element {0}'s data length is different among lines", key));
                        }
                        else
                            elements.Add(values[0], length);
                    }
                }

                return elements;
            }
        }

        public Minibatch GetNextMinibatch(DeviceDescriptor device = null)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            var minibatchData = _minibatchSource.GetNextMinibatch((uint)MinibatchSize, device);

            var minibatch = new Minibatch();

            foreach (var info in _streamInfos)
                minibatch.Add(info.Key, minibatchData[info.Value]);

            return minibatch;
        }

        public Minibatch GetValidationMinibatch(DeviceDescriptor device = null)
        {
            return _validationMinibatch;
        }

        public void SetValidationData(DataSourceSet validationData)
        {
            // Keep the original data to avoid data being garbage-collected.
            _validationData = validationData;

            _validationMinibatch = new Minibatch(validationData.Features, false, null);
        }

        public void SetValidationData(Minibatch minibatch)
        {
            _validationMinibatch = minibatch;
        }
    }
}
