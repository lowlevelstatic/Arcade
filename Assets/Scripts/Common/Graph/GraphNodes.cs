using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Graph
{
    public readonly struct GraphNodes
    {
        private readonly (GraphCoordinates coordinates, NodeID node)[] m_nodes;
        
        public static GraphNodes Empty => new(Array.Empty<(GraphCoordinates coordinates, NodeID node)>());
        
        public GraphNodes(IEnumerable<(GraphCoordinates coordinates, NodeID node)> attributes) =>
            m_nodes = attributes.Select(entry => (entry.coordinates, entry.node)).ToArray();

        public MutableGraphNodes CloneMutable() => new(m_nodes);

        public IEnumerable<NodeID> FindNodes(Func<NodeID, bool> predicate) => m_nodes
            .Where(entry => predicate(entry.node))
            .Select(entry => entry.node);
        
        public IEnumerable<NodeID> FindNodes(Func<GraphCoordinates, bool> predicate) => m_nodes
            .Where(entry => predicate(entry.coordinates))
            .Select(entry => entry.node);
        
        public GraphCoordinates GetNodeCoordinates(NodeID node) =>
            TryGetNodeCoordinates(node, out var coordinates) ? coordinates : default;
        
        public bool TryGetNodeCoordinates(NodeID node, out GraphCoordinates coordinates)
        {
            var matches = m_nodes
                .Where(entry => entry.node == node)
                .Select(entry => entry.coordinates)
                .Take(1)
                .ToList();

            if (!matches.Any())
            {
                coordinates = default;
                return false;
            }
            
            coordinates = matches.First();
            return true;
        }

        public NodeID GetNodeAtCoordinates(GraphCoordinates coordinates) =>
            TryGetNodeAtCoordinates(coordinates, out var node) ? node : default;
        
        public bool TryGetNodeAtCoordinates(GraphCoordinates coordinates, out NodeID node)
        {
            var matches = m_nodes
                .Where(entry => entry.coordinates.Equals(coordinates))
                .Select(entry => entry.node);

            foreach (var match in matches)
            {
                node = match;
                return true;
            }

            node = default;
            return false;
        }
    }
}