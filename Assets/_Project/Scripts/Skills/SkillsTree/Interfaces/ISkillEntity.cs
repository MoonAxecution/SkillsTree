namespace Game.Skills
{
    public interface ISkillEntity : ISkillEntityReadOnly
    {
        void Learn();
        void Forget();
    }
}