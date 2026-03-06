using System;
using UnityEngine;
using com.Sal77.GameCore;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public class BehaviourVariable
    {
        public string Name
        {
            get => m_name;
            set => m_name = value;
        }
        public Type Type => m_multiTypeValue.Type;
        public MultiTypeObject MultiTypeValue
        {
            get
            {
                if(m_multiTypeValue == null)
                {
                    m_multiTypeValue = new();
                }
                m_multiTypeValue.Type = Type;
                return m_multiTypeValue;
            }
        }

        [SerializeField] private string m_name;
        [SerializeField] private MultiTypeObject m_multiTypeValue = new();
        [SerializeField] private VariableType m_variableType;

        public enum VariableType
        {
            Buffer,
            Configurable
        }
    }
}
