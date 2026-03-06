using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public class ExecutionObject : IBehaviourExecution
    {
        public BehaviourObject BehaviourObject => m_behaviourObject;
        [SerializeField] private BehaviourObject m_behaviourObject;
        public ExecutionVariable[] ExecutionVariables => m_executionVariables.ToArray();
        [SerializeField] private List<ExecutionVariable> m_executionVariables = new();
        public ExecutionAction[] ExecutionActions => m_executionActions.ToArray();
        [SerializeField] private List<ExecutionAction> m_executionActions = new();
        public ExecutionAction CurrentAction
        {
            get => m_currentAction;
            set => m_currentAction = value;   
        }
        [SerializeReference] private ExecutionAction m_currentAction;

        public ExecutionObject(BehaviourObject behaviourObject)
        {
            m_behaviourObject = behaviourObject;
            Setup();

            Reset();
        }

        public ExecutionObject(BehaviourObject behaviourObject, Type eventType, params object[] eventParams)
        {
            m_behaviourObject = behaviourObject;
            Setup();

            Reset(eventType, eventParams);
        }

        public void DefineVariable(string name, Type type)
        {
            var variable = new ExecutionVariable(name, type);

            m_executionVariables.Add(variable);
        }

        public void WriteVariable(string name, Type type, object value)
        {
            var variable = m_executionVariables.FirstOrDefault(x => x.Name == name);

            if(variable == null)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to write variable '{name}' not found within behaviour");
                return;
            }

            if(variable.Type != type)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to write variable '{name}' found with mismatching type");
                return;
            }

            variable.Set(value, type);
        }

        public void WriteVariable<T>(BehaviourBinding variableBinding, T value)
        {
            var variable = m_executionVariables.FirstOrDefault(x => x.Name == variableBinding.TargetVariable.Name);

            if(variable == null)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to write variable '{variable.Name}' not found within behaviour");
                return;
            }

            if(variable.Type != value.GetType())
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to write variable '{variable.Name}' found with mismatching type");
                return;
            }

            variable.Set(value);
        }

        public T ReadVariable<T>(BehaviourBinding variableBinding)
        {
            var variable = m_executionVariables.FirstOrDefault(x => x.Name == variableBinding.TargetVariable.Name);

            if(variable == null)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to read variable '{variable.Name}' not found within behaviour");
                return default;
            }

            if(variable.Type != typeof(T))
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to read variable '{variable.Name}' found with mismatching type");
                return default;
            }

            return variable.Get<T>();
        }
        public void Setup()
        {
            /*foreach(var variable in m_behaviourObject.BehaviourVariables)
            {
                DefineVariable(variable.Name, variable.Type);
            }

            Dictionary<BehaviourAction, ExecutionAction> actionCache = new();

            foreach(var action in m_behaviourObject.BehaviourActions)
            {
                ExecutionAction executionAction = new ExecutionAction(action);
                m_executionActions.Add(executionAction);
                actionCache.Add(action, executionAction);
            }

            foreach(var actionPair in actionCache)
            {
                var nextAction = actionPair.Key.NextAction;
                if(nextAction != null)
                {
                    actionPair.Value.NextAction = actionCache[nextAction];
                }
            }*/
        }

        public void Reset()
        {
            Reset(null);
        }

        public void Reset(Type eventType, params object[] eventParams)
        {
            /*var behaviourEvent = m_behaviourObject.BehaviourEvents.First(x => x.GetType() == eventType);

            var eventSourceVariables = behaviourEvent.SourceVariables;

            for(int i=0; i<eventParams.Length; i++)
            {
                WriteVariable(eventSourceVariables[i].Name, eventSourceVariables[i].Type, eventParams[i]);
            }

            foreach(var constant in m_behaviourObject.BehaviourConstants)
            {
                WriteVariable(constant.Name, constant.Type, constant.MultiTypeValue.GetValue());
            }

            foreach(var constant in m_behaviourObject.BehaviourConstants)
            {
                WriteVariable(constant.Name, constant.Type, constant.MultiTypeValue.GetValue());
            }

            if (m_executionActions.Count > 0)
            {
                m_currentAction = m_executionActions[0];
            }*/
        }

        public void Update()
        {
            if(m_currentAction != null)
            {
                if(m_currentAction.ExecutionResult == ExecutionActionResult.Pending)
                {
                    m_currentAction.Execute(this);
                } else
                {
                    m_currentAction.Update(this);
                }

                if (m_currentAction.ExecutionResult == ExecutionActionResult.Successful)
                {
                    m_currentAction = m_currentAction.NextAction;
                }
            }
        }
    }
}