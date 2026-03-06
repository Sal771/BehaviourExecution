using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public abstract class BehaviourEvent : BehaviourSource
    {
        public string EventName => GetEventName();
        protected abstract string GetEventName();
    }
}
