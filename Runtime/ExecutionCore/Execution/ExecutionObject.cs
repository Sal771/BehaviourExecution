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
        public ExecutionVariable[] ExecutionVariables => m_executionVariables.ToArray();
        public ExecutionAction CurrentAction
        {
            get => m_currentAction;
            set => m_currentAction = value;   
        }
        public bool Completed => m_completed;
        [SerializeField] private BehaviourObject m_behaviourObject;
        [SerializeField] private List<ExecutionVariable> m_executionVariables = new();
        public Dictionary<BehaviourActionBuffer, List<ExecutionAction>> m_executionActions = new();
        [SerializeField] private Stack<BehaviourActionBuffer> m_executingBuffers = new();
        [SerializeField] private Stack<int> m_executingBuffersIndex = new();
        [SerializeReference] private ExecutionAction m_currentAction;
        [SerializeReference] private BehaviourActionBuffer m_currentBuffer;
        [SerializeField] private int m_executionIndex;
        [SerializeField] private bool m_completed;

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
                    m_executionIndex++;

                    if(m_executionIndex < m_executionActions[m_currentBuffer].Count)
                    {
                        m_currentAction = m_executionActions[m_currentBuffer][m_executionIndex];
                    }
                    else
                    {
                        if(m_executingBuffers.Count > 0)
                        {
                            GoToPreviousBuffer();
                        } 
                        else
                        {
                            m_completed = true;
                        }
                    }
                }
            }
        }

        public void GoToPreviousBuffer()
        {
            if(m_executingBuffers.Count > 0)
            {
                m_currentBuffer = m_executingBuffers.Pop();
                m_executionIndex = m_executingBuffersIndex.Pop();
                if(m_executionActions[m_currentBuffer].Count < m_executionIndex)
                {
                    m_currentAction = m_executionActions[m_currentBuffer][m_executionIndex];
                }
                else
                {
                    GoToPreviousBuffer();
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

        public int GetNumberOption(string numberName)
        {
            var sourceNumberOption = m_currentAction.BehaviourAction.ActionNumberOptions.FirstOrDefault(x => x.Name == numberName);
        
            if(sourceNumberOption == null)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to read number option '{numberName}' not found within action.");
                return default;
            }

            return sourceNumberOption.Value;
        }

        public T GetEnumValue<T>(string enumName)
        {
            var sourceEnum = m_currentAction.BehaviourAction.ActionEnums.FirstOrDefault(x => x.Name == enumName);
        
            if(sourceEnum == null)
            {
                Debug.LogWarning($"{m_behaviourObject.name} - Attempted to read number option '{enumName}' not found within action.");
                return default;
            }

            return (T)Enum.ToObject(typeof(T), sourceEnum.CurrentOptionIndex);
        }

        public void ExecuteActionBuffer(string bufferName)
        {
            var actionBuffer = m_currentAction.BehaviourAction.ActionBuffers.FirstOrDefault(x => x.Name == bufferName);

            if(actionBuffer.BehaviourActions.Count() <= 0) return;

            m_executingBuffers.Push(m_currentBuffer);
            m_executingBuffersIndex.Push(m_executionIndex);

            m_currentBuffer = actionBuffer;
            m_executionIndex = 0;

            m_currentAction = m_executionActions[actionBuffer][0];

            foreach(var executionAction in m_executionActions[actionBuffer])
            {
                executionAction.Reset();
            }
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

            foreach (var behaviourEvent in m_behaviourObject.BehaviourEvents)
            {
                foreach(var variable in behaviourEvent.EventVariables)
                {
                    DefineVariable(variable.Name, variable.Type);
                }
            }

            foreach(var action in m_behaviourObject.BehaviourActions)
            {
                if (!m_executionActions.ContainsKey(m_behaviourObject.BehaviourActionBuffer))
                {
                    m_executionActions[m_behaviourObject.BehaviourActionBuffer] = new();
                }

                ExecutionAction executionAction = new ExecutionAction(action);
                m_executionActions[m_behaviourObject.BehaviourActionBuffer].Add(executionAction);
                AddActionRecursive(action);
            }
        }

        public void AddActionRecursive(BehaviourAction behaviourAction)
        {
            foreach(var buffer in behaviourAction.ActionBuffers)
            {
                if (!m_executionActions.ContainsKey(buffer))
                {
                    m_executionActions[buffer] = new();
                }
                foreach(var action in buffer.BehaviourActions)
                {
                    ExecutionAction executionAction = new ExecutionAction(action);
                    m_executionActions[buffer].Add(executionAction);
                    AddActionRecursive(action);
                }
            }
        }

        public void Reset()
        {
            Reset(null);
        }

        public void Reset(Type eventType, params object[] eventParams)
        {
            m_executionIndex = 0;

            if(eventType != null)
            {
                var behaviourEvent = m_behaviourObject.BehaviourEvents.FirstOrDefault(x => x.GetType() == eventType);

                if(behaviourEvent != null)
                {
                    var eventSourceVariables = behaviourEvent.EventVariables;

                    for(int i=0; i<eventParams.Length; i++)
                    {
                        WriteVariable(eventSourceVariables[i].Name, eventSourceVariables[i].Type, eventParams[i]);
                    }
                }

            }

            foreach(var blackboard in m_behaviourObject.BehaviourBlackboards)
            {
                foreach(var variable in blackboard.BehaviourVariables)
                {
                    if (variable.VariableMode == BehaviourVariableMode.Configurable)
                    {
                        WriteVariable(variable.Name, variable.MultiTypeValue);
                    }
                    else
                    {
                        ResetVariable(variable.Name);
                    }
                }
            }

            if (m_executionActions[BehaviourObject.BehaviourActionBuffer].Count > 0)
            {
                m_currentBuffer = BehaviourObject.BehaviourActionBuffer;
                m_currentAction = m_executionActions[m_currentBuffer][0];
            }
        }
    }
}