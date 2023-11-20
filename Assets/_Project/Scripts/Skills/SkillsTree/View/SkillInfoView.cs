using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Skills.UI
{
    [Serializable]
    public class SkillInfoView
    {
        [SerializeField] private Button _learnButton;
        [SerializeField] private Button _forgetButton;
        [SerializeField] private Button _forgetAllButton;
        [SerializeField] private TMP_Text _skillPriceLabel;

        public event Action Learning;
        public event Action Forgetting;
        public event Action ForgettingAll;
        
        public void Start()
        {
            _learnButton.onClick.AddListener(Learn);
            _forgetButton.onClick.AddListener(Forget);
            _forgetAllButton.onClick.AddListener(ForgetAll);
        }

        public void UpdateButtonsState(bool isLearnAvailable, bool isForgetAvailable)
        {
            _learnButton.gameObject.SetActive(isLearnAvailable);
            _forgetButton.gameObject.SetActive(isForgetAvailable);
        }

        public void UpdateSkillPriceLabel(string price)
        {
            _skillPriceLabel.text = price;
        }
        
        private void Learn()
        {
            Learning.Fire();
        }

        private void Forget()
        {
            Forgetting.Fire();
        }

        private void ForgetAll()
        {
            ForgettingAll.Fire();
        }
    }
}