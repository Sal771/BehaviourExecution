using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public abstract class BehaviourAction : BehaviourSource
    {
        public BehaviourAction NextAction
        {
            get => m_nextAction;
            set => m_nextAction = value;
        }
        public BehaviourAction ParentAction
        {
            get => m_nextAction;
            set => m_nextAction = value;
        }
        [SerializeField] private BehaviourAction m_nextAction;
        public string ActionName => GetActionName();
        protected abstract string GetActionName();
        public abstract ExecutionActionResult Execute(IBehaviourExecution executionContext);
        public virtual bool WaitCondition(IBehaviourExecution executionContext){return true;}
    }
}
