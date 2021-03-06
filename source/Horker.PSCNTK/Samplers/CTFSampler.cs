﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    [Serializable]
    public class CTFSampler : SamplerBase
    {
        private int _minibatchSize;
        private List<StreamConfiguration> _streamConfigurations;

        private MinibatchSource _minibatchSource;
        private Dictionary<string, StreamInformation> _streamInfos;

        public int MinibatchSize => _minibatchSize;
        public IList<StreamConfiguration> StreamConfigurations => _streamConfigurations;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var sc in _streamConfigurations)
                    sc.Dispose();

                foreach (var si in _streamInfos.Values)
                    si.Dispose();

                _minibatchSource.Dispose();
            }
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

        public override Minibatch GetNextMinibatch(DeviceDescriptor device = null)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            var minibatchMap = _minibatchSource.GetNextMinibatch((uint)MinibatchSize, device);

            var minibatch = new Minibatch();

            foreach (var info in _streamInfos)
                minibatch.Add(info.Key, minibatchMap[info.Value]);

            return minibatch;
        }
    }
}
