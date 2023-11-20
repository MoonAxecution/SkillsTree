using System;
using Game.Player;
using Game.UI;
using UnityEngine;

namespace Game.Skills.UI
{
    public class SkillsTreeScreen : MonoBehaviour
    {
        [Inject] private IPlayerService _playerService;
        [Inject] private ISkillsTreeService _skillsTreeService;

        private const string SkillPointsPrefix = "Очки навыков: ";
        
        [SerializeField] private ValueView _skillPointsView;
        [SerializeField] private SkillInfoView _skillInfoView;
        [SerializeField] private SkillView[] _skillViews;
        
        [SerializeField] private Color _selectedSkillColor;
        [SerializeField] private Color _learnedSkillColor;
        [SerializeField] private Color _unlearnedSkillColor;

        private SkillView _selectedSkill;
        
        private void Awake()
        {
            this.Inject();

            _playerService.SkillPointsChanged += UpdateInfoButtons;
            _skillsTreeService.TreeUpdated += UpdateInfoButtons;

            InitStartdSelectedSkill();
            InitSkillPointsView();
            InitSkillInfoView();
            InitSkillViews();
        }

        private void InitStartdSelectedSkill()
        {
            ChangeSelectedSkill(_skillViews[0]);
        }

        private void InitSkillPointsView()
        {
            _skillPointsView.Init(SkillPointsPrefix, DependencyInjector.Resolve<IPlayerService>().SkillPoints);
        }

        private void InitSkillInfoView()
        {
            _skillInfoView.Start();
            _skillInfoView.Learning += Learn;
            _skillInfoView.Forgetting += Forget;
            _skillInfoView.ForgettingAll += ForgetAll;
        }

        private void InitSkillViews()
        {
            foreach (SkillView skillView in _skillViews)
            {
                skillView.Init(GetSkill(skillView.SkillType));
                skillView.Selected += OnSkillSelected;
                skillView.StateChanged += OnSkillStateChanged;
            }
        }

        private void UpdateInfoButtons()
        {
            _skillInfoView.UpdateButtonsState(_skillsTreeService.IsLearnAvailable(_selectedSkill.Skill), 
                _skillsTreeService.IsForgetAvailable(_selectedSkill.Skill));
            
            _skillInfoView.UpdateSkillPriceLabel(_selectedSkill.Price.ToString());
        }

        private void OnSkillSelected(SkillView skillView)
        {
            if (_selectedSkill == skillView) return;
            
            ChangeSelectedSkill(skillView);
            UpdateInfoButtons();
        }

        private void OnSkillStateChanged(SkillView skillView)
        {
            if (skillView == _selectedSkill) return;
            
            skillView.ChangeImageColor(GetColorForSkill(skillView));
        }

        private void ChangeSelectedSkill(SkillView selectedSkillView)
        {
            if (_selectedSkill != null)
                _selectedSkill.ChangeImageColor(GetColorForSkill(_selectedSkill));

            _selectedSkill = selectedSkillView;
            _selectedSkill.ChangeImageColor(_selectedSkillColor);
        }

        private Color GetColorForSkill(SkillView skillView) 
            => skillView.IsLearned ? _learnedSkillColor : _unlearnedSkillColor;

        private void Learn()
        {
            _skillsTreeService.Learn(_selectedSkill.SkillType);
        }

        private void Forget()
        {
            _skillsTreeService.Forget(_selectedSkill.SkillType);
        }

        private void ForgetAll()
        {
            _skillsTreeService.ForgetAll();
        }
        
        private ISkillEntityReadOnly GetSkill(SkillType skillType) => _skillsTreeService.GetSkill(skillType);

        private void OnDestroy()
        {
            _playerService.SkillPointsChanged -= UpdateInfoButtons;
            _skillsTreeService.TreeUpdated -= UpdateInfoButtons;
        }
    }
}