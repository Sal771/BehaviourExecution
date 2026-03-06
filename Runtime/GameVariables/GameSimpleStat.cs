using UnityEngine;
using System;
using System.Collections.Generic;

namespace com.Sal77.GameVariables
{

    [Serializable]
    public class GameSimpleStat<T>
    {
        [SerializeField] protected T m_value;
        public Action<T> OnValueChanged;
        public T Value
        {
            get { return m_value; }
            set
            {
                m_value = value;

                OnValueChanged?.Invoke(m_value);
            }
        }
    }

}