using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Horker.PSCNTK
{
    // <summary>
    // CNTK Text Format file builder for series data.
    // </summary>
    public class SeriesCTFBuilder
    {
        private int _lookback;

        private IDictionary<string, IReadOnlyList<float>> _featureMap;
        private IDictionary<string, IReadOnlyList<float>> _labelMap;

        public SeriesCTFBuilder(int lookback)
        {
            if (lookback < 0) {
                throw new ArgumentException("lookback should not be negative");
            }

            _lookback = lookback;

            _featureMap = new Dictionary<string, IReadOnlyList<float>>();
            _labelMap = new Dictionary<string, IReadOnlyList<float>>();
        }

        public void AddFeature(string name, IReadOnlyList<float> data)
        {
            CNTKBuilderHelper.CheckFeatureName(name);
            _featureMap.Add(name, data);
        }

        public void AddLabel(string name, IReadOnlyList<float> data)
        {
            CNTKBuilderHelper.CheckFeatureName(name);
            _labelMap.Add(name, data);
        }

        public void Write(TextWriter writer)
        {
            var builder = new CTFBuilder(writer, false);

            int total = int.MaxValue;
            foreach (var d in _featureMap) {
                total = Math.Min(total, d.Value.Count);
            }
            foreach (var d in _labelMap) {
                total = Math.Min(total, d.Value.Count);
            }

            for (var seq = 0; seq < total - _lookback + 1; ++seq) {
                for (var i = 0; i < _lookback; ++i) {
                    foreach (var d in _featureMap) {
                        builder.AddDenseSample(d.Key, new float[] { d.Value[seq + i] });
                    }

                    if (i == _lookback - 1) {
                        foreach (var d in _labelMap) {
                            builder.AddDenseSample(d.Key, new float[] { d.Value[seq + i] });
                        }
                    }

                    builder.NextLine();
                }

                builder.NextSequence();
            }

            builder.Finish();
        }
    }
}
