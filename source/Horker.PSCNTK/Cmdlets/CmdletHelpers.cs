﻿using System;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    class CmdletHelpers
    {
        public static Function GetFunctionInstance(object func, object[] arguments, string displayName)
        {
            if (func == null)
                return null;

            if (func is PSObject psobj)
                func = psobj.BaseObject;

            if (func is Function fn)
                return fn;

            if (func is WrappedFunction wf)
                return wf;

            if (func is string)
            {
                var f = func as string;
                if (string.IsNullOrEmpty(f))
                    return null;

                var lossMethod = Helpers.GetCNTKLibMethod(f, arguments.Length);
                return (Function)lossMethod.Invoke(null, arguments);
            }

            throw new ArgumentException(displayName + " should be an instance of Function or a function name");
        }
    }
}
