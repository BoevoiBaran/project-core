using System;
using System.Collections.Generic;

namespace AStar
{
    public static class Pathfinder
    {
        private static System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// External Func to find Node's neighbours
        /// </summary>
        public static List<TNode> FindPath<TNode>(TNode start, TNode destination,
            Func<TNode, TNode, double> getMoveCost,
            Func<TNode, TNode, double> getDistance,
            Func<TNode, IEnumerable<TNode>> getNeighbours,
            Func<TNode, TNode, bool> checkPassability)
        {
            if (start == null || destination == null)
            {
                return null;
            }

            sw.Restart();
            var sortedList = PFHelper<TNode>.SortedList;
            sortedList.Reset();
            sortedList.AddToOpenSet(start, 0, 0);

            return PF(start, destination, getMoveCost, getDistance, getNeighbours, checkPassability);
        }

        private static List<TNode> PF<TNode>
        (TNode start, TNode destination,
            Func<TNode, TNode, double> getMoveCost,
            Func<TNode, TNode, double> getEstimate,
            Func<TNode, IEnumerable<TNode>> getNeighbours,
            Func<TNode, TNode, bool> checkPassability)
        {
            sw.Restart();
            var sortedList = PFHelper<TNode>.SortedList;

            while (!sortedList.IsEmpty)
            {
                var sortedNode = sortedList.Get();
                sortedList.AddToCloseSet(sortedNode);
                if (sortedNode.Node.Equals(destination))
                {
                    return PFHelper<TNode>.CalcPath(sortedList.GetFinalMapCopy(), start, destination,
                        getNeighbours);
                }

                foreach (var n in getNeighbours(sortedNode.Node))
                {
                    if (!checkPassability(n, sortedNode.Node))
                    {
                        continue;
                    }

                    var d = getMoveCost(n, sortedNode.Node);

                    var weight = d + sortedNode.Weight;

                    sortedList.AddToOpenSet(n, weight, weight + getEstimate(n, destination));

                    if (weight > 9999999 || sw.ElapsedMilliseconds > 1000000000) // to prevent infinite pathfinding
                    {
                        UnityEngine.Debug.LogError("pathfinding fuse fired");
                        return null;
                    }
                }
            }

            return null;
        }
    }
}