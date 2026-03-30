using System;
using System.Runtime.Serialization;
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
        public IBehaviourActionReadMode ReadMode => m_readMode;
        
        [SerializeField] private string m_variableName;
        [SerializeField] private Type m_variableType;
        [SerializeField] private string m_serializedVariableType;
        [SerializeField] private string m_targetVariableName;
        [SerializeField] private IBehaviourActionReadMode m_readMode;
        public BehaviourActionVariable(string name, Type type, IBehaviourActionReadMode readMode)
        {
            m_variableName = name;
            m_variableType = type;
            m_serializedVariableType = type.AssemblyQualifiedName;
            m_readMode = readMode;
        }
    }
}