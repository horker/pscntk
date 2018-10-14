using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    public static class FunctionInvoke
    {
        public static Value[] Invoke(this Function func, Dictionary<Variable, Value> inputs = null, DeviceDescriptor device = null, bool errorWhenArgumentUnused = true)
        {
            if (inputs == null)
                inputs = new Dictionary<Variable, Value>();

            if (!func.IsComposite)
                func = Function.AsComposite(func);

            var outputMap = new Dictionary<Variable, Value>();
            foreach (var output in func.Outputs)
                outputMap.Add(output, null);

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            func.Evaluate(inputs, outputMap, true, device);

            var results = new Value[func.Outputs.Count];
            for (var i = 0; i < func.Outputs.Count; ++i)
                results[i] = outputMap[func.Outputs[i]];

            return results;
        }

        public static Value[] Invoke(this Function func, Dictionary<string, Value> arguments, DeviceDescriptor device = null, bool errorWhenArgumentUnused = true)
        {
            var inputs = new Dictionary<Variable, Value>();

            foreach (var entry in arguments)
            {
                var key = FunctionFind.FindVariable(func, entry.Key);
                if (key == null)
                {
                    if (errorWhenArgumentUnused)
                        throw new ArgumentException(string.Format("Unknown argument key '{0}'", entry.Key));
                    else
                        continue;
                }

                inputs.Add(key, entry.Value);
            }

            return Invoke(func, inputs, device, errorWhenArgumentUnused);
        }

        public static Value[] Invoke(this Function func, Hashtable arguments, DeviceDescriptor device = null, bool errorWhenArgumentUnused = true)
        {
            if (arguments == null)
                arguments = new Hashtable();

            var inputs = new Dictionary<Variable, Value>();

            foreach (DictionaryEntry entry in arguments)
            {
                Variable key;
                Value value;

                var entryKey = entry.Key;
                if (entryKey is PSObject psobj)
                    entryKey = psobj.BaseObject;

                if (entryKey is Variable v)
                    key = v;
                else if (entryKey is WrappedVariable wv)
                    key = wv;
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

                value = Converter.ToValue(entry.Value, key.Shape.Dimensions.ToArray());

                inputs.Add(key, value);
            }

            return Invoke(func, inputs, device, errorWhenArgumentUnused);
        }

        public static Value[] Invoke(this Function func, IDictionary<string, IDataSource<float>> dataSet, DeviceDescriptor device = null, bool errorWhenArgumentUnused = true)
        {
            var inputs = new Dictionary<Variable, Value>();

            foreach (var entry in dataSet)
            {
                Variable key;
                Value value;

                var va = FunctionFind.FindVariable(func, entry.Key); ;
                if (va == null)
                {
                    if (errorWhenArgumentUnused)
                        throw new ArgumentException(string.Format("Unknown argument key '{0}'", entry.Key));
                    else
                        continue;
                }

                key = va;

                value = entry.Value.ToValue();

                inputs.Add(key, value);
            }

            return Invoke(func, inputs, device, errorWhenArgumentUnused);
        }

        public static Value[] Invoke(this Function func, DataSourceSet dataSet, DeviceDescriptor device = null, bool errorWhenArgumentUnused = true)
        {
            return Invoke(func, (IDictionary<string, IDataSource<float>>)dataSet.Features, device, errorWhenArgumentUnused);
        }

        public static Value[] Invoke(this Function func, Minibatch batch, DataNameToInputMap map = null, DeviceDescriptor device = null, bool errorWhenArgumentUnused = true)
        {
            if (map == null)
                map = new DataNameToInputMap(new Function[] { func });

            map.InitializeByMinibatch(batch);

            var inputs = map.GetVariableValueMap(batch);

            return Invoke(func, inputs, device, errorWhenArgumentUnused);
        }
    }
}