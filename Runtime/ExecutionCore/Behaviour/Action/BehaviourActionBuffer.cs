using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public class BehaviourActionBuffer : BehaviourActionField
    {
        public BehaviourAction[] BehaviourActions => m_behaviourActions.ToArray();
        public string Name => m_name;

        [SerializeField] private List<BehaviourAction> m_behaviourActions = new();
        [SerializeField] private string m_name;

        public BehaviourActionBuffer(string name)
        {
            m_name = name;
        }

        public void Add(BehaviourAction behaviourAction)
        {
            m_behaviourActions.Add(behaviourAction);
        }
        public void AddRange(IEnumerable<BehaviourAction> behaviourActions)
        {
            m_behaviourActions.AddRange(behaviourActions);
        }

        public void Remove(BehaviourAction behaviourAction)
        {
            m_behaviourActions.Remove(behaviourAction);
        }
        
        public void RemoveAt(int index)
        {
            m_behaviourActions.RemoveAt(index);
        }

        public void InsertTo(BehaviourAction instance, int targetIndex)
        {
            m_behaviourActions.InsertTo(instance, targetIndex);
        }

        public void Clear()
        {
            m_behaviourActions.Clear();
        }
    }
}
