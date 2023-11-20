using System;
using System.Collections.Generic;
using System.Linq;
using Game.Player;

namespace Game.Skills
{
    public class SkillsTreeService : ISkillsTreeService
    {
        [Inject] private IPlayerService _playerService;

        private readonly Dictionary<int, ISkillEntity> _skills;
        private readonly HashSet<ISkillEntity> _learnedSkills = new HashSet<ISkillEntity>();

        private SkillType _lastCheckedSkillForForgetting = SkillType.Walk;

        public event Action TreeUpdated;

        public SkillsTreeService(Dictionary<int, ISkillEntity> skills)
        {
            this.Inject();
            _skills = skills;
        }

        public bool Learn(SkillType skillType)
        {
            ISkillEntity skill = GetSkill(skillType);
            
            if (!IsLearnAvailable(skill)) return false;
            
            _playerService.DecreaseSkillPoints(skill.SkillData.SkillPointsPrice);
            skill.Learn();
            _learnedSkills.Add(skill);
            TreeUpdated.Fire();
            
            return true;
        }

        public void Forget(SkillType skillType)
        {
            ISkillEntity skill = GetSkill(skillType);
            
            if (!IsForgetAvailable(skill, skillType)) return;

            Forget(skill);
            _learnedSkills.Remove(skill);
            TreeUpdated.Fire();
        }
        
        private void Forget(ISkillEntity skill)
        {
            _playerService.IncreaseSkillPoints(skill.SkillData.SkillPointsPrice);
            skill.Forget();
        }

        public void ForgetAll()
        {
            foreach (ISkillEntity skillEntity in _learnedSkills)
                Forget(skillEntity);

            _learnedSkills.Clear();
            TreeUpdated.Fire();
        }

        public bool IsLearnAvailable(ISkillEntityReadOnly skill)
        {
            return !skill.IsLearned
                   && _playerService.SkillPoints.Value >= skill.SkillData.SkillPointsPrice
                   && skill.DependentSkills.Any(dependentSkill => dependentSkill.IsLearned);
        }

        public bool IsForgetAvailable(ISkillEntityReadOnly skill)
        {
            _lastCheckedSkillForForgetting = skill.SkillData.Type;
            
            return !skill.SkillData.IsUnforgettable 
                   && skill.IsLearned 
                   && new SkillPathSearcher(skill).CanBeForgotten();
        }

        private bool IsForgetAvailable(ISkillEntityReadOnly skill, SkillType skillType)
        {
            return _lastCheckedSkillForForgetting == skillType || IsForgetAvailable(skill);
        }

        private int GetSkillPrice(SkillType skillType) => GetSkill(skillType).SkillData.SkillPointsPrice;

        public ISkillEntity GetSkill(SkillType skillType) => _skills[(int) skillType];
    }
}