using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using com.Sal77.BehaviourExecution;
using System.IO;

//TODO variable renaming
//TODO Drag n Scroll
//TODO Section title

[CustomEditor(typeof(BehaviourObject))]
public class BehaviourObjectEditor : Editor
{
    private bool m_instructionsFoldout;
    private int m_selectedActionIndex;
    private List<BehaviourAction> m_selectedActions = new();
    private BehaviourExecutionThemeConfig m_themeConfig;

    public class BehaviourActionDrag
    {
        public BehaviourAction behaviourAction;
    }

    public override void OnInspectorGUI()
    {
        if(m_themeConfig == null)
        {
            m_themeConfig = BehaviourEditorUtility.GetThemeConfig();
        }

        BehaviourObject behaviourObject = (BehaviourObject)target;
        BehaviourActionBuffer behaviourActionBuffer = behaviourObject.BehaviourActionBuffer;

        //Instructions Box

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.InstructionStyle);

        GUI.backgroundColor = m_themeConfig.Details1Color;
        m_instructionsFoldout = EditorGUILayout.Foldout(m_instructionsFoldout, "Instructions");

        if (m_instructionsFoldout)
        {
            EditorGUILayout.LabelField(
                "Expand width to read fully.\n\n"
                +"BLACKBOARDS.\n\n"
                +"EVENTS.\n\n"
                +"ACTIONS.\n"
            , GUILayout.Height(260));
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        //Blackboard Box

        //Header
        GUI.backgroundColor = m_themeConfig.Background2Color;
        EditorGUILayout.BeginVertical(m_themeConfig.HeaderStyle);

        EditorGUILayout.LabelField("Blackboards", m_themeConfig.HeaderStyle);

        EditorGUILayout.EndVertical();

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.Background1Style, GUILayout.MinHeight(120));
        EditorGUILayout.LabelField("");
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        //Events Box

        //Header
        GUI.backgroundColor = m_themeConfig.Background2Color;
        EditorGUILayout.BeginVertical(m_themeConfig.HeaderStyle);

        EditorGUILayout.LabelField("Events", m_themeConfig.HeaderStyle);

