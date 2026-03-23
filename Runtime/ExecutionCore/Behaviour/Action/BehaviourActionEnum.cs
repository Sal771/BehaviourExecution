using System;
using System.Reflection;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public class BehaviourActionEnum : BehaviourActionField
    {
        public string Name => m_name;
        public string[] Options => m_options;
        public string CurrentOption => m_options[m_currentOptionIndex];
        public int CurrentOptionIndex{ get => m_currentOptionIndex; set => m_currentOptionIndex = value;}

        [SerializeField] private string m_name;
        [SerializeField] private string[] m_options;
        [SerializeField] private int m_currentOptionIndex;
        public BehaviourActionEnum(string name, Type enumType)
        {
            m_name = name;
            FieldInfo[] enumFields = enumType.GetFields();

            // We need to skip the first value
            m_options = new string[enumFields.Length - 1];

            for(int i = 1; i < m_options.Length + 1; i++)
            {
                m_options[i - 1] = enumFields[i].Name;
            }
        }
    }
}