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

        public void ExecuteDirectly(BehaviourObject behaviourObject)
        {
            ExecuteDirectly(behaviourObject, null);
        }
        public void ExecuteDirectly<T>(BehaviourObject behaviourObject, params object[] eventParams)
        {
            ExecuteDirectly(behaviourObject, typeof(T), eventParams);
        }
        public void ExecuteDirectly(BehaviourObject behaviourObject, Type eventType, params object[] eventParams)
        {
            if(behaviourObject == null) return;

            var executionObject = new ExecutionObject(behaviourObject, eventType, eventParams);

            if (executionObject.BehaviourObject.BehaviourActions.Length == 0) return;
            m_executionObjects.Add(executionObject);
            executionObject.Update();
        }
        public void AddBehaviour(BehaviourObject behaviourObject)
        {
            
        }
        public void RemoveBehaviour(BehaviourObject behaviourObject)
        {
            
        }
        public void RemoveAllBehaviours()
        {
            
        }
        public void RemoveAllBehaviourOfCategory<T>()
        {
            
        }
        public void Execute<T>(params object[] eventParams)
        {
            
        }
        public void StopExecute(ExecutionObject executionObject)
        {
            m_executionObjects.Remove(executionObject);
        }
    }
}
