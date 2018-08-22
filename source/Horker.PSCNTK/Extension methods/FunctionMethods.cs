﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Collections;

namespace Horker.PSCNTK
{
    public class FunctionMethods
    {
        public static CNTK.Variable Find(PSObject func, string name)
        {
            var f = (CNTK.Function)func.BaseObject;
            return CNTKFunctionHelper.Find(f, name);
        }

        public static CNTK.Value Invoke(PSObject func, Hashtable Arguments = null, CNTK.DeviceDescriptor device = null)
        {
            var f = (CNTK.Function)func.BaseObject;
            return CNTKFunctionHelper.Invoke(f, Arguments, device);
        }

        public static string AsTree(PSObject func, bool detailed = false)
        {
            var f = (CNTK.Function)func.BaseObject;
            var w = new FunctionAsTree(f, detailed);
            return w.Result;
        }
    }
}
