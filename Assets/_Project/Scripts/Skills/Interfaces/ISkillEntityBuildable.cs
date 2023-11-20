namespace Game.Skills
{
    public interface ISkillEntityBuildable
    {
        void SetDependentSkills(ISkillEntity[] dependentSkills);
    }
}