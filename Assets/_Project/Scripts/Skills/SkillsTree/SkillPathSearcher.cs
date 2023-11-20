using System.Collections.Generic;

namespace Game.Skills
{
    public class SkillPathSearcher
    {
        private const int BaseSkillId = 0;

        private readonly ISkillEntityReadOnly _selectedSkill;
        private readonly HashSet<ISkillEntityReadOnly> _visited = new HashSet<ISkillEntityReadOnly>();
        private readonly HashSet<SkillType> _traversedSuccessfulSkills = new HashSet<SkillType>();
        private readonly Stack<ISkillEntityReadOnly> _stack = new Stack<ISkillEntityReadOnly>();

        public SkillPathSearcher(ISkillEntityReadOnly selectedSkill)
        {
            _selectedSkill = selectedSkill;
        }

        public bool CanBeForgotten()
        {
            foreach (ISkillEntityReadOnly initialSkill in _selectedSkill.DependentSkills)
            {
                if (initialSkill.SkillData.Type == BaseSkillId || !initialSkill.IsLearned) continue;
                if (!IsPathToBaseSkillAvailable(initialSkill)) return false;
                
                _visited.Clear();
                _stack.Clear();
            }

            return true;
        }

        private bool IsPathToBaseSkillAvailable(ISkillEntityReadOnly initialSkill)
        {
            bool canBeForgotten = true;
            _visited.Add(_selectedSkill);
            _visited.Add(initialSkill);
            _stack.Push(initialSkill);

            while (_stack.Count != 0)
            {
                var currentSkill = _stack.Pop();

                if (_traversedSuccessfulSkills.Contains(currentSkill.SkillData.Type)) return true;
                
                _traversedSuccessfulSkills.Add(currentSkill.SkillData.Type);

                if (!currentSkill.IsLearned) continue;

                canBeForgotten = currentSkill.SkillData.Type == BaseSkillId;

                if (canBeForgotten) break;
                
                AddNextSkillsForTraverse(currentSkill);
            }

            return canBeForgotten;
        }

        private void AddNextSkillsForTraverse(ISkillEntityReadOnly currentSkill)
        {
            foreach (ISkillEntityReadOnly skillEntity in currentSkill.DependentSkills)
            {
                if (_visited.Contains(skillEntity)) continue;
                    
                _visited.Add(skillEntity);
                _stack.Push(skillEntity);
            }
        }
    }
}