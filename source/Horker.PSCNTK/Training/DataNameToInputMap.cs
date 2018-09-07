using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class DataNameToInputMap
    {
        private Dictionary<string, Variable> _map;
        private Function[] _funcs;

        public DataNameToInputMap(Function[] funcs, Hashtable map = null)
        {
            _map = new Dictionary<string, Variable>();

            _funcs = funcs.Where(x => x != null).ToArray();

            if (map != null)
                InitializeByUserGivenMap(map);
        }

        private Variable FindVariable(string name)
        {
            foreach (var f in _funcs)
            {
                var va = FunctionFind.FindVariable(f, name);
                if (va != null)
                    return va;
            }

            return null;
        }

        private void InitializeByUserGivenMap(Hashtable map)
        {
            foreach (DictionaryEntry entry in map)
            {
                object value = entry.Value;
                if (value is PSObject)
                    value = (value as PSObject).BaseObject;

                if (value is Variable)
                    _map.Add(entry.Key.ToString(), entry.Value as Variable);
                else
                {
                    var va = FindVariable(value.ToString());
                    if (va == null)
                        throw new ArgumentException(string.Format("Pair ({0}, {1}) in parameterMap doesn't match any variable in the model", entry.Key, value.ToString()));
                    _map.Add(entry.Key.ToString(), va);
                }
            }
        }

        public void InitializeByMinibatch(Minibatch batch)
        {
            if (_map.Count > 0)
                return;

            foreach (var entry in batch.Features)
            {
                var name = entry.Key;
                var va = FindVariable(name);
                if (va != null)
                    _map.Add(name, va);
            }
        }

        public Dictionary<Variable, MinibatchData> GetVariableMinibatchDataMap(Minibatch batch)
        {
            var arguments = new Dictionary<Variable, MinibatchData>();
            foreach (var entry in batch.Features)
            {
                Variable v = null;
                if (_map.TryGetValue(entry.Key, out v))
                    arguments.Add(v, entry.Value);
            }

            if (arguments.Count == 0)
                throw new ApplicationException("Minibatch is empty or contains no data corresponding to arguments");

            return arguments;
        }

        public Dictionary<Variable, Value> GetVariableValueMap(Minibatch batch)
        {
            var inputs = new Dictionary<Variable, Value>();
            foreach (var entry in batch.Features)
            {
                Variable v = null;
                if (_map.TryGetValue(entry.Key, out v))
                    inputs.Add(v, entry.Value.data);
            }

            if (inputs.Count == 0)
                throw new ApplicationException("Minibatch is empty or contains no data corresponding to the input variables of the model");

            return inputs;
        }
    }
}
