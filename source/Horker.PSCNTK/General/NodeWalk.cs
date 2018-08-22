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
        void ProcessFunction(Function function, int depth);
        void ProcessVariable(Variable variable, int depth, bool visited);
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

        private void WalkToFunction(Function func, int depth)
        {
            _walker.ProcessFunction(func, depth);

            if (func.IsComposite)
                WalkToFunction(func.RootFunction, depth + 1);
            else
                foreach (var arg in func.Inputs)
                    WalkToVariable(arg, depth + 1);

        }

        private void WalkToVariable(Variable va, int depth)
        {
            var visited = _visited.Contains(va);

            _walker.ProcessVariable(va, depth, visited);

            if (visited)
                return;

            _visited.Add(va);

            if (va.Owner != null)
                WalkToFunction(va.Owner, depth + 1);
        }
    }
}