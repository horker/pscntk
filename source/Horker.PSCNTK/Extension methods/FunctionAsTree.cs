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
        private StringBuilder _output;
        private bool _shouUid;
        private bool _showValue;

        public string Result { get => _output.ToString(); }

        public FunctionAsTree(CNTK.Function func, bool showUid = false, bool showValue = true)
        {
            _output = new StringBuilder();
            _shouUid = showUid;
            _showValue = showValue;

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

            return true;
        }

        public bool ProcessVariable(Variable va, int depth, bool visited)
        {
            var indent = new string(' ', depth * 2);
            var v = visited ? " *" : "";
            var shape = string.Join("x", va.Shape.Dimensions);

            string name;
            if (_shouUid)
                name = " <" + (string.IsNullOrEmpty(va.Name) ? va.Uid : va.Name + ":" + va.Uid) + ">";
            else
                name = string.IsNullOrEmpty(va.Name) ? "" : " <" + va.Name + ">";

            _output.AppendFormat("{0}{1} @{2} [{3}]{4}{5}\r\n", indent, depth, va.Kind, shape, name, v);

            if (_showValue && (va.IsParameter || va.IsConstant))
            {
                var ds = DataSource<float>.FromVariable(va);
                if (ds.Shape.GetSize(-1) <= 5)
                    _output.AppendFormat("{0}    {1}\n", indent, Converter.ArrayToString("Value", ds.Data, ds.Shape, false));
                else
                {
                    var seg = new ArraySegment<float>(ds.Data, 0, 5);
                    var s = string.Join(" ", seg.Select(x => string.Format("{0:0.#####}", x)));
                    _output.AppendFormat("{0}    Value [{1}] [{2} ...]\n", indent, shape, s);
                }
            }

            return true;
        }
    }
}