        EditorGUILayout.EndVertical();

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.Background1Style, GUILayout.MinHeight(120));
        EditorGUILayout.LabelField("");
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        //Actions Box

        //Header
        GUI.backgroundColor = m_themeConfig.Background2Color;
        EditorGUILayout.BeginVertical(m_themeConfig.HeaderStyle);

        EditorGUILayout.LabelField("Actions", m_themeConfig.HeaderStyle);

        EditorGUILayout.EndVertical();

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.Background1Style, GUILayout.MinHeight(120));

        EditorGUILayout.BeginHorizontal();

        GUI.backgroundColor = m_themeConfig.Button1Color;
        string[] actionscategorized = BehaviourUtility.AllAvailableActionsCategorized();
        m_selectedActionIndex = EditorGUILayout.Popup(m_selectedActionIndex, actionscategorized);

        if (GUILayout.Button("+"))
        {
            Type actionType = BehaviourUtility.ActionTypeFromCategorized(actionscategorized[m_selectedActionIndex]);
            BehaviourAction action = (BehaviourAction)ScriptableObject.CreateInstance(actionType);
            action.name = actionType.Name + "_" + Guid.NewGuid();

            AssetDatabase.AddObjectToAsset(action, behaviourObject);

            behaviourActionBuffer.Add(action);

            EditorUtility.SetDirty(behaviourObject);
            AssetDatabase.SaveAssets();
        }

        GUI.enabled = false;

        if (GUILayout.Button("C"))
        {
            //Copy
        }

        if (GUILayout.Button("P"))
        {
            //Paste
        }

        GUI.enabled = true;

        EditorGUILayout.EndHorizontal();

        DrawBehaviourActionBuffer(behaviourObject, behaviourActionBuffer);

        EditorGUILayout.EndVertical();

        //Drag Exit
        if (Event.current.type == EventType.DragExited)
        {
            DragAndDrop.SetGenericData("Behaviour Action Drag", null);
            this.Repaint();
        }
    }

    private void DrawBehaviourActionBuffer(BehaviourObject behaviourObject, BehaviourActionBuffer behaviourActionBuffer)
    {
        Rect lastRect = GUILayoutUtility.GetLastRect(); ;
        Rect indicatorRect;

        BehaviourActionDrag currentActionDrag = (BehaviourActionDrag)DragAndDrop.GetGenericData("Behaviour Action Drag");

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.Background1Style);

        var actions = behaviourObject.BehaviourActions;
        for (int i = 0; i < actions.Length; i++)
        {
            var behaviourAction = actions[i];

            if(currentActionDrag == null || currentActionDrag.behaviourAction != behaviourAction)
            {
                GUI.backgroundColor = m_themeConfig.Background2Color;
            }
            else
            {
                GUI.backgroundColor = m_themeConfig.Background2Color * 0.5f + m_themeConfig.ActionDragTint * 0.5f;
            }
            
            EditorGUILayout.BeginVertical(m_themeConfig.Background2Style);

            EditorGUILayout.BeginHorizontal();
            
            GUI.backgroundColor = m_themeConfig.ActionNameBackgroundColor;

            EditorGUILayout.LabelField(behaviourAction.ActionName, m_themeConfig.ActionNameStyle);

            GUI.backgroundColor = m_themeConfig.Button1Color;

            if (GUILayout.Button("X", GUILayout.Width(40)))
            {
                int removeIndex = i;
                UnityEngine.Object actionToRemove = behaviourAction;

                behaviourActionBuffer.RemoveAt(removeIndex);

                DestroyImmediate(actionToRemove, true);

                EditorUtility.SetDirty(behaviourObject);
                AssetDatabase.SaveAssets();

                break;
            }

            EditorGUILayout.EndHorizontal();

            DrawBehaviourActionBufferFields(behaviourAction);

            EditorGUILayout.EndVertical();

            if (BehaviourEditorUtility.LastRectClicked())
            {
                DragAndDrop.SetGenericData("Behaviour Action Drag", null);
                this.Repaint();
            }

            if (BehaviourEditorUtility.LastRectDragStart())
            {
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.StartDrag("Behaviour Action Drag");

                BehaviourActionDrag actionDrag = new();
                actionDrag.behaviourAction = behaviourAction;

                DragAndDrop.SetGenericData("Behaviour Action Drag", actionDrag);
                this.Repaint();
            }

            //Before Indicator

            lastRect = GUILayoutUtility.GetLastRect();

            if(currentActionDrag != null)
            {
                indicatorRect = new Rect(lastRect.x, lastRect.y - 2, lastRect.width, 4);
                var rectHitbox = new Rect(indicatorRect.x, indicatorRect.y-16, indicatorRect.width, 32);

                if(rectHitbox.Contains(Event.current.mousePosition)) DrawIndicator(indicatorRect);

                if (BehaviourEditorUtility.DropArea(rectHitbox, DragAndDropVisualMode.Move, null))
                {
                    int currentIndex = System.Array.IndexOf(behaviourObject.BehaviourActions, currentActionDrag.behaviourAction);
                    int targetIndex;
                    if (currentIndex < i)
                    {
                        targetIndex = i - 1;
                    }
                    else
                    {
                        targetIndex = i;
                    }

                    behaviourActionBuffer.InsertTo(currentActionDrag.behaviourAction, targetIndex);

                    EditorUtility.SetDirty(behaviourObject);
                    AssetDatabase.SaveAssets();

                    DragAndDrop.SetGenericData("Behaviour Action Drag", null);
                    this.Repaint();
                }
            }
        }

        if(currentActionDrag != null)
        {
            indicatorRect = new Rect(lastRect.x, lastRect.y + lastRect.height + 2, lastRect.width, 4);
            var lastRectHitbox = new Rect(indicatorRect.x, indicatorRect.y-16, indicatorRect.width, 32);

            if(lastRectHitbox.Contains(Event.current.mousePosition)) DrawIndicator(indicatorRect);

            if (BehaviourEditorUtility.DropArea(lastRectHitbox, DragAndDropVisualMode.Move, null))
            {
                behaviourActionBuffer.InsertTo(currentActionDrag.behaviourAction, behaviourObject.BehaviourActions.Length);

                EditorUtility.SetDirty(behaviourObject);
                AssetDatabase.SaveAssets();

                DragAndDrop.SetGenericData("Behaviour Action Drag", null);
                this.Repaint();
            }
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawBehaviourActionBufferFields(BehaviourAction action)
    {
        action.UpdateBindings();

        var allFields = new List<BehaviourActionField>();
        
        foreach (var variable in action.ActionVariables)
        {
            allFields.Add(variable);
        }
        
        foreach (var enumField in action.ActionEnums)
        {
            allFields.Add(enumField);
        }
        
        foreach (var buffer in action.ActionBuffers)
        {
            allFields.Add(buffer);
        }
        
        foreach (var numberOption in action.ActionNumberOptions)
        {
            allFields.Add(numberOption);
        }
        
        var sortedFields = allFields.OrderBy(f => f.OrderIndex).ToList();
        
        EditorGUILayout.BeginHorizontal();
        int count = 0;
        Type previousActionType = null;
        
        foreach (var field in sortedFields)
        {
            var actionType = GetBehaviourActionFieldType(field);

            // New row every 2 fields
            if (count == 2 || previousActionType != actionType)
            {
                count = 0;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }

            EditorGUILayout.BeginHorizontal();

            switch(field)
            {
                case BehaviourActionVariable actionVariable:
                    DrawActionVariable(actionVariable);
                    break;

                case BehaviourActionEnum actionEnum:
                    DrawActionEnum(actionEnum);
                    break;

                case BehaviourActionBuffer actionBuffer:
                    DrawActionBuffer(actionBuffer);
                    break;

                case BehaviourNumberOption numberOption:
                    DrawNumberOption(numberOption);
                    break;
            };

            EditorGUILayout.EndHorizontal();

            count++;

            previousActionType = actionType;
        }

        EditorGUILayout.EndHorizontal();
    }

    private Type GetBehaviourActionFieldType(BehaviourActionField actionField)
    {
        switch(actionField)
        {
            case BehaviourActionVariable:
                return typeof(BehaviourActionVariable);

            case BehaviourActionEnum:
                return typeof(BehaviourActionEnum);

            case BehaviourActionBuffer:
                return typeof(BehaviourActionBuffer);

            case BehaviourNumberOption:
                return typeof(BehaviourNumberOption);
        };

        return null;
    }

    private void DrawActionVariable(BehaviourActionVariable actionVariable)
    {
        GUI.backgroundColor = m_themeConfig.ActionFieldVariableBackgroundColor;
        var width = EditorGUIUtility.currentViewWidth * m_themeConfig.FieldWidthRatio;
        EditorGUILayout.BeginHorizontal(m_themeConfig.ActionFieldStyle, GUILayout.Width(width));

        var variableDisplayName = BehaviourUtility.DisplayNameFromType(actionVariable.VariableType);

        EditorGUILayout.LabelField($"{actionVariable.Name} [{variableDisplayName}]", GUILayout.Width(m_themeConfig.FieldLabelWidth));

        GUI.backgroundColor = m_themeConfig.Button1Color;

        EditorGUILayout.Popup(0, new string[0]);

        EditorGUILayout.EndHorizontal();
    }

    private void DrawActionEnum(BehaviourActionEnum actionEnum)
    {
        GUI.backgroundColor = m_themeConfig.ActionFieldEnumBackgroundColor;
        var width = EditorGUIUtility.currentViewWidth * m_themeConfig.FieldWidthRatio;
        EditorGUILayout.BeginHorizontal(m_themeConfig.ActionFieldStyle, GUILayout.Width(width));

        EditorGUILayout.LabelField($"{actionEnum.Name}", GUILayout.Width(m_themeConfig.FieldLabelWidth));

        GUI.backgroundColor = m_themeConfig.Button1Color;
        actionEnum.CurrentOptionIndex = EditorGUILayout.Popup(actionEnum.CurrentOptionIndex, actionEnum.Options);
    
        EditorGUILayout.EndHorizontal();
    }

    private void DrawActionBuffer(BehaviourActionBuffer actionBuffer)
    {
        GUI.backgroundColor = m_themeConfig.ActionFieldActionBufferBackgroundColor;
        var width = EditorGUIUtility.currentViewWidth * m_themeConfig.FieldWidthRatio;
        EditorGUILayout.BeginHorizontal(m_themeConfig.ActionFieldStyle, GUILayout.Width(width));

        EditorGUILayout.LabelField($"{actionBuffer.Name}", GUILayout.Width(m_themeConfig.FieldLabelWidth));

        EditorGUILayout.EndHorizontal();
    }

    private void DrawNumberOption(BehaviourNumberOption actionNumberOption)
    {
        GUI.backgroundColor = m_themeConfig.ActionFieldNumberOptionBackgroundColor;
        var width = EditorGUIUtility.currentViewWidth * m_themeConfig.FieldWidthRatio;
        EditorGUILayout.BeginHorizontal(m_themeConfig.ActionFieldStyle, GUILayout.Width(width));

        EditorGUILayout.LabelField($"{actionNumberOption.Name}", GUILayout.Width(m_themeConfig.FieldLabelWidth));
        GUILayout.FlexibleSpace();
        
        GUI.backgroundColor = m_themeConfig.Button1Color;
        actionNumberOption.Value = EditorGUILayout.IntField(actionNumberOption.Value);

        EditorGUILayout.EndHorizontal();
    }

    private void DrawIndicator(Rect rect)
    {
        EditorGUI.DrawRect(rect, m_themeConfig.IndicatorColor);
    }
}
