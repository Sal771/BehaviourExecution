using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace com.Sal77.BehaviourExecution
{
    [Serializable]
    public abstract class BehaviourAction : ScriptableObject, IBehaviourAction
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
        public BehaviourActionVariable[] ActionVariables => m_actionVariables.ToArray();
        public BehaviourActionEnum[] ActionEnums => m_actionEnums.ToArray();
        public BehaviourActionBuffer[] ActionBuffers => m_actionBuffers.ToArray();
        public BehaviourNumberOption[] ActionNumberOptions => m_numberOptions.ToArray();
        public string ActionName => GetActionName();
        public string ActionGuid => m_actionGuid;

        [SerializeField] private BehaviourAction m_nextAction;
        [SerializeField] private List<BehaviourActionVariable> m_actionVariables = new();
        [SerializeField] private List<BehaviourActionEnum> m_actionEnums = new();
        [SerializeField] private List<BehaviourActionBuffer> m_actionBuffers = new();
        [SerializeField] private List<BehaviourNumberOption> m_numberOptions = new();
        [SerializeField] private string m_actionGuid;
        
        private Dictionary<string, string> m_targetVariableCache = new();
        private Dictionary<string, int> m_enumIndexCache = new();
        private Dictionary<string, BehaviourAction[]> m_actionBufferCache = new();
        private Dictionary<string, int> m_numberOptionCache = new();
        private int m_actionFieldsCount;
        public BehaviourAction()
        {
            m_actionGuid = Guid.NewGuid().ToString();
        }

        protected abstract string GetActionName();
        public abstract ExecutionActionResult Execute(IBehaviourExecution executionContext);
        public virtual bool WaitCondition(IBehaviourExecution executionContext){ return true; }

        public void UpdateBindings()
        {
            m_actionVariables.Clear();
            m_enumIndexCache.Clear();
            m_actionBufferCache.Clear();
            m_numberOptionCache.Clear();
            m_actionFieldsCount = 0;

            foreach(var variable in m_actionVariables){
                if(variable == null) continue;
                if(variable.Name == String.Empty) continue;

                m_targetVariableCache.Add(variable.Name, variable.TargetVariableName);
            }
            foreach(var actionEnum in m_actionEnums){
                if(actionEnum == null) continue;
                if(actionEnum.Name == String.Empty) continue;

                m_enumIndexCache.Add(actionEnum.Name, actionEnum.CurrentOptionIndex);
            }
            foreach(var actionBuffer in m_actionBuffers){
                if(actionBuffer == null) continue;
                if(actionBuffer.Name == String.Empty) continue;

                m_actionBufferCache.Add(actionBuffer.Name, actionBuffer.BehaviourActions);
            }
            foreach(var numberOption in m_numberOptions){
                if(numberOption == null) continue;
                if(numberOption.Name == String.Empty) continue;

                m_numberOptionCache.Add(numberOption.Name, numberOption.Value);
            }

            m_actionVariables.Clear();
            m_actionEnums.Clear();
            m_actionBuffers.Clear();
            m_numberOptions.Clear();

            DefineBindings(this);
        }
        protected abstract void DefineBindings(IBehaviourAction actionContext);
        public void DeclareVariable<T>(string name)
        {
            var behaviourActionVariable = new BehaviourActionVariable(name, typeof(T));
            behaviourActionVariable.OrderIndex = m_actionFieldsCount;

            if(m_targetVariableCache.TryGetValue(name, out string targetVariableName))
            {
                behaviourActionVariable.TargetVariableName = targetVariableName;
            }
            m_actionVariables.Add(behaviourActionVariable);

            m_actionFieldsCount++;
        }

        public int DeclareEnum<T>(string name)
        {
            var behaviourEnum = new BehaviourActionEnum(name, typeof(T));
            behaviourEnum.OrderIndex = m_actionFieldsCount;


            int returnIndex = 0;

            if(m_enumIndexCache.TryGetValue(name, out int index))
            {
                behaviourEnum.CurrentOptionIndex = index;
                returnIndex = index;
            }
            m_actionEnums.Add(behaviourEnum);

            m_actionFieldsCount++;

            return returnIndex;
        }

        public void DeclareActionBuffer(string name)
        {
            var behaviourActionBuffer = new BehaviourActionBuffer(name);
            behaviourActionBuffer.OrderIndex = m_actionFieldsCount;

            if(m_actionBufferCache.TryGetValue(name, out BehaviourAction[] actions))
            {
                behaviourActionBuffer.AddRange(actions);
            }
            m_actionBuffers.Add(behaviourActionBuffer);

            m_actionFieldsCount++;
        }

        public int DeclareNumberOption(string name)
        {
            var behaviourNumberOption = new BehaviourNumberOption(name);
            behaviourNumberOption.OrderIndex = m_actionFieldsCount;

            int returnValue = 0;

            if(m_numberOptionCache.TryGetValue(name, out int currentValue))
            {
                behaviourNumberOption.Value = currentValue;
                returnValue = currentValue;
            }
            m_numberOptions.Add(behaviourNumberOption);

            m_actionFieldsCount++;

            return returnValue;
        }
    }
}
