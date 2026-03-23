using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public class ExecutionVariable
    {
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
            set
            {
                m_type = value;
                m_serializedType = m_type.FullName;
            }
        }
        private Type m_type;
        [SerializeField] private string m_serializedType;
        [SerializeField] private MultiTypeObject m_multiTypeObject = new();

        public ExecutionVariable(string name, Type type)
        {
            m_type = type;
            m_name = name;
            m_serializedType = m_type.FullName;
            m_multiTypeObject.Type = m_type;
        }
        public T Get<T>()
        {
            if(m_type != typeof(T)){
                Debug.Log($"{m_name} - Attempted to read variable of type '{m_type.Name}' with wrong type '{typeof(T).Name}'");
                return default;
            }
            return m_multiTypeObject.GetValue<T>();
        }
        public void Set<T>(T value)
        {
            Set(value, typeof(T));
        }
        public void Set(object value, Type type)
        {
            if(m_type != type){
                Debug.Log($"{m_name} - Attempted to write variable of type '{m_type.Name}' with wrong type '{type.Name}'");
                return;
            }
            m_multiTypeObject.SetValue(value, type);
        }
        public void Set(MultiTypeObject source)
        {
            Set(source);
        }
        public void Reset()
        {
            m_multiTypeObject.Reset();
        }
    }
}
