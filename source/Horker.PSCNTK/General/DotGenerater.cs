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

        private Function _model;
        private List<Variable> _nodes;
        private List<Link> _links;
        private List<Function> _subgraphs;

        public string Result => _output.ToString();

        public DotGenerator(Function func)
        {
            _output = new StringBuilder();

            _model = func;
            _nodes = new List<Variable>();
            _links = new List<Link>();
            _subgraphs = new List<Function>();
            _subgraphs.Add(null);

            _output.AppendLine("digraph dot {");
            _output.AppendLine("graph [charset=\"utf-8\"; splines=\"polyline\"; fontname=\"Consolas\"];");
            _output.AppendLine("node [fontname=\"Consolas\"];");
            _output.AppendLine("edge [dir=\"back\"; arrowtail=\"normal\"];");

            _links.Add(new Link(func.Uid, func.RootFunction.Uid));

            new NodeWalk(func, this);

            WriteAllNodes();

            _output.AppendLine("}");
        }

        public bool ProcessFunction(Function func, int depth)
        {
            if (func.IsComposite)
                return true;

            if (!string.IsNullOrEmpty(func.Name))
                _subgraphs.Add(func);

            foreach (var va in func.Inputs)
                AddLink(func, va);

            return true;
        }

        public bool ProcessVariable(Variable va, int depth, bool visited)
        {
            return true;
        }

        private void AddNode(Variable va)
        {
            if (_nodes.Contains(va))
                return;
            _nodes.Add(va);
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

        private void WriteAllNodes()
        {
            WriteOutputNode();

            var groups = _nodes.GroupBy(x => NodeGroup.FindGroup(x));

            foreach (var g in groups)
                WriteNodeInGroup(g.Key, g);

            foreach (var link in _links)
                _output.AppendFormat("{0} -> {1};\r\n", link.From, link.To);
        }

        private void WriteOutputNode()
        {
            var name =
                "Output" +
                "|" +
                string.Join(" x ", _model.Output.Shape.Dimensions);
            var style = "style=\"rounded, filled\" fillcolor=\"#ccccff\"";

            _output.AppendFormat("{0} [label=\"{1}\" shape=\"record\" {2}];\r\n", _model.Uid, name, style);
        }

        private void WriteNodeInGroup(NodeGroup group, IEnumerable<Variable> nodes)
        {
            bool first = true;
            bool hasNodes = false;

            foreach (var node in nodes)
            {
                if (first && group != null)
                {
                    _output.AppendFormat("subgraph cluster_{0} {{\r\n", group.UniqueName);
                    _output.AppendFormat("label = \"{0}\";\r\n", group.Name);
                    _output.AppendLine("labelloc = \"t\";");
                    _output.AppendLine("labeljust = \"r\";");
                    _output.AppendLine("style = \"dotted, filled\";");
                    _output.AppendLine("fillcolor = \"#f0f0f0\";");
                    first = false;
                    hasNodes = true;
                }

                var value = node;
                if (value.IsOutput)
                {
                    // Function

                    var func = value.Owner;

                    var name = func.OpName + "|" + (func.Output == null ? "(undef)" : string.Join(" x ", func.Output.Shape.Dimensions));
                    var style = "style=\"filled\" fillcolor=\"white\"";

                    _output.AppendFormat("{0} [label=\"{1}\" shape=\"record\" {2}];\r\n", func.Uid, name, style);
                }
                else
                {
                    // Variable

                    var va = value as CNTK.Variable;

                    var style = "style=\"rounded\"";
                    if (va.IsInput)
                        style = "style=\"rounded, filled\" fillcolor=\"#ffffcc\"";

                    var name = (string.IsNullOrEmpty(va.Name) ? va.Uid : va.Name) + "|" + string.Join(" x ", va.Shape.Dimensions);
                    _output.AppendFormat("{0} [label=\"{1}\" shape=\"record\" {2}];\r\n", va.Uid, name, style);
                }
            }

            if (hasNodes)
                _output.AppendLine("}");
        }
    }
}
