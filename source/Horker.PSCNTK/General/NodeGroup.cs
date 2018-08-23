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
        private List<WeakReference> _nodes;

        public string Name { get => _name; set { _name = value; } }
        public string UniqueName { get => _uniqueName; set { _uniqueName = value; } }

        public IEnumerable<Variable> Nodes => GetLiveNodes();

        static int _uniqueIndex = 0;

        public NodeGroup(string name)
        {
            _name = name;
            _uniqueName = _uniqueIndex.ToString();
            ++_uniqueIndex;

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
                if (_nodes.Count == 0)
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
        private static Stack<NodeGroup> _groupStack = new Stack<NodeGroup>();

        public static NodeGroup Current = null;
        public static IEnumerable<NodeGroup> Groups => _groups;

        public static NodeGroup EnterNewGroup(string name)
        {
            var g = new NodeGroup(name);
            _groups.Add(g);
            _groupStack.Push(g);
            Current = g;

            return g;
        }

        public static void LeaveGroup()
        {
            _groupStack.Pop();
            if (_groupStack.Count > 0)
                Current = _groupStack.Peek();
            else
                Current = null;
        }

        public static void RemoveGroup(NodeGroup group)
        {
            if (_groupStack.Contains(group))
                throw new ArgumentException("Can't remove current group");

            _groups.Remove(group);
        }

        public static NodeGroup FindGroup(Variable v)
        {
            foreach (var g in _groups)
                if (g.Nodes.Contains(v))
                    return g;

            return null;
        }

        #endregion
    }
}
