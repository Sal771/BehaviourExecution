using System.Collections.Generic;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public class BehaviourBlackboard
    {
        public BehaviourVariable[] BehaviourVariables => m_behaviourVariables.ToArray();

        [SerializeField] private List<BehaviourVariable> m_behaviourVariables = new();
    }
}