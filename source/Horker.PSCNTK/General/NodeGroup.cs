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
        private string _name;
        private string _uniqueName;

        private NodeGroup _parent;
        private List<NodeGroup> _subgroups;

        private List<WeakReference> _nodes;

        public string Name { get => _name; set { _name = value; } }
        public string UniqueName { get => _uniqueName; set { _uniqueName = value; } }

        public NodeGroup Parent => _parent;
        public IEnumerable<NodeGroup> Subgroups => _subgroups;

        public IEnumerable<Variable> Nodes => GetLiveNodes();

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

            _nodes = new List<WeakReference>();
        }

        private IEnumerable<Variable> GetLiveNodes()
        {
            int totalCount = 0;
            int liveCount = 0;
            foreach (var n in _nodes)
            {
                ++totalCount;
                object v = n.Target;
                if (n != null)
                {
                    ++liveCount;
                    yield return (Variable)v;
                }
            }

            if (totalCount * .5 > liveCount)
            {
                _nodes.RemoveAll(x => !x.IsAlive);
                if (_nodes.Count == 0 && _subgroups.Count == 0)
                    RemoveGroup(this);
            }
        }

        public bool Contains(Variable v)
        {
            return _nodes.Any(x => x.Target == v);
        }

        public void Add(Variable v)
        {
            _nodes.Add(new WeakReference(v));
        }

        #region Group holder

        private static List<NodeGroup> _groups = new List<NodeGroup>();

        public static NodeGroup Current = null;
        public static IEnumerable<NodeGroup> Groups => _groups;

        public static NodeGroup EnterNewGroup(string name)
        {
            var g = new NodeGroup(name, Current);
            _groups.Add(g);
            Current = g;

            return g;
        }

        public static void LeaveGroup()
        {
            Current = Current._parent;
        }

        public static void RemoveGroup(NodeGroup group)
        {
            if (group._subgroups.Count > 0)
                throw new ArgumentException("Can't remove a group that contains subgroups");

            if (Current == group)
                throw new ArgumentException("Can't remove the current group");

            group.Parent._subgroups.Remove(group);
            _groups.Remove(group);
        }

        public static NodeGroup FindGroup(Variable v)
        {
            foreach (var g in _groups)
                foreach (var n in g._nodes)
                    if (n.Target == v)
                        return g;

            return null;
        }

        #endregion
    }
}
