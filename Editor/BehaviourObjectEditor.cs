#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using com.Sal77.BehaviourExecution;
using com.Sal77.GameCore;
using System.IO;

[CustomEditor(typeof(BehaviourObject))]
public class BehaviourObjectEditor : Editor
{
    private BehaviourObject m_behaviourObject;
    private ShowType m_showType = ShowType.Variable;
    private int m_eventsPopupIndex = 0;
    private int m_actionsPopupIndex = 0;
    private int m_typePopupIndex = 0;
    private int m_constantSortOptionIndex = 0;
    private int m_variableSortOptionIndex = 0;
    private int m_actionRetargetOptionIndex = 0;
    private string m_creatingConstantName;
    private BehaviourVariable m_renamingVariable;
    private BehaviourBinding m_retargetingBinding;
    private bool m_baseEditorToggle = false;


    void OnEnable()
    {
        m_behaviourObject = (BehaviourObject)target;
    }
    public override void OnInspectorGUI()
    {
        DrawStartSection();
        EditorGUILayout.Space(30);

        if (m_showType == ShowType.Variable)
        {
            DrawEventsSection();
            EditorGUILayout.Space(30);
            DrawConstantsSection();
            EditorGUILayout.Space(30);
            DrawVariablesSection();
        } else
        {
            DrawActionsSection();
        }
        
        AssetDatabase.SaveAssetIfDirty(m_behaviourObject);

        EditorGUILayout.Space(60);

        m_baseEditorToggle = EditorGUILayout.Foldout(m_baseEditorToggle,"Base Editor (For Debug)");

        if(m_baseEditorToggle)
        {
            base.OnInspectorGUI();
        }
    }

