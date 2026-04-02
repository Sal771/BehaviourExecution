using System;
using UnityEngine;

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
        public Type Type => MultiTypeValue.Type;
        public MultiTypeObject MultiTypeValue
        {
            get
            {
                if(m_multiTypeValue == null)
                {
                    m_multiTypeValue = new();
                }

                return m_multiTypeValue;
            }
        }
        public BehaviourVariableMode VariableMode { get {return m_variableMode;} set {m_variableMode = value;} }

        [SerializeField] private string m_name;
        [SerializeField] private MultiTypeObject m_multiTypeValue;
        [SerializeField] private BehaviourVariableMode m_variableMode;

        public BehaviourVariable(string name, Type type)
        {
            m_name = name;
            m_multiTypeValue = new();
            m_multiTypeValue.Type = type;
        }
    }
}
