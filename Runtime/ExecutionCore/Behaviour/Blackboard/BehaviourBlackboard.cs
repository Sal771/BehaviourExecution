using System.Collections.Generic;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [CreateAssetMenu(fileName = "New Behaviour Object", menuName = "Behaviour Execution/Blackboard")]
    public class BehaviourBlackboard : ScriptableObject
    {
        public BehaviourVariable[] BehaviourVariables => m_behaviourVariables.ToArray();

        [SerializeField] private List<BehaviourVariable> m_behaviourVariables = new();

        public void Add(BehaviourVariable behaviourVariable)
        {
            m_behaviourVariables.Add(behaviourVariable);
        }

        public void Remove(BehaviourVariable behaviourVariable)
        {
            m_behaviourVariables.Remove(behaviourVariable);
        }

        public void InsertTo(BehaviourVariable behaviourVariable, int targetIndex)
        {
            m_behaviourVariables.InsertTo(behaviourVariable, targetIndex);
        }

        public int IndexOf(BehaviourVariable behaviourVariable)
        {
            return m_behaviourVariables.IndexOf(behaviourVariable);
        }
    }
}