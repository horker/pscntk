using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Collections;
using CNTK;

namespace Horker.PSCNTK
{
    public class FunctionMethods
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

        public static Value Invoke(PSObject func, object arguments = null, DataNameToInputMap map = null)
        {
            Function f = ToFunction(func);

            if (arguments == null)
                return FunctionInvoke.Invoke(f, new Dictionary<Variable, Value>(), null, false);

            if (arguments is Dictionary<Variable, Value>)
                return FunctionInvoke.Invoke(f, arguments as Dictionary<Variable, Value>, null, false);

            if (arguments is Hashtable)
                return FunctionInvoke.Invoke(f, arguments as Hashtable, null, false);

            return FunctionInvoke.Invoke(f, arguments as Minibatch, map, null, false);
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
