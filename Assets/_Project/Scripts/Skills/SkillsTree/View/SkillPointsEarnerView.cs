using UnityEngine;
using UnityEngine.UI;

namespace Game.Skills.UI
{
    public class SkillPointsEarnerView : MonoBehaviour
    {
        [Inject] private SkillPointsChangerService _skillPointsChangerService;
        
        [SerializeField] private Button _earnButton;

        private void Awake()
        {
            this.Inject();
            _earnButton.onClick.AddListener(Earn);
        }
        
        private void Earn()
        {
            _skillPointsChangerService.Add();
        }
    }
}