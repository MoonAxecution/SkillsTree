using System;

namespace Game.Skills
{
    public interface ISkillsTreeService
    {
        event Action TreeUpdated;
        
        bool Learn(SkillType skillType);
        bool IsLearnAvailable(ISkillEntityReadOnly skill);

        void Forget(SkillType skillType);
        void ForgetAll();
        bool IsForgetAvailable(ISkillEntityReadOnly skill);
        
        ISkillEntity GetSkill(SkillType skillType);
    }
}