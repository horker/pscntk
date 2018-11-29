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
        private Dictionary<string, List<Variable>> _map;
        private Function[] _funcs;

        public DataNameToInputMap(Function[] funcs, Hashtable map = null)
        {
            _map = new Dictionary<string, List<Variable>>();

            _funcs = funcs.Where(x => x != null).ToArray();

            if (map != null)
                InitializeByUserGivenMap(map);
        }

        private List<Variable> FindVariables(string name)
        {
            var variables = new List<Variable>();
            foreach (var f in _funcs)
            {
                var v = FunctionFind.FindVariables(f, name);
                variables.AddRange(v);
            }

            return variables;
        }

        private void AddToMap(string key, Variable va)
        {
            List<Variable> variables;
            _map.TryGetValue(key, out variables);
            if (variables == null)
                _map.Add(key, new List<Variable>() { va });
            else
            {
                if (!variables.Contains(va))
                    variables.Add(va);
            }
        }

        private void InitializeByUserGivenMap(Hashtable map)
        {
            foreach (DictionaryEntry entry in map)
            {
                string key = entry.Key.ToString();
                object value = entry.Value;
                if (value is PSObject)
                    value = (value as PSObject).BaseObject;

                if (value is Variable v)
                    AddToMap(key, v);
                else if (value is WrappedVariable wv)
                    AddToMap(key, wv);
                else
                {
                    var variables = FindVariables(value.ToString());
                    if (variables.Count == 0)
                        throw new ArgumentException(string.Format("Pair ({0}, {1}) in parameterMap doesn't match any variable in the model", entry.Key, value.ToString()));

                    foreach (var va in variables)
                        AddToMap(key, va);
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
                var variables = FindVariables(name);
                if (variables != null)
                {
                    foreach (var va in variables)
                        AddToMap(name, va);
                }
            }
        }

        public Dictionary<Variable, Value> GetVariableValueMap(Minibatch batch)
        {
            var arguments = new Dictionary<Variable, Value>();
            foreach (var entry in batch.Features)
            {
                List<Variable> variables = null;
                if (_map.TryGetValue(entry.Key, out variables))
                {
                    foreach (var va in variables)
                        arguments.Add(va, entry.Value);
                }
            }

            if (arguments.Count == 0)
                throw new ApplicationException("Minibatch is empty or contains no data corresponding to the input variables of the model");

            return arguments;
        }

        public UnorderedMapVariableValuePtr GetVariableValueMapAsCNTKUnorderedMap(Minibatch batch)
        {
            var arguments = GetVariableValueMap(batch);
            var map = new UnorderedMapVariableValuePtr();
            foreach (var entry in arguments)
                map.Add(entry.Key, entry.Value);

            return map;
        }
    }
}
