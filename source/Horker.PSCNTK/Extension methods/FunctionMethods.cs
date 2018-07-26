using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Collections;

namespace Horker.PSCNTK
{
    public class FunctionMethods
    {
        public static CNTK.Variable Get(PSObject func, string name)
        {
            var f = (CNTK.Function)func.BaseObject;

            var vas = f.Inputs.Where(v => v.Name == name).ToArray();
            if (vas.Length > 0)
                return vas[0];

            return null;
        }

        public static CNTK.Value Invoke(PSObject func, Hashtable Arguments = null, CNTK.DeviceDescriptor device = null)
        {
            var f = (CNTK.Function)func.BaseObject;

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
                    var va = Get(func, entry.Key.ToString());
                    if (va == null)
                        throw new ArgumentException(string.Format("Unknown argument key '{0}'", entry.Key.ToString()));

                    key = va;
                }

                value = Converter.ToValue(entry.Value);

                inputs.Add(key, value);
            }

            // TODO: multiple outputs
            var output = new Dictionary<CNTK.Variable, CNTK.Value>();

            output.Add(f.Output, null);

            if (device == null)
                device = CNTK.DeviceDescriptor.UseDefaultDevice();

            f.Evaluate(inputs, output, true, device);

            return output[f.Output];
        }
    }
}
