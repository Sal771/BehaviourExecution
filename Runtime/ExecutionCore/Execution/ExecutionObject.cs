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
        public static int EndlessCycleThreshold = 2048;
        [Header("Debug")]
        [SerializeField] private BehaviourObject m_behaviourObject;
        [SerializeField] private List<ExecutionVariable> m_executionVariables = new();
        private Dictionary<BehaviourActionBuffer, List<ExecutionAction>> m_executionActions = new();
        [SerializeField] private Stack<BehaviourActionBuffer> m_executingBuffers = new();
        [SerializeField] private Stack<int> m_executingBuffersIndex = new();
        [SerializeReference] private ExecutionAction m_currentAction;
        [SerializeReference] private BehaviourActionBuffer m_currentBuffer;
        [SerializeField] private int m_executionIndex;
        private bool m_enteredBuffer;
        [SerializeField] private bool m_completed;
        private int m_endlessCycleCounter;
        #if UNITY_EDITOR
        [SerializeField] private List<DebugExecutionActionNode> m_debugExecutionActionsNodes = new();
        [SerializeField] private int m_debugExecutingBuffersCount;

        [Serializable]
        private class DebugExecutionActionNode
        {
            public string ActionBufferName;
            public List<ExecutionAction> ExecutionActions = new();
        }
        #endif

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
                RunCurrentAction();
            }
            else
            {
                m_completed = true;
            }

            m_endlessCycleCounter = 0;
            #if UNITY_EDITOR
            m_debugExecutingBuffersCount = m_executionActions.Count;
            #endif
        }

        public void RunCurrentAction()
        {
            if(m_currentAction.ExecutionResult == ExecutionActionResult.Pending)
            {
                m_executionIndex++;

                m_currentAction.Execute(this);
            }
            else
            {
                m_currentAction.Update(this);
            }

            if (m_currentAction.ExecutionResult == ExecutionActionResult.Successful)
            {
                ChooseNextAction();

                if(m_currentAction != null)
                {
                    if (m_endlessCycleCounter >= EndlessCycleThreshold)
                    {
                        Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Action exceeded actions to change amount of ({EndlessCycleThreshold} in a frame, Possible infinite loop detected.) .");
                        return;
                    }
                    m_endlessCycleCounter++;
                    RunCurrentAction();
                }
            }
        }

        public void ChooseNextAction()
        {
            if(m_executionIndex < m_executionActions[m_currentBuffer].Count)
            {
                m_currentAction = m_executionActions[m_currentBuffer][m_executionIndex];
            }
            else
            {

                if(m_executingBuffers.Count > 0)
                {
                    GoToPreviousBuffer();
                } else
                {
                    m_currentAction = null;
                }
            }
        }

        public void GoToPreviousBuffer()
        {
            if(m_executingBuffers.Count > 0)
            {
                m_currentBuffer = m_executingBuffers.Pop();
                m_executionIndex = m_executingBuffersIndex.Pop();

                if(m_executionIndex < m_executionActions[m_currentBuffer].Count)
                {
                    m_currentAction = m_executionActions[m_currentBuffer][m_executionIndex];

                    for(int i = m_executionIndex; i<m_executionActions[m_currentBuffer].Count; i++)
                    {
                        m_executionActions[m_currentBuffer][i].Reset();
                    }
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

        private void InitializeVariable(string variableName, MultiTypeObject multiTypeObject)
        {
           var variable = m_executionVariables.FirstOrDefault(x => x.Name == variableName);

            if(variable == null)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to initialize missing variable '{variableName}'.");
            }

            if(variable.Type != multiTypeObject.Type)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to initialize variable '{variableName}' with mismatching type.");
            }

            variable.Set(multiTypeObject);
        }

        private void InitializeVariable(string variableName, Type type, object value)
        {
           var variable = m_executionVariables.FirstOrDefault(x => x.Name == variableName);

            if(variable == null)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to initialize missing variable '{variableName}'.");
            }

            if(variable.Type != type)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to initialize variable '{variableName}' with mismatching type.");
            }

            variable.Set(value, type);
        }

        public ExecutionVariable GetVariable(string variableName)
        {
            var sourceVariable = m_currentAction.BehaviourAction.ActionVariables.FirstOrDefault(x => x.Name == variableName);
            
            ExecutionVariable variable = null;

            if(sourceVariable != null)
            {
                variable = m_executionVariables.FirstOrDefault(x => x.Name == sourceVariable.TargetVariableName);
            }

            if(variable == null)
            {
                string alternateName = m_currentAction.BehaviourAction.ActionGuid+"_"+variableName;
                variable = m_executionVariables.FirstOrDefault(x => x.Name == alternateName);
            }

            if(variable == null)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Failed to resolve variable '{variableName}'.");
            }

            return variable;
        }

        public void WriteVariable(string variableName, Type type, object value)
        {
            var variable = GetVariable(variableName);

            if(variable == null)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to write missing variable '{variableName}'");
                return;
            }

            if(variable.Type != type)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to write variable '{variableName}' found with mismatching type");
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
            var variable = GetVariable(variableName);

            if(variable == null)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to write missing variable '{variable.Name}'.");
                return;
            }

            if(variable.Type != source.Type)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to write variable '{variable.Name}' found with mismatching type.");
                return;
            }

            variable.Set(source);
        }

        public T ReadVariable<T>(string variableName)
        {
            var variable = GetVariable(variableName);

            if(variable == null)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to read missing variable '{variableName}'.");
                return default;
            }

            if(variable.Type != typeof(T))
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to read variable '{variable.Name}' found with mismatching type");
                return default;
            }

            return variable.Get<T>();
        }

        public int GetNumberOption(string numberName)
        {
            var sourceNumberOption = m_currentAction.BehaviourAction.ActionNumberOptions.FirstOrDefault(x => x.Name == numberName);
        
            if(sourceNumberOption == null)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to read number option '{numberName}' not found within action.");
                return default;
            }

            return sourceNumberOption.Value;
        }

        public T GetEnumValue<T>(string enumName)
        {
            var sourceEnum = m_currentAction.BehaviourAction.ActionEnums.FirstOrDefault(x => x.Name == enumName);
        
            if(sourceEnum == null)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to read number option '{enumName}' not found within action.");
                return default;
            }

            return (T)Enum.ToObject(typeof(T), sourceEnum.CurrentOptionIndex);
        }

        public void ExecuteActionBuffer(string bufferName)
        {
            var actionBuffer = m_currentAction.BehaviourAction.ActionBuffers.FirstOrDefault(x => x.Name == bufferName);

            if(actionBuffer == null)
            {
                Debug.LogWarning($"[Execution Object] ({m_behaviourObject.name}) - Attempted to execute Action buffer '{bufferName}' not found within action.");
                return;
            }

            if(actionBuffer.BehaviourActions.Length <= 0) return;

            m_enteredBuffer = true;

            m_executingBuffers.Push(m_currentBuffer);
            m_executingBuffersIndex.Push(m_executionIndex);

            m_currentBuffer = actionBuffer;
            m_executionIndex = 0;

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

                AddActionRecursive(action, m_behaviourObject.BehaviourActionBuffer);
            }
        }

        public void AddActionRecursive(BehaviourAction behaviourAction, BehaviourActionBuffer actionBuffer)
        {
            ExecutionAction executionAction = new ExecutionAction(behaviourAction);
            m_executionActions[actionBuffer].Add(executionAction);

            #if UNITY_EDITOR
            if(!m_debugExecutionActionsNodes.Any(x => x.ActionBufferName == actionBuffer.Name))
            {
                DebugExecutionActionNode node = new();
                node.ActionBufferName = actionBuffer.Name;
                m_debugExecutionActionsNodes.Add(node);
            }

            m_debugExecutionActionsNodes.First(x => x.ActionBufferName == actionBuffer.Name).ExecutionActions.Add(executionAction);
            #endif

            foreach(var variable in behaviourAction.ActionVariables)
            {
                if(variable.TargetVariableName != string.Empty)
                {
                    bool notPresentInBlackboards = !m_behaviourObject.BehaviourBlackboards.Any(x => x.BehaviourVariables.Any(y => y.Name == variable.TargetVariableName));
                    if(notPresentInBlackboards){
                        Debug.LogWarning($"[EXECUTION OBJECT] ({m_behaviourObject.name}) - Did not find the target variable '{variable.TargetVariableName}' within the behaviour blackboards. Can't create the execution variable.");
                        continue;
                    }
                    DefineVariable(variable.TargetVariableName, variable.VariableType);
                }
                else
                {
                    DefineVariable(behaviourAction.ActionGuid+"_"+variable.Name, variable.VariableType);
                }
            }

            foreach(var buffer in behaviourAction.ActionBuffers)
            {
                if (!m_executionActions.ContainsKey(buffer))
                {
                    m_executionActions[buffer] = new();
                }
                foreach(var action in buffer.BehaviourActions)
                {
                    AddActionRecursive(action, buffer);
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
                        InitializeVariable(eventSourceVariables[i].Name, eventSourceVariables[i].Type, eventParams[i]);
                    }
                }

            }

            foreach(var blackboard in m_behaviourObject.BehaviourBlackboards)
            {
                foreach(var variable in blackboard.BehaviourVariables)
                {
                    if (variable.VariableMode == BehaviourVariableMode.Configurable)
                    {
                        InitializeVariable(variable.Name, variable.MultiTypeValue);
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