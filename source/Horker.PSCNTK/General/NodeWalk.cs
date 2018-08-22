using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using CNTK;

namespace Horker.PSCNTK
{
    public interface INodeWalker
    {
        bool ProcessFunction(Function function, int depth);
        bool ProcessVariable(Variable variable, int depth, bool visited);
    }

    public class NodeWalk
    {
        private Function _model;
        private INodeWalker _walker;
        private HashSet<Variable> _visited;

        public NodeWalk(CNTK.Function model, INodeWalker walker)
        {
            _model = model;
            _walker = walker;
            _visited = new HashSet<Variable>();

            WalkToFunction(_model, 0);
        }

        private bool WalkToFunction(Function func, int depth)
        {
            if (!_walker.ProcessFunction(func, depth))
                return false;

            if (func.IsComposite)
            {
                if (!WalkToFunction(func.RootFunction, depth + 1))
                    return false;
            }
            else
            {
                foreach (var arg in func.Inputs)
                {
                    if (!WalkToVariable(arg, depth + 1))
                        return false;
                }
            }

            return true;
        }

        private bool WalkToVariable(Variable va, int depth)
        {
            var visited = _visited.Contains(va);
            if (!visited)
                _visited.Add(va);

            if (!_walker.ProcessVariable(va, depth, visited))
                return false;

            if (visited)
                return true;

            if (va.Owner != null)
            {
                if (!WalkToFunction(va.Owner, depth + 1))
                    return false;
            }

            return true;
        }
    }
}