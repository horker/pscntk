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
        public static object Find(PSObject func, string name)
        {
            var f = func.BaseObject as Function;
            var w = new FunctionFind(f, name, false, false);
            return w.Results[0];
        }

        public static object FindAll(PSObject func, string name)
        {
            var f = func.BaseObject as Function;
            var w = new FunctionFind(f, name, true, false);
            return w.Results;
        }

        public static Value Invoke(PSObject func, object arguments = null, DataNameToInputMap map = null)
        {
            var f = func.BaseObject as Function;

            if (arguments is Dictionary<Variable, Value>)
                return FunctionInvoke.Invoke(f, arguments as Dictionary<Variable, Value>, null, false);

            if (arguments is Hashtable)
                return FunctionInvoke.Invoke(f, arguments as Hashtable, null, false);

            return FunctionInvoke.Invoke(f, arguments as Minibatch, map, null, false);
        }

        public static string AsTree(PSObject func, object arguments = null, DataNameToInputMap map = null, bool showUid = false, bool showValue = true)
        {
            var f = func.BaseObject as Function;
            FunctionAsTree w;
            if (arguments is Hashtable)
                w = new FunctionAsTree(f, arguments as Hashtable, showUid, showValue);
            else
                w = new FunctionAsTree(f, arguments as Minibatch, map, showUid, showValue);

            return w.Result;
        }

        public static string ToDot(PSObject func)
        {
            var f = func.BaseObject as Function;
            var g = new DotGenerator(f);
            return g.Result;
        }
    }
}
