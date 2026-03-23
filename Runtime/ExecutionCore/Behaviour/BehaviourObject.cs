using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [CreateAssetMenu(fileName = "New Behaviour Object", menuName = "Behaviour Execution/Behaviour Object")]
    public class BehaviourObject : ScriptableObject
    {
        public BehaviourEvent[] BehaviourEvents => m_behaviourEvents.ToArray();
        public BehaviourAction[] BehaviourActions => m_behaviourActionBuffer.BehaviourActions;
        public BehaviourActionBuffer BehaviourActionBuffer => m_behaviourActionBuffer;
        public BehaviourBlackboard[] BehaviourBlackboards => m_behaviourBlackboards.ToArray();
        [SerializeReference] private List<BehaviourEvent> m_behaviourEvents = new();
        [SerializeField] private BehaviourActionBuffer m_behaviourActionBuffer = new("Actions");
        [SerializeReference] private List<BehaviourBlackboard> m_behaviourBlackboards = new();
    }
}
