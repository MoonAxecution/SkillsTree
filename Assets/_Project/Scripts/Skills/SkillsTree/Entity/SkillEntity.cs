using System;

namespace Game.Skills
{
    public class SkillEntity : ISkillEntityBuildable, ISkillEntity
    {
        public event Action Learned;
        public event Action Forgotten;
        
        public SkillData SkillData { get; }
        public ISkillEntity[] DependentSkills { get; private set; }
        public bool IsLearned { get; private set; }

        public SkillEntity(SkillData skillData)
        {
            SkillData = skillData;
            IsLearned = SkillData.IsLearnedByDefault;
        }

        void ISkillEntityBuildable.SetDependentSkills(ISkillEntity[] dependentSkills)
        {
            DependentSkills = dependentSkills;
        }

        public void Learn()
        {
            IsLearned = true;
            Learned.Fire();
        }

        public void Forget()
        {
            IsLearned = false;
            Forgotten.Fire();
        }
    }
}