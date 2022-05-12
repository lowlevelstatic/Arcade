namespace Common.Graph
{
    public readonly struct GraphCoordinates
    {
        public readonly int x;
        public readonly int y;

        public GraphCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString() => $"[x={x}, y={y}]";
    }
}