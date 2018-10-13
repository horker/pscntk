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
        private bool _showValue;

        private Hashtable _arguments;
        private Minibatch _minibatch;
        private DataNameToInputMap _dataNameToInputMap;

        private bool _shouUid;

        private StringBuilder _output;

        public string Result { get => _output.ToString(); }

        public FunctionAsTree(Function func, bool showValue, Hashtable arguments = null, Minibatch minibatch = null, DataNameToInputMap dataNameToInputMap = null, bool showUid = true)
        {
            _showValue = showValue;

            _arguments = arguments;
            _minibatch = minibatch;
            _dataNameToInputMap = dataNameToInputMap;

            _shouUid = showUid;

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
                    Value[] values;
                    if (_minibatch == null)
                        values = FunctionInvoke.Invoke(func, _arguments, null, false);
                    else
                        values = FunctionInvoke.Invoke(func, _minibatch, _dataNameToInputMap, null, false);

                    foreach (var value in values)
                    {
                        var ds = DataSourceFactory.FromValue(value);
                        putDataSource(ds, indent);
                    }
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
                var ds = DataSourceFactory.FromVariable(va);
                putDataSource(ds, indent);
            }

            return true;
        }

        public void Complete()
        {
            // Do nothing
        }

        private void putDataSource(IDataSource<float> ds, string indent)
        {
            var shape = ds.Shape;
            _output.AppendFormat("{0}    {1}\r\n", indent, Converter.ArrayToString("->", ds.Data, shape, false));
        }
    }
}