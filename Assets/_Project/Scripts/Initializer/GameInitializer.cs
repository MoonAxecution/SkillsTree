using Game.Player;
using Game.Skills;
using UnityEngine;

namespace Game
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private SkillSettings _baseSkillSettings;
        [SerializeField] private string _nextSceneName;
        
        private void Awake()
        {
            CreateDependencies();
        }

        private void Start()
        {
            ScenesLoader.LoadScene(_nextSceneName);
        }

        private void CreateDependencies()
        {
            DependencyInjector.ReplaceComponent(new PlayerService(new PlayerEntity()) as IPlayerService);
            DependencyInjector.ReplaceComponent(CreateSkillsTreeService() as ISkillsTreeService);
        }

        private SkillsTreeService CreateSkillsTreeService()
        {
            return new SkillsTreeFactory().Create(_baseSkillSettings);
        }
    }
}