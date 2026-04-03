using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public class ExecutionRunner : MonoBehaviour
    {
        public ExecutionObject[] ExecutionObjects => m_executionObjects.ToArray();
        [SerializeField] private List<ExecutionObject> m_executionObjects = new();
        public void Update()
        {
            foreach(var executionObject in m_executionObjects)
            {
                if(!executionObject.Completed) executionObject.Update();
            }
            
        }

        public void Execute(BehaviourObject behaviourObject)
        {
            Execute(behaviourObject, null);
        }
        public void Execute<T>(BehaviourObject behaviourObject, params object[] eventParams)
        {
            Execute(behaviourObject, typeof(T), eventParams);
        }
        public void Execute(BehaviourObject behaviourObject, Type eventType, params object[] eventParams)
        {
            var executionObject = new ExecutionObject(behaviourObject, eventType, eventParams);

            if (executionObject.BehaviourObject.BehaviourActions.Length == 0) return;
            m_executionObjects.Add(executionObject);
            executionObject.Update();
        }
        public void StopExecute(ExecutionObject executionObject)
        {
            m_executionObjects.Remove(executionObject);
        }
    }
}
