using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using com.Sal77.GameCore;

namespace com.Sal77.BehaviourExecution
{
    [CreateAssetMenu(fileName = "New Behaviour Object", menuName = "Behaviour Object")]
    public class BehaviourObject : ScriptableObject
    {
        /*public BehaviourEvent[] BehaviourEvents => m_behaviourEvents.ToArray();
        [SerializeReference] private List<BehaviourEvent> m_behaviourEvents = new();
        public BehaviourConstant[] BehaviourConstants => m_behaviourConstants.ToArray();
        [SerializeReference] private List<BehaviourConstant> m_behaviourConstants = new();
        public BehaviourVariable[] BehaviourVariables => m_behaviourVariables.ToArray();
        [SerializeReference] private List<BehaviourVariable> m_behaviourVariables = new();
        public BehaviourBinding[] BehaviourBindings => m_behaviourBindings.ToArray();
        [SerializeReference] private List<BehaviourBinding> m_behaviourBindings = new();
        public BehaviourAction[] BehaviourActions => m_behaviourActions.ToArray();
        [SerializeReference] private List<BehaviourAction> m_behaviourActions = new();
        private BehaviourSource m_currentBehaviourSource;
        private BehaviourSourceMode m_currentBehaviourSourceMode;
        void OnEnable()
        {
            ValidateObject();
        }
        void OnValidate()
        {
            ValidateObject();
        }

        public void ValidateObject()
        {
            RemoveEmptyEntries();
            RemoveInvalidBindings();
            RemoveInvalidVariables();
            RemoveUnusedVariables();
            UpdateConstantVariables();
            UpdateNormalVariables();
            UpdateEventVariables();
            RemoveInvalidVariables();
            UpdateActionsNext();
        }
        private void OnSelectionChanged()
        {
            if(Selection.activeObject == this)
            {
                ValidateObject();
            }
        }
        public BehaviourBinding DeclareVariable<T>(string name)
        {
            return DeclareVariable(name, typeof(T));
        }
        public BehaviourBinding DeclareVariable(string name, Type type)
        {
            if(name == string.Empty || type == null) return null;

            m_currentBehaviourSource.AddVariable(name, type);

            BehaviourVariable behaviourVariable = m_behaviourVariables.FirstOrDefault(x => x.Source == m_currentBehaviourSource && x.SourceKey == name);
            BehaviourBinding variableBinding = m_behaviourBindings.FirstOrDefault(x => x.Source == m_currentBehaviourSource && x.SourceKey == name);

            if(behaviourVariable == null && variableBinding != null)
            {
                behaviourVariable = variableBinding.TargetVariable;
            }
            
            if(behaviourVariable == null)
            {
                behaviourVariable = AddVariable(name, type, m_currentBehaviourSource);
            }

            switch (m_currentBehaviourSourceMode)
            {
                case BehaviourSourceMode.Variable:

                    return null;

                case BehaviourSourceMode.Binding:

                    if(variableBinding == null)
                    {
                        variableBinding = new BehaviourBinding(type, behaviourVariable, m_currentBehaviourSource, name);
                        m_behaviourBindings.Add(variableBinding);
                    }

                    return variableBinding;
            }

            return null;
        }

        private void RemoveInvalidBindings()
        {
            List<BehaviourBinding> bindingsToRemove = new();

            foreach (var binding in m_behaviourBindings)
            {
                if(binding.Source == null)
                {
                    Debug.LogWarning($"{name} - An existing variable binding has no valid source.\n The variable binding will be removed.");
                    bindingsToRemove.Add(binding);
                    continue;
                }

                if(binding.Type == null)
                {
                    Debug.LogWarning($"{name} - Existing variable binding '{binding.Source}' has no valid type.\n The variable binding will be removed.");
                    bindingsToRemove.Add(binding);
                    continue;
                }

                var source = binding.Source;

                if(source == null)
                {
                    Debug.LogWarning($"{name} - Existing variable binding to '{binding.Source}' ({binding.Type.Name}) has no valid source.\n The variable binding will be removed.");
                    bindingsToRemove.Add(binding);
                    continue;
                }

                var sourceVariable = source.Get(binding.SourceKey);

                if(sourceVariable == null)
                {
                    Debug.LogWarning($"{name} - Existing variable binding to '{binding.Source}' ({binding.Type.Name}) has not found its key '{binding.SourceKey}' within source '{source}'.\n The variable binding will be removed.");
                    bindingsToRemove.Add(binding);
                    continue;
                }

                if(sourceVariable.Type != binding.Type)
                {
                    Debug.LogWarning($"{name} - Existing variable binding to '{binding.Source}' ({binding.Type.Name}) has found its key '{binding.SourceKey}' within source '{source}' with mismatching type '{sourceVariable.Type.Name}'.\n The variable binding will be removed.");
                    bindingsToRemove.Add(binding);
                    continue;
                }

                var targetVariable = binding.TargetVariable;

                if (targetVariable == null)
                {
                    Debug.LogWarning($"{name} - Existing variable binding to '{binding.Source}' ({binding.Type.Name}) does not have a matching behaviour variable '{binding.TargetVariable}'.\n The variable binding will be removed.");
                    bindingsToRemove.Add(binding);
                    continue;
                }
            }

            foreach (var binding in bindingsToRemove)
            {
                m_behaviourBindings.Remove(binding);
                #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
                #endif
            }
        }

        public void RemoveInvalidVariables()
        {
            List<BehaviourVariable> variablesToRemove = new();

            foreach (var variable in m_behaviourVariables)
            {
                if(variable.Name == string.Empty)
                {
                    Debug.LogWarning($"{name} - An existing behaviour variable has no valid name.\n The behaviour variable will be removed.");
                    variablesToRemove.Add(variable);
                    continue;
                }

                if(variable.Type == null)
                {
                    Debug.LogWarning($"{name} - Existing behaviour variable '{variable.Name}' has no valid type.\n The behaviour variable will be removed.");
                    variablesToRemove.Add(variable);
                    continue;
                }

                var source = variable.Source;

                if(source == null)
                {
                    Debug.LogWarning($"{name} - Existing behaviour variable '{variable.Name}' ({variable.Type.Name}) has found no source with key '{variable.SourceKey}'.\n The behaviour variable will be removed.");
                    variablesToRemove.Add(variable);
                    continue;
                }

                var sourceVariable = source.Get(variable.SourceKey);

                if(sourceVariable == null)
                {
                    Debug.LogWarning($"{name} - Existing behaviour variable '{variable.Name}' ({variable.Type.Name}) has not found its key within source '{source}' with key '{variable.SourceKey}'.\n The behaviour variable will be removed.");
                    variablesToRemove.Add(variable);
                    continue;
                }

                if(sourceVariable.Type != variable.Type)
                {
                    Debug.LogWarning($"{name} - Existing behaviour variable '{variable.Name}' ({variable.Type.Name}) has found its key '{variable.SourceKey}' within source '{source}' with mismatching type '{sourceVariable.Type.Name}'.\n The behaviour variable will be removed.");
                    variablesToRemove.Add(variable);
                    continue;
                }
            }

            foreach (var variable in variablesToRemove)
            {
                m_behaviourVariables.Remove(variable);
                #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
                #endif
            }
        }

        private void RemoveUnusedVariables()
        {
            List<BehaviourVariable> variablesToRemove = new();

            foreach (var variable in m_behaviourVariables)
            {
                if(m_behaviourEvents.Any(x => x == variable.Source)) continue;

                if(m_behaviourConstants.Any(x => x == variable.Source)) continue;

                var linkedBindingsCount = m_behaviourBindings.Count(x => x.TargetVariable == variable);

                if(linkedBindingsCount == 0)
                {
                    Debug.LogWarning($"{name} - Existing behaviour variable '{variable.Name}' ({variable.Type.Name}) has no bindings that use it.\n The behaviour variable will be removed.");
                    variablesToRemove.Add(variable);
                    continue;
                }
            }

            foreach (var variable in variablesToRemove)
            {
                m_behaviourVariables.Remove(variable);
                #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
                #endif
            }
        }

        private void UpdateNormalVariables()
        {
            foreach(var action in m_behaviourActions)
            {

                m_currentBehaviourSource = action;
                m_currentBehaviourSourceMode = BehaviourSourceMode.Binding;

                action.ClearVariables();
                action.DefineBindings(this);

                m_currentBehaviourSource = null;
            }
        }

        private void UpdateConstantVariables()
        {
            foreach(var constant in m_behaviourConstants)
            {
                m_currentBehaviourSource = constant;
                m_currentBehaviourSourceMode = BehaviourSourceMode.Variable;

                constant.ClearVariables();
                constant.DefineBindings(this);
                
                m_currentBehaviourSource = null;
            }
        }

        private void UpdateEventVariables()
        {
            foreach(var executionEvent in m_behaviourEvents)
            {
                m_currentBehaviourSource = executionEvent;
                m_currentBehaviourSourceMode = BehaviourSourceMode.Variable;

                executionEvent.ClearVariables();
                executionEvent.DefineBindings(this);

                m_currentBehaviourSource = null;
            }
        }
        private void UpdateActionsNext()
        {
            foreach(var executionAction in m_behaviourActions)
            {
                var executionActionIndex =  m_behaviourActions.IndexOf(executionAction);
                if(executionActionIndex + 1 < m_behaviourActions.Count)
                {
                    executionAction.NextAction = m_behaviourActions[executionActionIndex + 1];
                }
            }
        }
        private void RemoveEmptyEntries()
        {
            m_behaviourEvents.RemoveAll(x => x == null);
            m_behaviourConstants.RemoveAll(x => x == null);
            m_behaviourActions.RemoveAll(x => x == null);
            m_behaviourVariables.RemoveAll(x => x == null);
            m_behaviourBindings.RemoveAll(x => x == null);
        }
        public BehaviourVariable AddVariable(string name, Type type, BehaviourSource source)
        {
            var uniqueName = m_behaviourVariables.MakeUnique(x => x.Name, name);

            var variable = new BehaviourVariable(uniqueName, type, source, name);
            m_behaviourVariables.Add(variable);

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif

            return variable;
        }
        public void ChangeVariableName(BehaviourVariable targetVariable, string newName)
        {
            var bindingsToUpdate = m_behaviourBindings.Where(x => x.TargetVariable == targetVariable);

            foreach(var binding in bindingsToUpdate)
            {
                binding.TargetVariable = targetVariable;
            }
            targetVariable.Name = newName;

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
        public void ChangeConstantName(BehaviourConstant targetConstant, string newName)
        {
            var variablesToUpdate = m_behaviourVariables.Where(x => x.Source == targetConstant);
            targetConstant.Name = newName;

            UpdateConstantVariables();

            foreach(var variable in variablesToUpdate)
            {
                ChangeVariableName(variable, newName);
                variable.SourceKey = newName;
            }

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
        public void ChangeBindingTarget(BehaviourBinding behaviourBinding, BehaviourVariable targetVariable)
        {
            behaviourBinding.TargetVariable = targetVariable;
            RemoveUnusedVariables();
            RemoveInvalidVariables();
            RemoveInvalidBindings();
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
        public Type[] GetAvailableTypes()
        {
            List<Type> availableTypes = new()
            {
                typeof(int),
                typeof(float),
                typeof(string),
                typeof(bool),
                typeof(Vector3),
                typeof(Transform),
                typeof(GameObject)
            };

            foreach(var variable in m_behaviourVariables)
            {
                if (variable.Type != null && !availableTypes.Contains(variable.Type))
                {
                    availableTypes.Add(variable.Type);
                }
            }

            return availableTypes.ToArray();
        }
        public void AddAction(Type type)
        {
            if(!typeof(BehaviourAction).IsAssignableFrom(type)) throw new ArgumentException($"The type does not match '{nameof(BehaviourAction)}'");
            
            var behaviourAction = (BehaviourAction) ScriptableObject.CreateInstance(type);
            m_behaviourActions.Add(behaviourAction);
            behaviourAction.name = behaviourAction.ActionName;

            AssetDatabase.AddObjectToAsset(behaviourAction, AssetDatabase.GetAssetPath(this));

            UpdateNormalVariables();
            UpdateActionsNext();

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
        public void RemoveAction(BehaviourAction behaviourAction)
        {
            m_behaviourActions.Remove(behaviourAction);
            AssetDatabase.RemoveObjectFromAsset(behaviourAction);

            RemoveUnusedVariables();
            RemoveInvalidVariables();
            RemoveInvalidBindings();
            UpdateActionsNext();

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
        public void AddConstant(string name, Type type)
        {
            var constant = ScriptableObject.CreateInstance<BehaviourConstant>();
            constant.Name = name;
            constant.Type = type;
            m_behaviourConstants.Add(constant);
            constant.name = "Constant_"+type.ToString();

            AssetDatabase.AddObjectToAsset(constant, AssetDatabase.GetAssetPath(this));

            UpdateConstantVariables();
            UpdateNormalVariables();

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
        public void RemoveConstant(BehaviourConstant constant)
        {
            m_behaviourConstants.Remove(constant);
            AssetDatabase.RemoveObjectFromAsset(constant);

            RemoveUnusedVariables();
            RemoveInvalidVariables();
            RemoveInvalidBindings();

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
        public void AddEvent(Type type)
        {
            if(!typeof(BehaviourEvent).IsAssignableFrom(type)) throw new ArgumentException($"The type does not match '{nameof(BehaviourEvent)}'");
            
            var behaviourEvent = (BehaviourEvent) ScriptableObject.CreateInstance(type);
            m_behaviourEvents.Add(behaviourEvent);
            behaviourEvent.name = behaviourEvent.EventName;

            AssetDatabase.AddObjectToAsset(behaviourEvent, AssetDatabase.GetAssetPath(this));

            UpdateEventVariables();
            UpdateNormalVariables();

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
        public void RemoveEvent(BehaviourEvent behaviourEvent)
        {
            m_behaviourEvents.Remove(behaviourEvent);
            AssetDatabase.RemoveObjectFromAsset(behaviourEvent);

            RemoveUnusedVariables();
            RemoveInvalidVariables();
            RemoveInvalidBindings();

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        private enum BehaviourSourceMode{
            Variable,
            Binding
        }*/
    }
}
