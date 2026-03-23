using System;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public class BehaviourActionVariable : BehaviourActionField
    {
        public string Name => m_variableName;
        public Type VariableType
        {
            get
            {
                if(m_variableType == null)
                {
                    m_variableType = Type.GetType(m_serializedVariableType);
                }
                
                return m_variableType;
            }
        }
        public string TargetVariableName { get => m_targetVariableName; set => m_targetVariableName = value; }
        
        [SerializeField] private string m_variableName;
        [SerializeField] private Type m_variableType;
        [SerializeField] private string m_serializedVariableType;
        [SerializeField] private string m_targetVariableName;
        public BehaviourActionVariable(string name, Type type)
        {
            m_variableName = name;
            m_variableType = type;
            m_serializedVariableType = type.AssemblyQualifiedName;
        }
    }
}