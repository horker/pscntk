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
        private List<NodeGroup.Node> _nodes;
        private List<Link> _links;
        private List<NodeGroup.Node> _subgraphs;

        public string Result => _output.ToString();

        public DotGenerator(Function func)
        {
            _output = new StringBuilder();

            _model = func;
            _nodes = new List<NodeGroup.Node>();
            _links = new List<Link>();
            _subgraphs = new List<NodeGroup.Node>();
            _subgraphs.Add(null);

            _output.AppendLine("digraph dot {");
            _output.AppendLine("    graph [charset=\"utf-8\"; rankdir=\"BT\"; splines=\"spline\"; fontname=\"Consolas\"];");
            _output.AppendLine("    node [fontname=\"Consolas\"];");
            _output.AppendLine("    edge [dir=\"back\"; arrowtail=\"normal\"];");

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
                _subgraphs.Add(new NodeGroup.Node(func));

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
            if (_nodes.Any(x => x.Uid == va.Uid))
                return;
            _nodes.Add(new NodeGroup.Node(va));
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
            WriteOutputNode(1);

            var groups = _nodes.GroupBy(x => NodeGroup.FindGroup(x.Uid));

            var visited = new HashSet<NodeGroup>();

            foreach (var group in groups)
            {
                var g = group.Key;
                if (g == null)
                {
                    WriteNodes(group, 1);
                    continue;
                }

                while (g.Parent != null)
                    g = g.Parent;

                WriteNodeInGroup(g, groups, visited, 1);
            }

            foreach (var link in _links)
                _output.AppendFormat("    {0} -> {1};\r\n", link.From, link.To);
        }

        private void WriteOutputNode(int depth)
        {
            var name =
                "Output" +
                "|" +
                string.Join(" x ", _model.Output.Shape.Dimensions);
            var style = "style=\"rounded, filled\" fillcolor=\"#ccccff\"";

            _output.AppendFormat("    {0} [label=\"{1}\" shape=\"record\" {2}];\r\n", _model.Uid, name, style);
        }

        private void WriteNodeInGroup(NodeGroup group, IEnumerable<IGrouping<NodeGroup, NodeGroup.Node>> grouping, HashSet<NodeGroup> visited, int depth)
        {
            if (visited.Contains(group))
                return;

            visited.Add(group);

            var indent = new string(' ', depth * 4);
            _output.AppendFormat("{0}subgraph cluster_{1} {{\r\n", indent, group.UniqueName);
            _output.AppendFormat("{0}    label = \"{1}\";\r\n", indent, group.Name);
            _output.AppendFormat("{0}    labelloc = \"t\";\r\n", indent);
            _output.AppendFormat("{0}    labeljust = \"r\";\r\n", indent);
            _output.AppendFormat("{0}    style = \"dotted, filled\";\r\n", indent);
            _output.AppendFormat("{0}    fillcolor = \"#f0f0f0\";\r\n", indent);

            var nodes = grouping.Where(x => x.Key == group).FirstOrDefault();
            WriteNodes(nodes, depth + 1);

            foreach (var g in group.Subgroups)
                WriteNodeInGroup(g, grouping, visited, depth + 1);

            _output.AppendFormat("{0}}}\r\n", indent);
        }

        private void WriteNodes(IEnumerable<NodeGroup.Node> nodes, int depth)
        {
            if (nodes == null)
                return;

            var indent = new string(' ', depth * 4);

            foreach (var node in nodes)
            {
                var value = FunctionFind.Find(_model, node.Uid);
                if (value == null)
                    throw new ApplicationException(string.Format("Can't find network node with uid {0}", node.Uid));

                if (value.IsOutput)
                {
                    // Function

                    var func = value.Owner;

                    var name = func.OpName + "|" + (func.Output == null ? "(undef)" : string.Join(" x ", func.Output.Shape.Dimensions));
                    var style = "style=\"filled\" fillcolor=\"white\"";

                    _output.AppendFormat("{0}{1} [label=\"{2}\" shape=\"record\" {3}];\r\n", indent, func.Uid, name, style);
                }
                else
                {
                    // Variable

                    var va = value as CNTK.Variable;

                    var style = "style=\"rounded\"";
                    if (va.IsInput)
                        style = "style=\"rounded, filled\" fillcolor=\"#ffffcc\"";

                    var name = (string.IsNullOrEmpty(va.Name) ? va.Uid : va.Name) + "|" + string.Join(" x ", va.Shape.Dimensions);
                    _output.AppendFormat("{0}{1} [label=\"{2}\" shape=\"record\" {3}];\r\n", indent, va.Uid, name, style);
                }
            }
        }
    }
}
