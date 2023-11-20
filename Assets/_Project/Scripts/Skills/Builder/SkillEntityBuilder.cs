namespace Game.Skills
{
    public class SkillEntityBuilder
    {
        private ISkillEntityBuildable _skillEntity;

        public SkillEntityBuilder StartBuilding(ISkillEntityBuildable skillEntity)
        {
            _skillEntity = skillEntity;
            return this;
        }

        public SkillEntityBuilder SetDependentSkills(ISkillEntity[] dependentSkills)
        {
            _skillEntity.SetDependentSkills(dependentSkills);
            return this;
        }
        
        public SkillEntity Build() => _skillEntity as SkillEntity;
    }
}