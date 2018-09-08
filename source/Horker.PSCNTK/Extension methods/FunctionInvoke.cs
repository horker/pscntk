using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    public class FunctionInvoke
    {
        public static Value Invoke(Function func, Dictionary<Variable, Value> inputs = null, DeviceDescriptor device = null, bool errorWhenArgumentUnused = true)
        {
            if (inputs == null)
                inputs = new Dictionary<Variable, Value>();

            if (!func.IsComposite)
                func = Function.AsComposite(func);

            // TODO: multiple outputs
            var output = new Dictionary<Variable, Value>();
            output.Add(func.Output, null);

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            func.Evaluate(inputs, output, true, device);

            return output[func.Output];
        }

        public static Value Invoke(Function func, Hashtable arguments, DeviceDescriptor device = null, bool errorWhenArgumentUnused = true)
        {
            if (arguments == null)
                arguments = new Hashtable();

            var inputs = new Dictionary<Variable, Value>();

            foreach (DictionaryEntry entry in arguments)
            {
                Variable key;
                Value value;

                var entryKey = entry.Key;
                if (entryKey is PSObject)
                    entryKey = (entryKey as PSObject).BaseObject;

                if (entryKey is Variable)
                    key = entryKey as Variable;
                else if (entryKey is WrappedVariable)
                    key = entryKey as WrappedVariable;
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

            return Invoke(func, inputs, device, errorWhenArgumentUnused);
        }

        public static Value Invoke(Function func, Minibatch batch, DataNameToInputMap map = null, DeviceDescriptor device = null, bool errorWhenArgumentUnused = true)
        {
            if (map == null)
                map = new DataNameToInputMap(new Function[] { func });

            map.InitializeByMinibatch(batch);

            var inputs = map.GetVariableValueMap(batch);

            return Invoke(func, inputs, device, errorWhenArgumentUnused);
        }
    }
}