using System.Collections.Generic;

namespace AStar
{
    internal class SortedList<TNode>
    {
        public bool IsEmpty => _nodes.Count == 0;

        private readonly Stack<SortedNode<TNode>> _cache = new Stack<SortedNode<TNode>>();
        private readonly LinkedList<SortedNode<TNode>> _linkedList = new LinkedList<SortedNode<TNode>>();

        private readonly Dictionary<TNode, LinkedListNode<SortedNode<TNode>>> _nodes =
            new Dictionary<TNode, LinkedListNode<SortedNode<TNode>>>();

        private readonly Dictionary<TNode, double> _openSetNodes = new Dictionary<TNode, double>();
        
        private readonly Dictionary<TNode, double> _closedSetNodes = new Dictionary<TNode, double>();

        public void AddToCloseSet(SortedNode<TNode> sortedNode)
        {
            if (_closedSetNodes.ContainsKey(sortedNode.Node))
            {
                _closedSetNodes[sortedNode.Node] = sortedNode.Weight;
            }
            else
            {
                _closedSetNodes.Add(sortedNode.Node, sortedNode.Weight);
            }
        }

        public void AddToOpenSet(TNode node, double weight, double heuristic)
        {
            if (_openSetNodes.ContainsKey(node))
            {
                if (_openSetNodes[node] < weight)
                    return;
            }

            if (_nodes.ContainsKey(node))
            {
                UpdateNode(node, weight, heuristic);
                return;
            }

            SortedNode<TNode> newNode;
            if (_cache.Count == 0)
            {
                newNode = new SortedNode<TNode>()
                {
                    Heuristic = heuristic,
                    Node = node,
                    Weight = weight,
                };
            }
            else
            {
                newNode = _cache.Pop();
                newNode.ReUsed(node, weight, heuristic);
            }


            var n = _linkedList.First;
            while (n != null)
            {
                var sortedNode = n.Value;
                if (sortedNode.Heuristic > heuristic)
                {
                    AddNode(newNode, n, false);
                    return;
                }

                n = n.Next;
            }

            AddNode(newNode, null, true);
        }

        public SortedNode<TNode> Get()
        {
            var node = _linkedList.First;
            if (node == null)
            {
                return default;
            }

            var value = node.Value;
            _linkedList.RemoveFirst();
            _nodes.Remove(value.Node);
            _cache.Push(node.Value);
            return value;
        }

        public Dictionary<TNode, double> GetFinalMapCopy()
        {
            return _closedSetNodes;
        }

        private void AddNode(SortedNode<TNode> sortedNode, LinkedListNode<SortedNode<TNode>> where, bool isAddLast)
        {
            LinkedListNode<SortedNode<TNode>> listNode;
            if (isAddLast)
            {
                listNode = _linkedList.AddLast(sortedNode);
            }
            else
            {
                listNode = _linkedList.AddBefore(where, sortedNode);
            }

            _nodes.Add(sortedNode.Node, listNode);

            if (_openSetNodes.ContainsKey(sortedNode.Node))
            {
                _openSetNodes[sortedNode.Node] = sortedNode.Weight;
            }
            else
            {
                _openSetNodes.Add(sortedNode.Node, sortedNode.Weight);
            }

        }

        private void UpdateNode(TNode node, double weight, double heuristic)
        {
            var sortedNode = _nodes[node];
            sortedNode.Value.UpdateValues(weight, heuristic);
            _openSetNodes[node] = weight;
        }

        public void Reset()
        {
            var n = _linkedList.First;
            while (n != null)
            {
                var sortedNode = n.Value;
                _cache.Push(sortedNode);
                n = n.Next;
            }

            _openSetNodes.Clear();
            _nodes.Clear();
            _linkedList.Clear();
            _closedSetNodes.Clear();
        }
    }
}