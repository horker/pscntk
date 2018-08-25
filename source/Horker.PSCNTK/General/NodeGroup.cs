using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class NodeGroup
    {
        public class Node
        {
            WeakReference<Variable> _variable;
            WeakReference<Function> _function;

            private string _uid;

            public string Uid => _uid;
            public bool IsFunction => _function != null;

            public Node(Variable v)
            {
                if (v.IsOutput)
                {
                    var f = v.Owner;
                    _variable = null;

                    // Setting trackResurrection to true is necessary because without this setting WeakRef
                    // prematurely losts the target after garbage collection regardless of the target liveness.
                    // By this setting, we can keep track of the object reference, however, its reference is not
                    // valid so that accessing to it causes AccessViolationException (I don't know why).
                    // Thus, the Node class keeps WeakRef but does not use the object reference obtained by it.
                    // Instead, it keeps the node's uid and obtain the corresponding object from the network each time
                    // when the object reference is needed. WeakRef is used only to track object liveness.
                    _function = new WeakReference<Function>(f, true);

                    _uid = f.Uid;
                }
                else
                {
                    _variable = new WeakReference<Variable>(v, true);
                    _function = null;

                    _uid = v.Uid;
                }
            }

            public object Get()
            {
                if (_variable != null)
                {
                    Variable v = null;
                    if (_variable.TryGetTarget(out v))
                        return v;
                }
                else
                {
                    Function f = null;
                    if (_function.TryGetTarget(out f))
                        return f;
                }

                return null;
            }

            public bool IsAlive()
            {
                return Get() != null;
            }
        }

        private string _name;
        private string _uniqueName;

        private NodeGroup _parent;
        private List<NodeGroup> _subgroups;

        private List<Node> _nodes;

        public string Name { get => _name; set { _name = value; } }
        public string UniqueName { get => _uniqueName; set { _uniqueName = value; } }

        public NodeGroup Parent => _parent;
        public IEnumerable<NodeGroup> Subgroups => _subgroups;

        public IEnumerable<Node> Nodes => GetLiveNodes();

        private static int _uniqueIndex = 0;

        public NodeGroup(string name, NodeGroup parent)
        {
            _name = name;
            _uniqueName = _uniqueIndex.ToString();
            ++_uniqueIndex;

            _parent = parent;
            _subgroups = new List<NodeGroup>();
            if (parent != null)
                parent._subgroups.Add(this);

            _nodes = new List<Node>();
        }

        private IEnumerable<Node> GetLiveNodes()
        {
            return _nodes.Where(x => x.IsAlive());
        }

        public bool Contains(Variable va)
        {
            return _nodes.Any(x => x.Get() == va);
        }

        public void Add(Variable va)
        {
            _nodes.Add(new Node(va));
        }

        public bool Contains(string uid)
        {
            return _nodes.Any(x => x.Uid == uid);
        }

        public override string ToString()
        {
            var parentName = "(null)";
            if (_parent != null)
                parentName = _parent._uniqueName;

            var subgroups = string.Join(" ", _subgroups.Select(x => x._uniqueName));

            return string.Format("NodeGroup({0}, {1}, parent={2}, subgroups={3}, {4} nodes)", _uniqueName, _name, parentName, subgroups, _nodes.Count);
        }

        #region Group management

        private static List<NodeGroup> _groups = new List<NodeGroup>();

        public static NodeGroup Current = null;
        public static IEnumerable<NodeGroup> Groups => _groups;

        public static NodeGroup EnterNewGroup(string name)
        {
            RemoveDeadNodes();

            var g = new NodeGroup(name, Current);
            _groups.Add(g);
            Current = g;

            return g;
        }

        public static void LeaveGroup()
        {
            Current = Current._parent;
        }

        private static void RemoveDeadNodes()
        {
            foreach (var g in _groups)
                g._nodes.RemoveAll(x => x.Get() == null);

            bool stable;
            do
            {
                stable = true;
                foreach (var g in _groups)
                {
                    if (g != Current && g._nodes.Count == 0 && g._subgroups.Count == 0 && g._parent != null)
                    {
                        g._parent._subgroups.Remove(g);
                        g._parent = null;
                        stable = false;
                    }
                }
            }
            while (!stable);

            _groups.RemoveAll(g => g != Current && g._nodes.Count == 0 && g._subgroups.Count == 0 && g._parent == null);
        }

        public static NodeGroup FindGroup(string uid)
        {
            foreach (var g in _groups)
            {
                foreach (var n in g._nodes)
                    if (n.Uid == uid)
                        return g;
            }

            return null;
        }

        #endregion
    }
}
