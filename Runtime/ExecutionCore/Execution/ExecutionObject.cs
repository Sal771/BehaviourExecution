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

        private void DefineVariable(string variableName, Type type)
        {
            var variable = new ExecutionVariable(variableName, type);

            m_executionVariables.Add(variable);
        }

        public void WriteVariable(string variableName, Type type, object value)
        {
            var sourceVariable = m_currentAction.BehaviourAction.ActionVariables.FirstOrDefault(x => x.Name == variableName);
            
            if(sourceVariable == null)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to write variable '{variableName}' not found within action.");
                return;
            }

            var variable = m_executionVariables.FirstOrDefault(x => x.Name == sourceVariable.TargetVariableName);

            if(variable == null)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to write variable '{variableName}' not found within behaviour");
                return;
            }

            if(variable.Type != type)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to write variable '{variableName}' found with mismatching type");
                return;
            }

            variable.Set(value, type);
        }

        public void WriteVariable<T>(string variableName, T value)
        {
            WriteVariable(variableName, typeof(T), value);
        }
        public void WriteVariable(string variableName, MultiTypeObject source)
        {
            ExecutionVariable variable;

            if(m_currentAction != null)
            {
                var sourceVariable = m_currentAction.BehaviourAction.ActionVariables.FirstOrDefault(x => x.Name == variableName);
            
                if(sourceVariable == null)
                {
                    Debug.LogWarning($"{m_behaviourObject.name} - Attempted to write variable '{variableName}' not found within action.");
                    return;
                }
                
                variable = m_executionVariables.FirstOrDefault(x => x.Name == sourceVariable.TargetVariableName);
            }
            else
            {
                variable = m_executionVariables.FirstOrDefault(x => x.Name == variableName);
            }

            if(variable == null)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to write variable '{variable.Name}' not found within behaviour.");
                return;
            }

            if(variable.Type != source.Type)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to write variable '{variable.Name}' found with mismatching type.");
                return;
            }

            variable.Set(source);
        }

        public T ReadVariable<T>(string variableName)
        {
            var sourceVariable = m_currentAction.BehaviourAction.ActionVariables.FirstOrDefault(x => x.Name == variableName);
            
            if(sourceVariable == null)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to read variable '{variableName}' not found within action.");
                return default;
            }
            
            var variable = m_executionVariables.FirstOrDefault(x => x.Name == sourceVariable.TargetVariableName);

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

        public void ExecuteActionBuffer(string bufferName)
        {
            throw new NotImplementedException();//TODO Implement this
        }

        public void ResetVariable(string variableName)
        {
            var variable = m_executionVariables.FirstOrDefault(x => x.Name == variableName);

            variable.Reset();
        }
        public void Setup()
        {
            foreach(var blackboard in m_behaviourObject.BehaviourBlackboards)
            {
                foreach(var variable in blackboard.BehaviourVariables)
                {
                    DefineVariable(variable.Name, variable.Type);
                }
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
            }
        }

        public void Reset()
        {
            Reset(null);
        }

        public void Reset(Type eventType, params object[] eventParams)
        {
            if(eventType != null)
            {
                var behaviourEvent = m_behaviourObject.BehaviourEvents.FirstOrDefault(x => x.GetType() == eventType);

                //TODO Handle no event

                var eventSourceVariables = behaviourEvent.EventVariables;

                for(int i=0; i<eventParams.Length; i++)
                {
                    WriteVariable(eventSourceVariables[i].Name, eventSourceVariables[i].Type, eventParams[i]);
                }

            }

            foreach(var blackboard in m_behaviourObject.BehaviourBlackboards)
            {
                foreach(var variable in blackboard.BehaviourVariables)
                {
                    if (variable.VariableMode == BehaviourVariable.BehaviourVariableMode.Configurable)
                    {
                        WriteVariable(variable.Name, variable.MultiTypeValue);
                    }
                    else
                    {
                        ResetVariable(variable.Name);
                    }
                }
            }

            if (m_executionActions.Count > 0)
            {
                m_currentAction = m_executionActions[0];
            }
        }
    }
}