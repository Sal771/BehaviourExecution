using System;
using System.Collections.Generic;
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
        [SerializeField] private string m_name;
        public Type Type
        {
            get
            {
                if(m_type == null)
                {
                    if(m_serializedType == string.Empty) return null;
                    m_type = Type.GetType(m_serializedType);
                }

                return m_type;
            }
        }
        [NonSerialized] private Type m_type;
        [SerializeField] private string m_serializedType;
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

        public BehaviourVariable(string name, Type type, BehaviourSource source, string sourceKey)
        {
            m_type = type;
            m_serializedType = type.AssemblyQualifiedName;
            m_name = name;
            Source = source;
            m_sourceKey = sourceKey;
        }
    }
}
