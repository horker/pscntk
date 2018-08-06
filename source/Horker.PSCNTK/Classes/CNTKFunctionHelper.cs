using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace Horker.PSCNTK
{
    public class CNTKFunctionHelper
    {
        public static CNTK.Variable Find(CNTK.Function func, string name)
        {
            var vas = func.Inputs.Where(v => v.Name == name || v.Uid == name).ToArray();
            if (vas.Length > 0)
                return vas[0];

            return null;
        }

        public static CNTK.Value Invoke(CNTK.Function func, Hashtable Arguments = null, CNTK.DeviceDescriptor device = null)
        {
            if (Arguments == null)
                Arguments = new Hashtable();

            var inputs = new Dictionary<CNTK.Variable, CNTK.Value>();

            foreach (DictionaryEntry entry in Arguments)
            {
                CNTK.Variable key;
                CNTK.Value value;

                if (entry.Key is CNTK.Variable)
                    key = entry.Key as CNTK.Variable;
                else
                {
                    var va = Find(func, entry.Key.ToString());
                    if (va == null)
                        throw new ArgumentException(string.Format("Unknown argument key '{0}'", entry.Key.ToString()));

                    key = va;
                }

                value = Converter.ToValue(entry.Value);

                inputs.Add(key, value);
            }

            // TODO: multiple outputs
            var output = new Dictionary<CNTK.Variable, CNTK.Value>();
            output.Add(func.Output, null);

            if (device == null)
                device = CNTK.DeviceDescriptor.UseDefaultDevice();

            func.Evaluate(inputs, output, true, device);

            return output[func.Output];
        }

        static public string AsTree(CNTK.Function func)
        {
            var visitedVariables = new HashSet<string>();
            var output = new StringBuilder();

            AsTreeInternal(func, output, visitedVariables, 0);

            return output.ToString();
        }

        static private void AsTreeInternal(object node, StringBuilder output, HashSet<string> visitedVariables, int depth)
        {
            var indent = new string(' ', depth * 2);

            if (node is CNTK.Function)
            {
                var func = node as CNTK.Function;
                var name = string.IsNullOrEmpty(func.Name) ? func.Uid : func.Name + ":" + func.Uid;
                var args = string.Join(", ", func.Arguments.Select(arg => string.IsNullOrEmpty(arg.Name) ? arg.Uid : arg.Name));

                var visited = visitedVariables.Contains(func.Uid);
                var v = visited ? " *" : "";

                output.AppendFormat("{0}{1} {2}({3}) <{4}>{5}\r\n", indent, depth, func.OpName, args, name, v);

                if (visited)
                    return;

                foreach (var arg in func.Arguments)
                    AsTreeInternal(arg, output, visitedVariables, depth + 1);
            }
            else if (node is CNTK.Variable)
            {
                var va = node as CNTK.Variable;
                var name = string.IsNullOrEmpty(va.Name) ? va.Uid : va.Name + ":" + va.Uid;

                var visited = visitedVariables.Contains(va.Uid);
                var v = visited ? " *" : "";

                var shape = string.Join("x", va.Shape.Dimensions);
                output.AppendFormat("{0}{1} @{2} [{3}] <{4}>{5}\r\n", indent, depth, va.Kind, shape, name, v);

                if (visited)
                    return;

                visitedVariables.Add(va.Uid);

                if (va.Owner != null)
                    AsTreeInternal(va.Owner, output, visitedVariables, depth + 1);
            }
            else
            {
                output.AppendFormat("unknown: {0}\r\n", node.GetType().FullName);
            }
        }
    }
}