using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    public class CNTKFunctionHelper
    {
        public static Value Invoke(Function func, Hashtable Arguments = null, DeviceDescriptor device = null, bool errorWhenArgumentUnused = true)
        {
            if (Arguments == null)
                Arguments = new Hashtable();

            if (!func.IsComposite)
                func = Function.AsComposite(func);

            var inputs = new Dictionary<Variable, Value>();

            foreach (DictionaryEntry entry in Arguments)
            {
                Variable key;
                Value value;

                var entryKey = entry.Key;
                if (entryKey is PSObject)
                    entryKey = (entryKey as PSObject).BaseObject;

                if (entryKey is Variable)
                    key = entryKey as Variable;
                else
                {
                    var va = FunctionFind.FindVariable(func, entryKey.ToString());
                    if (va == null)
                    {
                        if (errorWhenArgumentUnused)
                            throw new ArgumentException(string.Format("Unknown argument key '{0}'", entryKey.ToString()));
                        else
                            continue;
                    }

                    key = va;
                }

                value = Converter.ToValue(entry.Value);

                inputs.Add(key, value);
            }

            // TODO: multiple outputs
            var output = new Dictionary<Variable, Value>();
            output.Add(func.Output, null);

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            func.Evaluate(inputs, output, true, device);

            return output[func.Output];
        }
    }
}