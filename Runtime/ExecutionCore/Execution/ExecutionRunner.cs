using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    public class ExecutionRunner : MonoBehaviour
    {
        [SerializeField] List<ExecutionObject> m_runningExecution = new();
        [SerializeField] private Dictionary<Type, List<BehaviourObject>> m_activeBehaviours = new();
        private Dictionary<Type, List<Action>> m_listeningActions = new();

        public void Update()
        {
            HashSet<ExecutionObject> objectsToRemove = new();

            foreach(var executionObject in m_runningExecution)
            {
                if(!executionObject.Completed)
                {
                    executionObject.Update();
                }
                else
                {
                    //objectsToRemove.Add(executionObject);
                }
            }

            foreach(var objectToRemove in objectsToRemove)
            {
                m_runningExecution.Remove(objectToRemove);
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

            m_runningExecution.Add(executionObject);
            executionObject.Update();
        }
        public void AddBehaviour(BehaviourObject behaviourObject)
        {
            foreach(var behaviourEvent in behaviourObject.BehaviourEvents)
            {
                Type behaviourType = behaviourEvent.GetType();

                if(!m_activeBehaviours.ContainsKey(behaviourType)) m_activeBehaviours[behaviourType] = new();
                
                m_activeBehaviours[behaviourType].Add(behaviourObject);
            }
        }
        public void RemoveBehaviour(BehaviourObject behaviourObject)
        {
            foreach(var behaviourEvent in behaviourObject.BehaviourEvents)
            {
                Type behaviourType = behaviourEvent.GetType();

                if(!m_activeBehaviours.ContainsKey(behaviourType)) continue;
                
                m_activeBehaviours[behaviourType].Remove(behaviourObject);
            }
        }
        public void RemoveAllBehaviours()
        {
            foreach(var behaviourEvent in m_activeBehaviours.Keys)
            {
                m_activeBehaviours[behaviourEvent].Clear();
            }
        }
        public void RemoveAllBehaviourOfEvent<T>()
        {
            if(!m_activeBehaviours.ContainsKey(typeof(T))) return;

            m_activeBehaviours[typeof(T)].Clear();
        }
        public bool HasBehaviour(BehaviourObject behaviourObject)
        {
            foreach(var behaviourEvent in behaviourObject.BehaviourEvents)
            {
                Type behaviourType = behaviourEvent.GetType();

                if(!m_activeBehaviours.ContainsKey(behaviourType)) continue;

                if(m_activeBehaviours[behaviourType].Count > 0)
                {
                    return true;
                }
            }

            return false;
        }
        public void Execute<T>(params object[] eventParams) where T : BehaviourEvent
        {
            if(!m_activeBehaviours.ContainsKey(typeof(T))) return;

            foreach(var behaviourObject in m_activeBehaviours[typeof(T)])
            {
                ExecuteDirectly<T>(behaviourObject, eventParams);
            }
        }
        public void StopExecute(BehaviourObject behaviourObject)
        {
            m_runningExecution.RemoveAll(x => x.BehaviourObject == behaviourObject);
        }
        public bool HasFinished(BehaviourObject behaviourObject)
        {
            return !m_runningExecution.Any(x => x.BehaviourObject == behaviourObject);
        }
        public bool HasFinishedAll<T>() where T : BehaviourEvent
        {
            
            return !m_runningExecution.Any( x => x.BehaviourObject.BehaviourEvents.Select(y => y.GetType()).Any(y => y == typeof(T)) );
        }
        public bool HasFinishedAll()
        {
            return m_runningExecution.Count > 0;
        }
        public void AddListener<T>(Action action) where T : BehaviourEvent
        {
            if(!m_listeningActions.ContainsKey(typeof(T))) m_listeningActions[typeof(T)] = new();

            m_listeningActions[typeof(T)].Add(action);
        }
        public void RemoveListener<T>(Action action) where T : BehaviourEvent
        {
            if(!m_listeningActions.ContainsKey(typeof(T))) return;

            m_listeningActions[typeof(T)].Remove(action);
        }
        public void RemoveAllListeners()
        {
            foreach(var subList in m_listeningActions)
            {
                subList.Value.Clear();
            }
        }
    }
}
