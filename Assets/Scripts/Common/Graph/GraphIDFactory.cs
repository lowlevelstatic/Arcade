namespace Common.Graph
{
    public static class GraphIDFactory
    {
        private static int _nextID;
        
        public static NodeID NodeID() => new(_nextID++);
    }
}