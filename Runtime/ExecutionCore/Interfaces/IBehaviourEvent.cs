using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public interface IBehaviourEvent
    {
        public void AddVariable<T>(string name);
    }
}