using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;

namespace Horker.PSCNTK
{
    public class CNTKFunctionHelper
    {
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
                    var va = FunctionFind.FindVariable(func, entry.Key.ToString());
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
    }
}