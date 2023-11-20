using UnityEngine;

namespace Game.Skills
{
    [CreateAssetMenu(fileName = "SkillSettings", menuName = "Game/Settings/Skills/Skill")]
    public class SkillSettings : ScriptableObject
    {
        [field: SerializeField]
        public SkillData Data { get; private set; }
        
        [field: SerializeField]
        public SkillSettings[] DependentSkills { get; private set; }
    }
}