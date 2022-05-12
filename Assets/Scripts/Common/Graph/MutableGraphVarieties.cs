using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Graph
{
    public class MutableGraphVarieties<TGraphID, TVarietyType> where TVarietyType : Enum
    {
        private readonly Dictionary<TGraphID, TVarietyType> m_varieties;
        
        public MutableGraphVarieties(IEnumerable<(TGraphID id, TVarietyType variety)> varieties) =>
            m_varieties = varieties
                .ToDictionary(element => element.id, element => element.variety);

        public GraphVarieties<TGraphID, TVarietyType> CloneImmutable() => new(m_varieties
            .Select(element => (element.Key, element.Value))
            .ToArray());

        public bool IsVariety(TGraphID id, TVarietyType variety) => GetVariety(id).Equals(variety);
        
        public TVarietyType GetVariety(TGraphID id) => m_varieties.TryGetValue(id, out var variety) ? variety : default; 
        
        public void SetVariety(TGraphID id, TVarietyType variety) => m_varieties[id] = variety;
        
        public void Clear() => m_varieties.Clear();
    }
}