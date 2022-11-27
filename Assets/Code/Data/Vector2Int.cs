namespace Code.Data
{
    public struct Vector2Int
    {
        public readonly int X;
        public readonly int Y;

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static Vector2Int Zero => new Vector2Int(0, 0);
    }
}