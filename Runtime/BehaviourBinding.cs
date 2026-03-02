using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public class BehaviourBinding
    {
        public Type Type
        {
            get
            {
                if(m_type == null)
                {
                    m_type = Type.GetType(m_serializedType);
                }

                return m_type;
            }
        }
        [NonSerialized] private Type m_type;
        [SerializeField] private string m_serializedType;
        public BehaviourVariable TargetVariable
        {
            get => m_targetVariable;
            set =>  m_targetVariable = value;
        }
        [SerializeReference] private BehaviourVariable m_targetVariable;
        public BehaviourSource Source
        {
            get => m_source;
            set => m_source = value;
        }
        [SerializeReference] private BehaviourSource m_source;
        public string SourceKey
        {
            get => m_sourceKey;
            set => m_sourceKey = value;
        }
        [SerializeField] private string m_sourceKey;

        public BehaviourBinding(Type type, BehaviourVariable targetVariable, BehaviourSource behaviourSource, string sourceKey)
        {
            m_targetVariable = targetVariable;
            m_type = type;
            m_serializedType = m_type.AssemblyQualifiedName;
            m_source = behaviourSource;
            m_sourceKey = sourceKey;
        }
    }
}
