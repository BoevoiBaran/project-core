using System;
using System.Collections.Generic;

namespace AStar
{
    public class PFHelper<TNode>
    {
        private static PFHelper<TNode> Instance => _instance ??= new PFHelper<TNode>();
        private static PFHelper<TNode> _instance;
        internal static SortedList<TNode> SortedList => Instance._sortedList;
        private readonly SortedList<TNode> _sortedList;

        private PFHelper()
        {
            _instance = this;
            _sortedList = new SortedList<TNode>();
        }

        public static List<TNode> CalcPath(
            Dictionary<TNode, double> nodes,
            TNode start, TNode destination,
            Func<TNode, IEnumerable<TNode>> getNeighbours)
        {
            var node = destination;
            var path = new List<TNode>();
            path.Add(node);
            while (!node.Equals(start))
            {
                node = FindNodeWithMinWeight(nodes, node, getNeighbours);
                path.Add(node);
                nodes.Remove(node);
            }

            path.Reverse();
            return path;
        }

        public static TNode FindNodeWithMinWeight<TNode>(
            Dictionary<TNode, double> nodes,
            TNode node,
            Func<TNode, IEnumerable<TNode>> getNeighbours
        )
        {
            TNode minNode = default;
            double minDist = double.MaxValue;
            foreach (var n in getNeighbours(node))
            {
                if (!nodes.ContainsKey(n))
                    continue;
                if (nodes[n] < minDist)
                {
                    minDist = nodes[n];
                    minNode = n;
                }
            }

            return minNode;
        }
    }
}