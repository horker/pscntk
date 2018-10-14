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

            if (arguments is IDictionary<string, IDataSource<float>> sddic)
                return FunctionInvoke.Invoke(f, sddic, null, false);

            if (arguments is DataSourceSet dss)
                return FunctionInvoke.Invoke(f, dss, null, false);

            throw new ArgumentException("Invalid type: arguments");
        }

        public static string AsTree(PSObject func)
        {
            Function f = ToFunction(func);
            FunctionAsTree w = new FunctionAsTree(f, false);
            return w.Result;
        }

        public static string AsTreeWithValues(PSObject func, object arguments = null, DataNameToInputMap map = null, bool showUid = true)
        {
            Function f = ToFunction(func);
            FunctionAsTree w;

            if (arguments == null)
                w = new FunctionAsTree(f, true, null, null, null);
            else if (arguments is Hashtable)
                w = new FunctionAsTree(f, true, arguments as Hashtable, null, null, showUid);
            else
                w = new FunctionAsTree(f, true, null, arguments as Minibatch, map, showUid);

            return w.Result;
        }

        public static IEnumerable<NodeInfo> GetNodeInfo(PSObject func)
        {
            Function f = ToFunction(func);
            var w = new FunctionGetNodeInfo(f, false);
            return w.GetNodeInfo();
        }

        public static IEnumerable<NodeInfo> GetNodeInfoWithValues(PSObject func, object arguments = null, DataNameToInputMap map = null)
        {
            Function f = ToFunction(func);
            FunctionGetNodeInfo w;

            if (arguments == null)
                w = new FunctionGetNodeInfo(f, true, null, null);
            else if (arguments is Hashtable)
                w = new FunctionGetNodeInfo(f, true, arguments as Hashtable);
            else
                w = new FunctionGetNodeInfo(f, true, null, arguments as Minibatch, map);

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
