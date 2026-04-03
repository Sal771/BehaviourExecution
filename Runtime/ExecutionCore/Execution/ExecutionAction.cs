using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public class ExecutionAction
    {
        public BehaviourAction BehaviourAction => m_behaviourAction;
        [SerializeField] private BehaviourAction m_behaviourAction;
        public ExecutionActionResult ExecutionResult => m_executionResult;
        [SerializeField] private ExecutionActionResult m_executionResult;
        
        public ExecutionAction(BehaviourAction behaviourAction)
        {
            m_behaviourAction = behaviourAction;
        }
        public void Execute(IBehaviourExecution executionContext)
        {
            m_executionResult = m_behaviourAction.Execute(executionContext);
        }
        public void Update(IBehaviourExecution executionContext)
        {   
            var result = m_behaviourAction.WaitCondition(executionContext);
            if(result) m_executionResult = ExecutionActionResult.Successful;
        }
    }
}