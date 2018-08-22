using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class DotGenerator : INodeWalker
    {
        public class Node
        {
            public Function Subgraph;
            public Function Function;
            public Variable Variable;

            public Node(Function subgraph, Function func)
            {
                Subgraph = subgraph;
                Function = func;
            }

            public Node(Function subgraph, Variable va)
            {
                Subgraph = subgraph;
                Variable = va;
            }
        }

        public class Link
        {
            public string From;
            public string To;

            public Link(string from, string to)
            {
                From = from;
                To = to;
            }
        }

        private StringBuilder _output;

        private List<Node> _nodes;
        private List<Link> _links;
        private List<Function> _subgraphs;

        public string Result => _output.ToString();

        public DotGenerator(Function func)
        {
            _output = new StringBuilder();

            _nodes = new List<Node>();
            _links = new List<Link>();
            _subgraphs = new List<Function>();
            _subgraphs.Add(null);

            _output.AppendLine("digraph dot {");
            _output.AppendLine("graph [splines=\"polyline\"];");
            _output.AppendLine("edge [dir=\"back\"; arrowtail=\"normal\"];");

            new NodeWalk(func, this);

            DefineNodesAndLinks();

            _output.AppendLine("}");
        }

        public bool ProcessFunction(Function func, int depth)
        {
            if (func.IsComposite)
                AddLink(func, func.RootFunction);
            else
            {
                if (!string.IsNullOrEmpty(func.Name))
                    _subgraphs.Add(func);

                foreach (var va in func.Inputs)
                    AddLink(func, va);
            }

            return true;
        }

        public bool ProcessVariable(Variable va, int depth, bool visited)
        {
            return true;
        }

        private void AddNode(Function func)
        {
            if (!string.IsNullOrEmpty(func.Name))
                _nodes.Add(new Node(func, func));
            else
                _nodes.Add(new Node(_subgraphs.Last(), func));
        }

        private void AddNode(Variable va)
        {
            if (va.IsInput)
                _nodes.Add(new Node(null, va));
            else
                _nodes.Add(new Node(_subgraphs.Last(), va));
        }

        private void AddLink(Function from, Function to)
        {
            AddNode(from);
            AddNode(to);
            _links.Add(new Link(from.Uid, to.Uid));
        }

        private void AddLink(Function from, Variable to)
        {
            AddNode(from);

            if (to.IsOutput)
            {
                var f = to.Owner;
                AddNode(f);
                _links.Add(new Link(from.Uid, f.Uid));

            }
            else
            {
                AddNode(to);
                _links.Add(new Link(from.Uid, to.Uid));
            }
        }

        private void AddLink(Variable from, Function to)
        {
            AddNode(from);
            AddNode(to);
            _links.Add(new Link(from.Uid, to.Uid));
        }

        private void DefineNodesAndLinks()
        {
            var subgraphs = _nodes.Select(x => x.Subgraph).Distinct();

            foreach (var sg in subgraphs)
            {
//                if (sg != null)
//                {
//                    _output.AppendFormat("subgraph cluster_{0} {{\r\n", sg.Uid);
//                    _output.AppendFormat("label = \"{0}\";\r\n", sg.Name);
//                    _output.AppendLine("labelloc = \"t\";");
//                    _output.AppendLine("labeljust = \"r\";");
//                    _output.AppendLine("style = \"dotted, filled\";");
//                    _output.AppendLine("fillcolor = \"#f0f0f0\";");
//                }

                foreach (var node in _nodes.Where(x => x.Subgraph == sg))
                {
                    if (node.Function != null)
                    {
                        var func = node.Function;

                        string name;
                        string style;
                        if (func.IsComposite)
                        {
                            name = (func.Output == null || string.IsNullOrEmpty(func.Output.Name) ? "Output" : func.Output.Name) +
                                "|" +
                                (func.Output == null ? "(undef)" : string.Join(" x ", func.Output.Shape.Dimensions));
                            style = "style=\"rounded, filled\" fillcolor=\"#ccccff\"";
                        }
                        else
                        {
                            name = func.OpName + "|" + (func.Output == null ? "(undef)" : string.Join(" x ", func.Output.Shape.Dimensions));
                            style = "style=\"filled\" fillcolor=\"white\"";
                        }

                        _output.AppendFormat("{0} [label=\"{1}\" shape=\"record\" {2}];\r\n", func.Uid, name, style);
                    }
                    else
                    {
                        var va = node.Variable;
                        var style = "style=\"rounded\"";
                        if (va.IsInput)
                            style = "style=\"rounded, filled\" fillcolor=\"#ffffcc\"";

                        var name = (string.IsNullOrEmpty(va.Name) ? va.Uid : va.Name) + "|" + string.Join(" x ", va.Shape.Dimensions);
                        _output.AppendFormat("{0} [label=\"{1}\" shape=\"record\" {2}];\r\n", va.Uid, name, style);
                    }
                }

//                if (sg != null)
//                    _output.AppendLine("}");
            }

            foreach (var link in _links)
                _output.AppendFormat("{0} -> {1};\r\n", link.From, link.To);
        }
    }
}
