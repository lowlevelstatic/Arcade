using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Graph
{
    public class MutableGraphNodes
    {
        private readonly Dictionary<GraphCoordinates, NodeID> m_nodes;
        
        public MutableGraphNodes(IEnumerable<(GraphCoordinates coordinates, NodeID node)> nodes) =>
            m_nodes = nodes.ToDictionary(node => node.coordinates, node => node.node);

        public GraphNodes CloneImmutable() => new(m_nodes
            .Select(entry => (entry.Key, entry.Value))
            .ToArray());
        
        public IEnumerable<NodeID> FindNodes(Func<NodeID, bool> predicate) => m_nodes
            .Where(entry => predicate(entry.Value))
            .Select(entry => entry.Value);

        public IEnumerable<NodeID> FindNodes(Func<GraphCoordinates, bool> predicate) => m_nodes
            .Where(entry => predicate(entry.Key))
            .Select(entry => entry.Value);
        
        public GraphCoordinates GetNodeCoordinates(NodeID node) =>
            TryGetNodeCoordinates(node, out var coordinates) ? coordinates : default;
        
        public bool TryGetNodeCoordinates(NodeID node, out GraphCoordinates coordinates)
        {
            var matches = m_nodes
                .Where(entry => entry.Value == node)
                .Select(entry => entry.Key)
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

        public bool TryGetNodeAtCoordinates(GraphCoordinates coordinates, out NodeID node) =>
            m_nodes.TryGetValue(coordinates, out node);
        
        public void AddNode(GraphCoordinates coordinates, NodeID node) => TryAddNode(coordinates, node);

        public bool TryAddNode(GraphCoordinates coordinates, NodeID node)
        {
            if (m_nodes.ContainsKey(coordinates))
            {
                return false;
            }

            m_nodes[coordinates] = node;
            return true;
        }

        public void RemoveNode(GraphCoordinates coordinates) => TryRemoveNode(coordinates);
        
        public bool TryRemoveNode(GraphCoordinates coordinates)
        {
            if (!m_nodes.ContainsKey(coordinates))
            {
                return false;
            }

            m_nodes.Remove(coordinates);
            return true;
        }

        public void Clear() => m_nodes.Clear();
    }
}