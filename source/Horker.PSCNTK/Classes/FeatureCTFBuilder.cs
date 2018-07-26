using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Horker.PSCNTK
{
    internal abstract class FeatureBase
    {
        protected string _name;

        public FeatureBase(string name)
        {
            CNTKBuilderHelper.CheckFeatureName(name);
            _name = name;
        }

        public abstract bool Write(CTFBuilder builder);
    }

    internal class DenseFeatureBySequence : FeatureBase
    {
        private IEnumerable<float> _data;
        private int _dimension;
        private IEnumerator<float> _cursor;

        public DenseFeatureBySequence(string name, IEnumerable<float> data, int dimension)
            : base(name)
        {
            if (dimension <= 0) {
                throw new ArgumentException("dimension should be greater than or equal to 0");
            }

            _data = data;
            _dimension = dimension;
            _cursor = null;
        }

        public override bool Write(CTFBuilder builder)
        {
            if (_cursor == null) {
                _cursor = _data.GetEnumerator();
            }

            bool first = true;
            for (var i = 0; i < _dimension; ++i) {
                if (_cursor.MoveNext() == false) {
                    return false;
                }

                if (first) {
                    builder.AddDenseSample(_name);
                }
                first = false;

                builder.AddDenseValue(_cursor.Current);
            }

            return true;
        }
    }

    internal class DenseFeatureByArrayOfArray : FeatureBase
    {
        private List<List<float>> _data;
        private IEnumerator<List<float>> _cursor;

        public DenseFeatureByArrayOfArray(string name)
            : base(name)
        {
            _data = new List<List<float>>();
            _cursor = null;
        }

        public void AddSample(IEnumerable<float> sample)
        {
            var l = new List<float>(sample);
            _data.Add(l);
        }

        public void AddAllSamples(float[][] samples)
        {
            for (var i = 0; i < samples.Length; ++i) {
                var l = new List<float>(samples[i]);
                _data.Add(l);
            }
        }

        public override bool Write(CTFBuilder builder)
        {
            if (_cursor == null) {
                _cursor = _data.GetEnumerator();
            }

            if (_cursor.MoveNext() == false) {
                return false;
            }

            builder.AddDenseSample(_name, _cursor.Current);

            return true;
        }
    }

    internal class SparseFeature : FeatureBase
    {
        private List<List<Tuple<int, float>>> _data;
        private IEnumerator<List<Tuple<int, float>>> _cursor;

        public SparseFeature(string name)
            : base(name)
        {
            _data = new List<List<Tuple<int, float>>>();
            _cursor = null;
        }

        public void AddNewSample()
        {
            _data.Add(new List<Tuple<int, float>>());
        }

        public void AddValue(int index, float value)
        {
            if (index < 0) {
                throw new ArgumentException("index should be greater than or equal to 0");
            }

            _data.Last().Add(new Tuple<int, float>(index, value));
        }

        public override bool Write(CTFBuilder builder)
        {
            if (_cursor == null) {
                _cursor = _data.GetEnumerator();
            }

            if (_cursor.MoveNext() == false) {
                return false;
            }

            builder.AddSparseSample(_name);

            var sample = _cursor.Current;
            foreach (var e in sample) {
                builder.AddSparseValue(e.Item1, e.Item2);
            }

            return true;
        }
    }

    internal class OneHotFeature : FeatureBase
    {
        private IEnumerable<int> _data;
        private int _classCount;
        private IEnumerator<int> _cursor;

        public OneHotFeature(string name, IEnumerable<int> data, int classCount)
            : base(name)
        {
            if (classCount <= 0) {
                throw new ArgumentException("number of class should be greater than or equal to 1");
            }

            _data = data;
            _classCount = classCount;
        }

        public override bool Write(CTFBuilder builder)
        {
            if (_cursor == null) {
                _cursor = _data.GetEnumerator();
            }

            if (_cursor.MoveNext() == false) {
                return false;
            }

            builder.AddOneHotSample(_name, _classCount, _cursor.Current);

            return true;
        }
    }

    internal class CommentFeature : FeatureBase
    {
        private IEnumerable<string> _data;
        private IEnumerator<string> _cursor;

        public CommentFeature(IEnumerable<string> data)
            : base("foo")
        {
            _data = data;
        }

        public override bool Write(CTFBuilder builder)
        {
            if (_cursor == null) {
                _cursor = _data.GetEnumerator();
            }

            if (_cursor.MoveNext() == false) {
                return false;
            }

            builder.AddComment(_cursor.Current);

            return true;
        }
    }

    // <summary>
    // CNTK Text Format file builder for features.
    // </summary>
    public class FeatureCTFBuilder
    {
        private List<FeatureBase> _features;

        public FeatureCTFBuilder()
        {
            _features = new List<FeatureBase>();
        }

        public void AddDenseFeature(string name, IReadOnlyList<float> data, int dimension)
        {
            _features.Add(new DenseFeatureBySequence(name, data, dimension));
        }

        public void AddDenseFeature(string name, float[][] data)
        {
            var l = new DenseFeatureByArrayOfArray(name);
            l.AddAllSamples(data);
            _features.Add(l);
        }

        public void AddDenseFeature(string name)
        {
            var l = new DenseFeatureByArrayOfArray(name);
            _features.Add(l);
        }

        public void AddDenseSample(IEnumerable<float> sample)
        {
            var l = _features.Last();
            ((DenseFeatureByArrayOfArray)l).AddSample(sample);
        }

        public void AddSparseFeature(string name)
        {
            _features.Add(new SparseFeature(name));
        }

        public void StartNewSparseSample()
        {
            var l = _features.Last();
            ((SparseFeature)l).AddNewSample();
        }

        public void AddSparseValue(int index, float value)
        {
            var l = _features.Last();
            ((SparseFeature)l).AddValue(index, value);
        }

        public void AddOneHotFeature(string name, IEnumerable<int> data, int classCount)
        {
            _features.Add(new OneHotFeature(name, data, classCount));
        }

        public void AddComment(IEnumerable<string> comment)
        {
            _features.Add(new CommentFeature(comment));
        }

        public void Write(TextWriter writer)
        {
            var builder = new CTFBuilder(writer, true);

            var completed = new List<FeatureBase>();
            while (true) {
                foreach (var f in _features) {
                    if (completed.Contains(f)) {
                        continue;
                    }

                    if (f.Write(builder) == false) {
                        completed.Add(f);
                        if (completed.Count == _features.Count) {
                            builder.Finish();
                            return;
                        }
                    }
                }

                builder.NextLine();
            }
        }
    }
}
