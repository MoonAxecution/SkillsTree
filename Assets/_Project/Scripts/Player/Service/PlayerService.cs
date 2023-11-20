using System;

namespace Game.Player
{
    public class PlayerService : IPlayerService
    {
        private readonly PlayerEntity _playerEntity;

        public IReactiveProperty<int> SkillPoints => _playerEntity.SkillPoints;

        public event Action SkillPointsChanged;
        
        public PlayerService(PlayerEntity playerEntity)
        {
            _playerEntity = playerEntity;
        }
        
        public void IncreaseSkillPoints(int value)
        {
            if (value < 0)
                throw new ArgumentException("Value must be greater than 0");
            
            _playerEntity.IncreaseSkillPoints(value);
            SkillPointsChanged.Fire();
        }

        public void DecreaseSkillPoints(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Value must be greater than 0");
            
            _playerEntity.DecreaseSkillPoints(value);
            SkillPointsChanged.Fire();
        }
    }
}