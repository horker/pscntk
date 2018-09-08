using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using CNTK;

namespace Horker.PSCNTK
{
    public class FunctionAsTree : INodeWalker
    {
        private Hashtable _arguments;
        private Minibatch _minibatch;
        private DataNameToInputMap _dataNameToInputMap;

        private bool _shouUid;
        private bool _showValue;

        private StringBuilder _output;

        public string Result { get => _output.ToString(); }

        public FunctionAsTree(Function func, Hashtable arguments = null, Minibatch minibatch = null, DataNameToInputMap dataNameToInputMap = null, bool showUid = true, bool showValue = true)
        {
            _arguments = arguments;
            _minibatch = minibatch;
            _dataNameToInputMap = dataNameToInputMap;

            _shouUid = showUid;
            _showValue = showValue;

            _output = new StringBuilder();

            new NodeWalk(func, this);
        }

        public bool ProcessFunction(Function func, int depth)
        {
            var indent = new string(' ', depth * 2);

            string name;
            if (_shouUid)
                name = "<" + (string.IsNullOrEmpty(func.Name) ? func.Uid : func.Name + ":" + func.Uid) + ">";
            else
                name = string.IsNullOrEmpty(func.Name) ? "" : "<" + func.Name + ">";

            _output.AppendFormat("{0}{1} {2} {3}\r\n", indent, depth, func.OpName, name);

            if (_showValue)
            {
                try
                {
                    Value value;
                    if (_minibatch == null)
                        value = FunctionInvoke.Invoke(func, _arguments, null, false);
                    else
                        value = FunctionInvoke.Invoke(func, _minibatch, _dataNameToInputMap, null, false);

                    var ds = DataSource<float>.FromValue(value);
                    putDataSource(ds, indent);
                }
                catch (Exception)
                {
                    // Pass
                }
            }

            return true;
        }

        public bool ProcessVariable(Function holder, Variable va, int depth, bool visited)
        {
            var indent = new string(' ', depth * 2);
            var v = visited ? " *" : "";
            var shape = string.Join(" x ", va.Shape.Dimensions);

            string name;
            if (_shouUid)
                name = " <" + (string.IsNullOrEmpty(va.Name) ? va.Uid : va.Name + ":" + va.Uid) + ">";
            else
                name = string.IsNullOrEmpty(va.Name) ? "" : " <" + va.Name + ">";

            _output.AppendFormat("{0}{1} @{2} [{3}]{4}{5}\r\n", indent, depth, va.Kind, shape, name, v);

            if (_showValue && (va.IsParameter || va.IsConstant))
            {
                var ds = DataSource<float>.FromVariable(va);
                putDataSource(ds, indent);
            }

            return true;
        }

        public void Complete()
        {
            // Do nothing
        }

        private void putDataSource(DataSource<float> ds, string indent)
        {
            var shape = ds.Shape;

            if (shape.GetSize(-1) <= 5)
                _output.AppendFormat("{0}    {1}\r\n", indent, Converter.ArrayToString("->", ds.Data, shape, false));
            else
            {
                var seg = new ArraySegment<float>(ds.Data, 0, 5);
                var s = string.Join(" ", seg.Select(x => string.Format("{0:0.#####}", x)));
                _output.AppendFormat("{0}    -> {1} [{2}...]\r\n", indent, shape, s);
            }
        }
    }
}