using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using CNTK;
using System;
using System.Collections;

namespace Horker.PSCNTK
{
    public class NodeInfo
    {
        public IWrappedNode Node;
        public string Uid;
        public string Name;
        public string Type;
        public string[] Path;
        public string Parent;
        public string[] Children;
        public Shape Shape;
        public IDataSource<float>[] Values;
    }

    class FunctionGetNodeInfo : INodeWalker
    {
        private Hashtable _arguments;
        private Minibatch _minibatch;
        private DataNameToInputMap _map;

        private BlockingCollection<NodeInfo> _queue;
        private Dictionary<string, NodeInfo> _history;
        private NodeInfo _poison;
        private Exception _exception;

        public FunctionGetNodeInfo(Function func, Hashtable arguments = null, Minibatch minibatch = null, DataNameToInputMap map = null)
        {
            _arguments = arguments;
            _minibatch = minibatch;
            _map = map;

            _queue = new BlockingCollection<NodeInfo>();
            _history = new Dictionary<string, NodeInfo>();
            _poison = new NodeInfo();
            _exception = null;

            Task.Run(() => {
                try
                {
                    new NodeWalk(func, this);
                }
                catch (Exception e)
                {
                    _exception = e;
                    _queue.Add(_poison);
                }
            });
        }

        public IEnumerable<NodeInfo> GetNodeInfo()
        {
            var node = _queue.Take();
            while (node != _poison)
            {
                yield return node;
                node = _queue.Take();
            }

            if (_exception != null)
                throw _exception;
        }

        public bool ProcessFunction(Function func, int depth)
        {
            IDataSource<float>[] values = null;
            NodeInfo outputNode = null;
            if (_history.TryGetValue(func.Output.Uid, out outputNode))
                values = outputNode.Values;
            else
                values = GetValues(func);

            var nodeInfo = new NodeInfo()
            {
                Node = (WrappedFunction)func,
                Uid = func.Uid,
                Name = func.Name,
                Type = func.IsComposite ? "CompositeFunction" : "Function",
                Path = GetPath(func.Uid, func.Output.Uid),
                Parent = func.Output.Uid,
                Children = func.Inputs.Select(x => x.Uid).ToArray(),
                Shape = func.Output.Shape,
                Values = values
            };

            _history.Add(func.Uid, nodeInfo);
            _queue.Add(nodeInfo);

            return true;
        }

        public bool ProcessVariable(Function holder, Variable va, int depth, bool visited)
        {
            if (visited)
                return true;

            IDataSource<float>[] values = null;

            if (va.Kind == VariableKind.Parameter || va.Kind == VariableKind.Constant)
                values = new IDataSource<float>[] { DataSourceFactory.FromVariable(va) };
            else if (va.Kind == VariableKind.Output)
                values = GetValues(va.Owner);

            var nodeInfo = new NodeInfo()
            {
                Node = (WrappedVariable)va,
                Uid = va.Uid,
                Name = va.Name,
                Type = Utils.VariableKindName(va.Kind),
                Path = GetPath(va.Uid, holder.Uid),
                Parent = holder.Uid,
                Children = va.Owner != null ? new string[] { va.Owner.Uid } : null,
                Shape = va.Shape,
                Values = values
            };

            _history.Add(va.Uid, nodeInfo);
            _queue.Add(nodeInfo);

            return true;
        }

        public void Complete()
        {
            _queue.Add(_poison);
        }

        private IDataSource<float>[] GetValues(Function func)
        {
            Value[] values = null;
            try
            {
                if (_minibatch == null)
                    values = FunctionInvoke.Invoke(func, _arguments, null, false);
                else
                    values = FunctionInvoke.Invoke(func, _minibatch, _map, null, false);
            }
            catch (Exception)
            {
                // Pass
            }

            return values.Select(x => DataSourceFactory.FromValue(x)).ToArray();
        }

        private string[] GetPath(string uid, string parentUid)
        {
            NodeInfo node = null;
            if (!_history.TryGetValue(parentUid, out node))
                return new string[] { uid };

            var result = new List<string>();

            do
            {
                result.Add(node.Uid);
            }
            while (_history.TryGetValue(node.Parent, out node));

            result.Reverse();
            result.Add(uid);

            return result.ToArray();
        }
    }
}
