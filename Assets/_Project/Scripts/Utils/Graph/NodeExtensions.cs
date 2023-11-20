using System.Collections.Generic;
using System.Linq;
using Game.Skills;

namespace Game.Graph
{
    public static class NodeExtensions
    {
        public static IEnumerable<ISkillEntity> BreadthSearch(this ISkillEntity startNode)
        {
            var visited = new HashSet<ISkillEntity> {startNode};
            var queue = new Queue<ISkillEntity>();
            queue.Enqueue(startNode);
            
            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                yield return node;
                
                foreach (var nextNode in node.DependentSkills.Where(n => !visited.Contains(n)))
                {
                    visited.Add(nextNode);
                    queue.Enqueue(nextNode);
                }
            }
        }
        
        public static IEnumerable<ISkillEntity> DepthSearch(this ISkillEntity startNode)
        {
            var visited = new HashSet<ISkillEntity> {startNode};
            var stack = new Stack<ISkillEntity>();
            stack.Push(startNode.DependentSkills[0]);
            
            while (stack.Count != 0)
            {
                var node = stack.Pop();
                yield return node;
                foreach (var nextNode in node.DependentSkills.Where(n => !visited.Contains(n)))
                {
                    visited.Add(nextNode);
                    stack.Push(nextNode);
                }
            }
        }
    }
}