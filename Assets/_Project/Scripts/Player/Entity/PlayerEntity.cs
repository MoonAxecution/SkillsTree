namespace Game.Player
{
    public class PlayerEntity
    {
        public ReactiveProperty<int> SkillPoints { get; } = new ReactiveProperty<int>();

        public void IncreaseSkillPoints(int value)
        {
            SkillPoints.Value += value;
        }

        public void DecreaseSkillPoints(int value)
        {
            SkillPoints.Value -= value;
        }
    }
}