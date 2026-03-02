using System;
using UnityEngine;
using com.Sal77.GameCore;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public class BehaviourConstant : BehaviourSource
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
                    m_type = Type.GetType(m_serializedType);
                }
                return m_type;
            }
            set
            {
                m_type = value;
                m_serializedType = m_type.AssemblyQualifiedName;
                MultiTypeValue.Type = m_type;
            }
        }
        [NonSerialized] private Type m_type;
        [SerializeField] private string m_serializedType;
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
        [SerializeField] private MultiTypeObject m_multiTypeValue = new();

        public override void DefineBindings(IBehaviourVariable actionContext)
        {
            actionContext.DeclareVariable(m_name, Type);
        }
    }
}
