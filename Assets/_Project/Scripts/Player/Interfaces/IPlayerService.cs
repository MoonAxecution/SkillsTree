using System;

namespace Game.Player
{
    public interface IPlayerService
    {
        event Action SkillPointsChanged;
        
        IReactiveProperty<int> SkillPoints { get; }

        void IncreaseSkillPoints(int value);
        void DecreaseSkillPoints(int value);
    }
}