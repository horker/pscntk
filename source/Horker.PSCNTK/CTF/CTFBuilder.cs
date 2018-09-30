using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CNTK;
using System.Linq;

namespace Horker.PSCNTK
{
    internal class CNTKBuilderHelper
    {
        public static bool IsValidFeatureName(string name)
        {
            if (string.IsNullOrEmpty(name) || ((IEnumerable<char>) name).Any(c => char.IsWhiteSpace(c))) {
                return false;
            }

            return true;
        }

        public static void CheckFeatureName(string name)
        {
            if (!IsValidFeatureName(name)) {
                throw new ArgumentException("name shoud not be empty nor contain whitespaces");
            }
        }
    }

    internal abstract class Sample
    {
        private String _name;

        public Sample(string name)
        {
            _name = name;
        }

        public virtual void Write(TextWriter s)
        {
            s.Write("\t|");
            s.Write(_name);
        }
    }

    internal class DenseSample : Sample
    {
        private List<float> _values;

        public DenseSample(string name, IEnumerable<float> values = null)
            : base(name)
        {
            _values = new List<float>();
            if (values != null) {
                _values.AddRange(values);
            }
        }

        public void AddValue(float value)
        {
            _values.Add(value);
        }

        public override void Write(TextWriter s)
        {
            if (_values == null) {
                return;
            }

            base.Write(s);

            foreach (var v in _values) {
                s.Write(' ');
                s.Write(v.ToString("g"));
            }
        }
    }

    internal class IndexValue
    {
        public int Index;
        public float Value;

        public IndexValue(int index, float value)
        {
            Index = index;
            Value = value;
        }
    }

    internal class SparseSample : Sample
    {
        private List<IndexValue> _values;

        public SparseSample(string name)
            : base(name)
        {
            _values = new List<IndexValue>();
        }

        public void AddValue(int index, float value)
        {
            _values.Add(new IndexValue(index, value));
        }

        public override void Write(TextWriter s)
        {
            base.Write(s);

            foreach (var v in _values) {
                s.Write(' ');
                s.Write(v.Index);
                s.Write(':');
                s.Write(v.Value.ToString("g"));
            }
        }
    }

    internal class CommentSample : Sample
    {
        public CommentSample(string comment)
            : base("# " + comment)
        { }

        public override void Write(TextWriter s)
        {
            base.Write(s);
        }
    }

    internal class OneHotSample : Sample
    {
        private int _total;
        private int _value;

        public OneHotSample(string name, int total, int value)
            : base(name)
        {
            _total = total;
            _value = value;
        }

        public override void Write(TextWriter s)
        {
            base.Write(s);

            for (var i = 0; i < _total; ++i) {
                if (i == _value) {
                    s.Write(" 1");
                }
                else {
                    s.Write(" 0");
                }
            }
        }
    }

    // ref.
    // https://docs.microsoft.com/en-us/cognitive-toolkit/brainscript-cntktextformat-reader
    public class CTFBuilder
    {
        private int _seq;
        private bool _seqAutoIncrement;
        private bool _bol;
        private bool _first;

        private Sample _sample;

        private TextWriter _writer;

        private bool _finished;

        public string NEWLINE = "\r\n";

        public TextWriter Writer { get; }

        public CTFBuilder(TextWriter writer, int initialSeq = 0, bool seqAutoIncrement = true)
        {
            _seq = initialSeq;
            _seqAutoIncrement = seqAutoIncrement;
            _bol = true;
            _first = true;

            _sample = null;

            _writer = writer;

            _finished = false;
        }

        public void NextLine()
        {
            FlushSample();

            _bol = true;
            if (_seqAutoIncrement) {
                ++_seq;
            }

        }

        public void NextSequence()
        {
            ++_seq;
        }

        public void Finish()
        {
            if (_finished) {
                return;
            }

            if (_sample != null) {
                NextLine();
            }

            _writer.Flush();

            _finished = true;
        }

        private void FlushSample()
        {
            if (_sample == null) {
                return;
            }

            if (_bol) {
                if (!_first) {
                    _writer.Write(NEWLINE);
                }
                _first = false;
                _writer.Write(_seq);
                _bol = false;
            }

            _sample.Write(_writer);

            _sample = null;
        }

        public void AddDenseSample(string name, IEnumerable<float> values = null)
        {
            FlushSample();
            _sample = new DenseSample(name, values);
        }

        public void AddDenseValue(float value)
        {
            ((DenseSample)_sample).AddValue(value);
        }

        public void AddSparseSample(string name)
        {
            FlushSample();

            _sample = new SparseSample(name);
        }

        public void AddSparseValue(int index, float value)
        {
            ((SparseSample)_sample).AddValue(index, value);
        }

        public void AddComment(string comment)
        {
            FlushSample();

            _sample = new CommentSample(comment);
        }

        public void AddOneHotSample(string name, int total, int value)
        {
            FlushSample();

            _sample = new OneHotSample(name, total, value);
        }

    }
}
