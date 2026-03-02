using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public abstract class BehaviourSource : ScriptableObject
    {
        public SourceVariable[] SourceVariables => m_sourceVariables.ToArray();
        [SerializeField] private List<SourceVariable> m_sourceVariables = new();

        public void AddVariable(string name, Type type)
        {
            if(type == null) return;
            var newSourceVariable = new SourceVariable(name, type);
            m_sourceVariables.Add(newSourceVariable);
        }

        public void ClearVariables()
        {
            m_sourceVariables.Clear();
        }
        public SourceVariable Get(string name)
        {
            return m_sourceVariables.FirstOrDefault(x => x.Name == name);
        }
        public bool HasKey(string key)
        {
            return m_sourceVariables.Any(x => x.Name == key);
        }

        public abstract void DefineBindings(IBehaviourVariable actionContext);

        [Serializable]
        public class SourceVariable {
            public string Name => m_name;
            [SerializeField] private string m_name;
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
            public SourceVariable(string name, Type type)
            {
                m_name = name;
                m_type = type;
                m_serializedType = m_type.AssemblyQualifiedName;
            }
        }
    }
}
