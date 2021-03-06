﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using CNTK;

namespace Horker.PSCNTK
{
    public class FunctionFind : INodeWalker
    {
        private string _name;
        private List<Function> _functions;
        private List<Variable> _variables;
        private bool _all;
        private bool _variablesOnly;

        public Variable[] Results
        {
            get {
                var results = new Variable[_functions.Count + _variables.Count];

                for (var i = 0; i < _functions.Count; ++i)
                    results[i] = _functions[i];

                for (var i = 0; i < _variables.Count; ++i)
                    results[i + _functions.Count] = _variables[i];

                return results;
            }
        }

        public FunctionFind(Function func, string name, bool all, bool variablesOnly)
        {
            _name = name;
            _all = all;
            _variablesOnly = variablesOnly;

            _functions = new List<Function>();
            _variables = new List<Variable>();

            new NodeWalk(func, this);
        }

        public bool ProcessFunction(Function func, int depth)
        {
            if (_variablesOnly)
                return true;

            if (func.Name == _name || func.Uid == _name)
            {
                _functions.Add(func);
                if (!_all)
                    return false;
            }

            return true;
        }

        public bool ProcessVariable(Function holder, Variable va, int depth, bool visited)
        {
            if (va.Name == _name || va.Uid == _name)
            {
                _variables.Add(va);
                if (!_all)
                    return false;
            }

            return true;
        }

        public void Complete()
        {
            // Do nothing
        }

        public static Variable Find(Function func, string name)
        {
            var w = new FunctionFind(func, name, false, false);

            var results = w.Results;
            if (results.Length == 0)
                return null;

            return results[0];
        }

        public static Variable FindVariable(Function func, string name)
        {
            var w = new FunctionFind(func, name, false, true);

            if (w._variables.Count == 0)
                return null;

            return w._variables[0];
        }

        public static IList<Variable> FindVariables(Function func, string name)
        {
            var w = new FunctionFind(func, name, true, true);
            return w._variables;
        }
    }
}