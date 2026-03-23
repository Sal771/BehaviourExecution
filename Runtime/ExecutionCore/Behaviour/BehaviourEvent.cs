using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public abstract class BehaviourEvent : ScriptableObject, IBehaviourEvent
    {
        public string EventName => GetEventName();
        protected abstract string GetEventName();
        public BehaviourEventVariable[] EventVariables => m_eventVariables.ToArray();

        [SerializeField] private List<BehaviourEventVariable> m_eventVariables = new();

        [Serializable]
        public class BehaviourEventVariable
        {
            public string Name => m_name;
            public Type Type
            {
                get
                {
                    if(m_type == null)
                    {
                        m_type = Type.GetType(m_serializedVariableType);
                    }

                    return m_type;
                }
            }

            [SerializeField] private string m_name;
            [SerializeField] private Type m_type;
            [SerializeField] private string m_serializedVariableType;

            public BehaviourEventVariable(string name, Type type)
            {
                m_name = name;
                m_type = type;
                m_serializedVariableType = type.AssemblyQualifiedName;
            }
        }

        public void UpdateBindings()
        {
            m_eventVariables.Clear();

            DefineBindings(this);
        }
        protected abstract void DefineBindings(IBehaviourEvent eventContext);

        public void AddVariable<T>(string name)
        {
            m_eventVariables.Add(new BehaviourEventVariable(name, typeof(T)) );
        }

    }
}
