using System;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public class BehaviourNumberOption : BehaviourActionField
    {
        public string Name => m_name;
        public int Value {get { return m_value; } set { m_value = value; }}

        [SerializeField] private string m_name;
        [SerializeField] private int m_value;
        public BehaviourNumberOption(string name)
        {
            m_name = name;
        }
    }
}