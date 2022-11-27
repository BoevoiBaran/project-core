namespace AStar
{
    internal struct SortedNode<TNode>
    {
        public TNode Node;
        public double Weight;
        public double Heuristic;

        public void UpdateValues(double newWeight, double newHeuristic)
        {
            Weight = newWeight;
            Heuristic = newHeuristic;
        }

        public void ReUsed(TNode node, double newWeight, double newHeuristic)
        {
            Node = node;
            Weight = newWeight;
            Heuristic = newHeuristic;
        }
    }
}