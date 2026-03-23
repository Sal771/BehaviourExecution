using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public class BehaviourActionField
    {
        public int OrderIndex {get { return m_orderIndex;} set { m_orderIndex = value; }}

        [SerializeField] private int m_orderIndex = new();
    }
}