    private void DrawStartSection()
    {
        GUIStyle titleStyle = new ();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 18;
        titleStyle.fixedHeight = 18;
        titleStyle.normal = new GUIStyleState()
        {
            textColor = Color.white
        };

        EditorGUILayout.LabelField(m_behaviourObject.name, titleStyle);

        EditorGUILayout.Space(15);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Variables", GUILayout.Width(120)))
        {
            m_showType = ShowType.Variable;
        }

        if (GUILayout.Button("Actions", GUILayout.Width(120)))
        {
            m_showType = ShowType.Actions;
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawEventsSection()
    {
        EditorGUILayout.LabelField("Events");

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        var eventsCategorized = BehaviourUtility.AllAvailableEventsCategorized();

        m_eventsPopupIndex = EditorGUILayout.Popup(m_eventsPopupIndex ,eventsCategorized, GUILayout.Width(120));

        if (GUILayout.Button("+", GUILayout.Width(30)) && eventsCategorized.Length > 0)
        {
            var selectedEventCategorized = eventsCategorized[m_eventsPopupIndex];
            var selectedEventName = BehaviourUtility.EventNameFromCategorized(selectedEventCategorized);

            var newEvent = BehaviourUtility.EventTypeFromName(selectedEventName);
            m_behaviourObject.AddEvent(newEvent);
        }

        EditorGUILayout.Space();

        EditorGUILayout.EndHorizontal();

        List<BehaviourEvent> eventsToRemove = new();

        foreach(var behaviourEvent in m_behaviourObject.BehaviourEvents){
            EditorGUILayout.BeginVertical("GroupBox");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(behaviourEvent.EventName);
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                eventsToRemove.Add(behaviourEvent);
            }

            EditorGUILayout.EndHorizontal();

            foreach(var behaviourField in behaviourEvent.SourceVariables)
            {
                var behaviourVariable = m_behaviourObject.BehaviourVariables.FirstOrDefault(x => x.Source == behaviourEvent && x.SourceKey == behaviourField.Name);
                if(behaviourVariable == null) continue;

                EditorGUILayout.BeginHorizontal("GroupBox");

                if (m_renamingVariable != behaviourVariable)
                {
                    EditorGUILayout.LabelField($"{behaviourField.Name} ({behaviourField.Type.Name}) - {behaviourVariable.Name}" , GUILayout.Width(240));
                    if (GUILayout.Button("Rename", GUILayout.Width(60)))
                    {
                        m_renamingVariable = behaviourVariable;
                    }
                }
                else
                {
                    var commitText = EditorGUILayout.DelayedTextField("", behaviourVariable.Name, GUILayout.Width(240));
                    if (GUILayout.Button("Rename", GUILayout.Width(60)))
                    {
                       m_renamingVariable = null;
                    }
                    
                    if(commitText != behaviourVariable.Name)
                    {
                        commitText = m_behaviourObject.BehaviourVariables.MakeUnique(x => x.Name, commitText);
                        m_behaviourObject.ChangeVariableName(behaviourVariable, commitText);
                        m_renamingVariable = null;
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        foreach(var eventToRemove in eventsToRemove)
        {
            m_behaviourObject.RemoveEvent(eventToRemove);
        }
    }

    private void DrawConstantsSection()
    {
        EditorGUILayout.LabelField("Constants");

        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("Available types may show more options depending on existing events, actions or constants.\n If the desired type is not shown, add a source that uses that type.", MessageType.Info);

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        var availableTypes = m_behaviourObject.GetAvailableTypes();
        var availableTypeNames = availableTypes.Select(x => x.Name).ToArray();

        m_typePopupIndex = Mathf.Clamp(m_typePopupIndex, 0, availableTypeNames.Length);
        m_typePopupIndex = EditorGUILayout.Popup(m_typePopupIndex ,availableTypeNames, GUILayout.Width(120));

        EditorGUILayout.LabelField("Name", GUILayout.Width(45));
        m_creatingConstantName = EditorGUILayout.TextField(m_creatingConstantName, GUILayout.Width(120));

        m_creatingConstantName = m_behaviourObject.BehaviourVariables.MakeUnique(x => x.Name, m_creatingConstantName);

        if (GUILayout.Button("+", GUILayout.Width(30)) && availableTypeNames.Length > 0)
        {
            m_behaviourObject.AddConstant(m_creatingConstantName, availableTypes[m_typePopupIndex]);
        }

        EditorGUILayout.Space();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(15);

        EditorGUILayout.BeginHorizontal();

        List<string> sortOptions = new(){"All"};
        sortOptions.AddRange(availableTypeNames);

        var sortOptionsArray = sortOptions.ToArray();

        EditorGUILayout.LabelField("Constant sort option:", GUILayout.Width(120));
        m_constantSortOptionIndex = Mathf.Clamp(m_constantSortOptionIndex, 0, sortOptionsArray.Length);
        m_constantSortOptionIndex = EditorGUILayout.Popup(m_constantSortOptionIndex, sortOptionsArray);

        Type sortType = null;
        if(m_constantSortOptionIndex != 0)
        {
            sortType = availableTypes[m_constantSortOptionIndex-1];
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        BehaviourConstant constantToRemove = null;

        EditorGUILayout.BeginVertical();

        foreach(var behaviourConstant in m_behaviourObject.BehaviourConstants)
        {
            if(sortType != null && behaviourConstant.Type != sortType) continue;

            var behaviourVariable = m_behaviourObject.BehaviourVariables.FirstOrDefault(x => x.Source == behaviourConstant);
            if(behaviourVariable == null) continue;

            EditorGUILayout.BeginHorizontal("GroupBox");

            if (m_renamingVariable != behaviourVariable)
            {
                EditorGUILayout.LabelField($"{behaviourConstant.Name} ({behaviourConstant.Type.Name})" , GUILayout.Width(120));
                behaviourConstant.MultiTypeValue.DrawField();
                if (GUILayout.Button("Rename", GUILayout.Width(60)))
                {
                    m_renamingVariable = behaviourVariable;
                }
            }
            else
            {
                var commitText = EditorGUILayout.DelayedTextField("", behaviourVariable.Name, GUILayout.Width(240));
                if (GUILayout.Button("Rename", GUILayout.Width(60)))
                {
                   m_renamingVariable = null;
                }
                
                if(commitText != behaviourVariable.Name)
                {
                    commitText = m_behaviourObject.BehaviourVariables.MakeUnique(x => x.Name, commitText);
                    m_behaviourObject.ChangeConstantName(behaviourConstant, commitText);
                    m_renamingVariable = null;
                }
            }

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                constantToRemove = behaviourConstant;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

        if(constantToRemove != null)
        {
            m_behaviourObject.RemoveConstant(constantToRemove);
        }
    }

    private void DrawVariablesSection()
    {
        EditorGUILayout.LabelField("Variables");

        EditorGUILayout.Space();

        var availableTypes = m_behaviourObject.GetAvailableTypes();
        var availableTypeNames = availableTypes.Select(x => x.Name).ToArray();

        EditorGUILayout.BeginHorizontal();

        List<string> sortOptions = new(){"All"};
        sortOptions.AddRange(availableTypeNames);

        var sortOptionsArray = sortOptions.ToArray();

        EditorGUILayout.LabelField("Variable sort option:", GUILayout.Width(120));
        m_variableSortOptionIndex = Mathf.Clamp(m_variableSortOptionIndex, 0, sortOptionsArray.Length);
        m_variableSortOptionIndex = EditorGUILayout.Popup(m_variableSortOptionIndex, sortOptionsArray);

        Type sortType = null;
        if(m_variableSortOptionIndex != 0)
        {
            sortType = availableTypes[m_variableSortOptionIndex-1];
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical();

        var eventVariables = m_behaviourObject.BehaviourVariables.Where(x => m_behaviourObject.BehaviourEvents.Any(y => x.Source == y));
        var constantVariables = m_behaviourObject.BehaviourVariables.Where(x => m_behaviourObject.BehaviourEvents.Any(y => x.Source == y));

        var actionVariables = m_behaviourObject.BehaviourVariables.Where(x => !eventVariables.Contains(x) && !constantVariables.Contains(x));

        foreach(var behaviourVariable in actionVariables)
        {
            if(sortType != null && behaviourVariable.Type != sortType) continue;

            var behaviourAction = m_behaviourObject.BehaviourActions.FirstOrDefault(x => x == behaviourVariable.Source);

            if(behaviourAction == null) continue;

            EditorGUILayout.BeginHorizontal("GroupBox");

            if (m_renamingVariable != behaviourVariable)
            {
                EditorGUILayout.LabelField($"{behaviourVariable.Name} ({behaviourVariable.Type.Name})" , GUILayout.Width(240));
                if (GUILayout.Button("Rename", GUILayout.Width(60)))
                {
                    m_renamingVariable = behaviourVariable;
                }
            }
            else
            {
                var commitText = EditorGUILayout.DelayedTextField("", behaviourVariable.Name, GUILayout.Width(240));
                if (GUILayout.Button("Rename", GUILayout.Width(60)))
                {
                   m_renamingVariable = null;
                }
                
                if(commitText != behaviourVariable.Name)
                {
                    commitText = m_behaviourObject.BehaviourVariables.MakeUnique(x => x.Name, commitText);
                    m_behaviourObject.ChangeVariableName(behaviourVariable, commitText);
                    m_renamingVariable = null;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawActionsSection()
    {
        EditorGUILayout.LabelField("Actions");

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        var actionsCategorized = BehaviourUtility.AllAvailableActionsCategorized();

        m_actionsPopupIndex = EditorGUILayout.Popup(m_actionsPopupIndex ,actionsCategorized, GUILayout.Width(120));

        if (GUILayout.Button("+", GUILayout.Width(30)) && actionsCategorized.Length > 0)
        {
            var selectedActionCategorized = actionsCategorized[m_actionsPopupIndex];
            var selectedActionName = BehaviourUtility.ActionNameFromCategorized(selectedActionCategorized);

            var newActionType = BehaviourUtility.ActionTypeFromName(selectedActionName);
            m_behaviourObject.AddAction(newActionType);
        }

        EditorGUILayout.Space();

        EditorGUILayout.EndHorizontal();

        BehaviourAction actionToRemove = null;

        foreach(var behaviourAction in m_behaviourObject.BehaviourActions){
            EditorGUILayout.BeginVertical("GroupBox");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(behaviourAction.ActionName);
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                actionToRemove = behaviourAction;
            }

            EditorGUILayout.EndHorizontal();

            var beehaviourBindings = m_behaviourObject.BehaviourBindings.Where(x => x.Source == behaviourAction);

            foreach(var behaviourBinding in beehaviourBindings)
            {
                var behaviourVariable = m_behaviourObject.BehaviourVariables.FirstOrDefault(x => x == behaviourBinding.TargetVariable);
                if(behaviourVariable == null) continue;

                EditorGUILayout.BeginHorizontal("GroupBox");

                if (m_retargetingBinding != behaviourBinding)
                {
                    EditorGUILayout.LabelField($"{behaviourVariable.SourceKey} ({behaviourVariable.Type.Name}) - {behaviourVariable.Name}" , GUILayout.Width(240));
                    if (GUILayout.Button("Retarget", GUILayout.Width(60)))
                    {
                        m_retargetingBinding = behaviourBinding;
                    }
                }
                else
                {
                    var availableVariables = m_behaviourObject.BehaviourVariables.Where(x => x.Type == behaviourBinding.Type).ToArray();
                    var availableVariableNames = new List<string>();
                    availableVariableNames.AddRange(availableVariables.Select(x => x.Name));
                    availableVariableNames.Add("New");
                    var availableVariableNamesArray = availableVariableNames.ToArray();

                    m_actionRetargetOptionIndex = Mathf.Clamp(m_actionRetargetOptionIndex, 0, availableVariableNamesArray.Length);
                    m_actionRetargetOptionIndex = EditorGUILayout.Popup(m_actionRetargetOptionIndex, availableVariableNamesArray, GUILayout.Width(240));
                    if (GUILayout.Button("Retarget", GUILayout.Width(60)))
                    {
                        BehaviourVariable targetVariable;
                        if(m_actionRetargetOptionIndex == availableVariables.Length)
                        {
                            targetVariable = m_behaviourObject.AddVariable(behaviourBinding.SourceKey, behaviourBinding.Type, behaviourAction);
                        } else
                        {
                            targetVariable = availableVariables[m_actionRetargetOptionIndex];
                        }
                        m_behaviourObject.ChangeBindingTarget(behaviourBinding, targetVariable);
                        m_retargetingBinding = null;
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        if(actionToRemove != null)
        {
            m_behaviourObject.RemoveAction(actionToRemove);
        }
    }

    private enum ShowType
    {
        Variable,
        Actions,
    }
}

#endif