using System.Collections.Generic;

namespace Game.Skills
{
    public class SkillsTreeFactory
    {
        private readonly SkillEntityBuilder _skillEntityBuilder;
        
        private Dictionary<int, ISkillEntity> _skills;

        public SkillsTreeFactory()
        {
            _skillEntityBuilder = new SkillEntityBuilder();
        }
        
        public SkillsTreeService Create(SkillSettings baseSkillSettings)
        {
            return new SkillsTreeService(CreateSkills(baseSkillSettings));
        }
        
        private Dictionary<int, ISkillEntity> CreateSkills(SkillSettings baseSkillSettings)
        {
            _skills = new Dictionary<int, ISkillEntity>(baseSkillSettings.DependentSkills.Length + 1);
            CreateSkill(baseSkillSettings);
            return _skills;
        }

        private ISkillEntity CreateSkill(SkillSettings skillSettings)
        {
            var skillEntity = new SkillEntity(skillSettings.Data);
            _skills.Add((int)skillSettings.Data.Type, skillEntity);
            
            return BuildSkill(skillEntity, CreateSkillDependencies(skillSettings.DependentSkills));
        }

        private ISkillEntity[] CreateSkillDependencies(SkillSettings[] dependentSkillsSettings)
        {
            var dependentSkills = new ISkillEntity[dependentSkillsSettings.Length];
            
            for (int i = 0; i < dependentSkillsSettings.Length; i++)
            {
                SkillSettings dependentSkillSettings = dependentSkillsSettings[i];
                _skills.TryGetValue((int)dependentSkillSettings.Data.Type, out ISkillEntity dependentSkill);
                dependentSkills[i] = dependentSkill ?? CreateSkill(dependentSkillSettings);
            }

            return dependentSkills;
        }

        private ISkillEntity BuildSkill(ISkillEntityBuildable skill, ISkillEntity[] dependentSkills)
        {
            return _skillEntityBuilder
                .StartBuilding(skill)
                .SetDependentSkills(dependentSkills)
                .Build();
        }
    }
}