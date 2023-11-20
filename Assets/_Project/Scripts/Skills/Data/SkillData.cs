using System;
using UnityEngine;

namespace Game.Skills
{
    [Serializable]
    public class SkillData
    {
        [field: SerializeField]
        public SkillType Type { get; private set; }
        
        [field: SerializeField]
        public string Name { get; private set; }
        
        [field: SerializeField]
        public bool IsUnforgettable { get; private set; }
        
        [field: SerializeField]
        public bool IsLearnedByDefault { get; private set; }
        
        [field: SerializeField]
        public int SkillPointsPrice { get; private set; }
    }
}