using Game.Player;

namespace Game.Skills
{
    public class SkillPointsChangerService
    {
        [Inject] private readonly IPlayerService _playerService;

        private const int EarnedSkillPoints = 1;
        
        public void Add()
        {
            _playerService.IncreaseSkillPoints(EarnedSkillPoints);
        }

        public void Spend(int count)
        {
            _playerService.DecreaseSkillPoints(count);
        }
    }
}