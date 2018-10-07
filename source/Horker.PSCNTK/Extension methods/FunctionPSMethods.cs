using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Collections;
using CNTK;

namespace Horker.PSCNTK
{
    public class FunctionPSMethods
    {
        private static Function ToFunction(PSObject func)
        {
            if (func.BaseObject is WrappedFunction)
                return func.BaseObject as WrappedFunction;
            else
                return func.BaseObject as Function;
        }

        public static WrappedVariable Find(PSObject func, string name)
        {
            Function f = ToFunction(func);

            var w = new FunctionFind(f, name, false, false);
            return w.Results[0];
        }

        public static WrappedVariable[] FindAll(PSObject func, string name)
        {
            Function f = ToFunction(func);

            var w = new FunctionFind(f, name, true, false);
            return w.Results.Select(x => (WrappedVariable)x).ToArray();
        }

        public static Value[] Invoke(PSObject func, object arguments = null, DataNameToInputMap map = null)
        {
            Function f = ToFunction(func);

            if (arguments == null)
                return FunctionInvoke.Invoke(f, new Dictionary<Variable, Value>(), null, false);

            if (arguments is Dictionary<Variable, Value> vvdic)
                return FunctionInvoke.Invoke(f, vvdic, null, false);

            if (arguments is Dictionary<string, Value> svdic)
                return FunctionInvoke.Invoke(f, svdic, null, false);

            if (arguments is Hashtable ht)
                return FunctionInvoke.Invoke(f, ht, null, false);

            if (arguments is Minibatch mb)
                return FunctionInvoke.Invoke(f, mb, map, null, false);

            throw new ArgumentException("Invalid type: arguments");
        }

        public static string AsTree(PSObject func, object arguments = null, DataNameToInputMap map = null, bool showUid = true, bool showValue = true)
        {
            Function f = ToFunction(func);
            FunctionAsTree w;

            if (arguments == null)
                w = new FunctionAsTree(f, null, null, null, showUid, showValue);
            else if (arguments is Hashtable)
                w = new FunctionAsTree(f, arguments as Hashtable, null, null, showUid, showValue);
            else
                w = new FunctionAsTree(f, null, arguments as Minibatch, map, showUid, showValue);

            return w.Result;
        }

        public static IEnumerable<NodeInfo> GetNodeInfo(PSObject func, object arguments = null, DataNameToInputMap map = null)
        {
            Function f = ToFunction(func);
            var w = new FunctionGetNodeInfo(f);

            if (arguments == null)
                w = new FunctionGetNodeInfo(f, null, null);
            else if (arguments is Hashtable)
                w = new FunctionGetNodeInfo(f, arguments as Hashtable);
            else
                w = new FunctionGetNodeInfo(f, null, arguments as Minibatch, map);

            return w.GetNodeInfo();
        }

        public static string ToDot(PSObject func)
        {
            Function f = ToFunction(func);
            var g = new DotGenerator(f);
            return g.Result;
        }
    }
}
