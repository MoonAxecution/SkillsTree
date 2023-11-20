using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Skills.UI
{
    public class SkillView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SkillType _skillType;
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private Graphic _image;
        
        public event Action<SkillView> Selected;
        public event Action<SkillView> StateChanged;

        public ISkillEntityReadOnly Skill { get; private set; }
        public int Price => Skill.SkillData.SkillPointsPrice;
        public bool IsLearned { get; private set; }
        
        public SkillType SkillType => _skillType;

        public void Init(ISkillEntityReadOnly skill)
        {
            Skill = skill;
            Skill.Learned += Learn;
            Skill.Forgotten += Forget;

            IsLearned = skill.IsLearned;
            _nameLabel.text = skill.SkillData.Name;
        }

        public void ChangeImageColor(Color color)
        {
            _image.color = color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Selected.Fire(this);
        }
        
        private void Learn()
        {
            IsLearned = true;
            StateChanged.Fire(this);
        }

        private void Forget()
        {
            IsLearned = false;
            StateChanged.Fire(this);
        }

        private void OnDestroy()
        {
            Skill.Learned -= Learn;
            Skill.Forgotten -= Forget;
        }
    }
}