using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Graph
{
    public readonly struct GraphVarieties<TGraphID, TVarietyType> where TVarietyType : Enum
    {
        private readonly (TGraphID id, TVarietyType variety)[] m_varieties;
        
        public static GraphVarieties<TGraphID, TVarietyType> Empty =>
            new(Array.Empty<(TGraphID id, TVarietyType variety)>());
        
        public GraphVarieties(IEnumerable<(TGraphID id, TVarietyType variety)> varieties) =>
            m_varieties = varieties.Select(entry => (entry.id, entry.variety)).ToArray();

        public MutableGraphVarieties<TGraphID, TVarietyType> CloneMutable() => new(m_varieties);

        public bool HasVariety(TGraphID id) => TryGetVariety(id, out _);
        
        public bool IsVariety(TGraphID id, TVarietyType variety)
        {
            var element = (id, variety);

            return m_varieties
                .Any(entry =>
                    EqualityComparer<(TGraphID, TVarietyType)>.Default
                        .Equals((entry.id, entry.variety), element));
        }

        public TVarietyType GetVariety(TGraphID id) => TryGetVariety(id, out var variety) ? variety : default;

        public bool TryGetVariety(TGraphID id, out TVarietyType variety)
        {
            var matches = m_varieties
                .Where(entry => entry.id.Equals(id))
                .Select(entry => entry.variety)
                .Take(1)
                .ToList();

            if (!matches.Any())
            {
                variety = default;
                return false;
            }

            variety = matches.First();
            return true;
        }

        public IEnumerable<TGraphID> ElementsOfVariety(TVarietyType variety) => m_varieties
            .Where(entry => EqualityComparer<TVarietyType>.Default.Equals(entry.variety, variety))
            .Select(entry => entry.id);
    }
}