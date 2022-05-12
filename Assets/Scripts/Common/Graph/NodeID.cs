namespace Common.Graph
{
    public readonly struct NodeID
    {
        private readonly int m_value;

        public NodeID(int value)
        {
            m_value = value;
        }

        public bool Equals(NodeID other) => m_value.Equals(other.m_value);

        public override bool Equals(object obj) => obj is NodeID other && Equals(other);

        public override int GetHashCode() => m_value;

        public static bool operator ==(NodeID left, NodeID right) => left.Equals(right);

        public static bool operator !=(NodeID left, NodeID right) => !left.Equals(right);
    }
}