using System;

namespace Game.Skills
{
    public interface ISkillEntityReadOnly
    {
        event Action Learned;
        event Action Forgotten;
        
        SkillData SkillData { get; }
        ISkillEntity[] DependentSkills { get; }
        bool IsLearned { get; }
    }
}