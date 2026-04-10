using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using com.Sal77.BehaviourExecution;
using System.IO;

//TODO variable renaming
//TODO variable selection
//TODO Section title foldout

[CustomEditor(typeof(BehaviourObject))]
public class BehaviourObjectEditor : Editor
{
    private bool m_instructionsFoldout;
    private int m_selectedEventIndex;
    private int m_selectedActionIndex;
    private List<BehaviourAction> m_selectedActions = new();
    private BehaviourExecutionThemeConfig m_themeConfig;
    private BehaviourObject m_behaviourObject;
    private BehaviourActionBuffer m_currentActionBuffer;
    private BehaviourAction m_currentAction;

    public class BehaviourActionDrag
    {
        public BehaviourAction behaviourAction;
    }

    public class BehaviourEventDrag
    {
        public BehaviourEvent behaviourEvent;
    }

    public override void OnInspectorGUI()
    {
        if (m_themeConfig == null)
        {
            m_themeConfig = BehaviourEditorUtility.GetThemeConfig();
        }

        m_behaviourObject = (BehaviourObject)target;

        m_behaviourObject.Validate();

        if(m_currentActionBuffer == null)
        {
            m_currentActionBuffer = m_behaviourObject.BehaviourActionBuffer;
        }

        DrawInstructions();

        EditorGUILayout.Space();

        DrawBlackboardBox();

        EditorGUILayout.Space();

        DrawEventsBox();

        EditorGUILayout.Space();

        //Actions Box
        DrawActionsBox();

        //Drag Exit
        if (Event.current.type == EventType.DragExited)
        {
            DragAndDrop.SetGenericData("Behaviour Action Drag", null);
            this.Repaint();
        }
    }

    private void DrawActionsBox()
    {

        //Header
        GUI.backgroundColor = m_themeConfig.Background2Color;
        EditorGUILayout.BeginVertical(m_themeConfig.HeaderStyle);

        EditorGUILayout.LabelField("Actions", m_themeConfig.HeaderStyle);

        EditorGUILayout.EndVertical();

        //Inner Buffer Header

        if (m_currentActionBuffer != m_behaviourObject.BehaviourActionBuffer)
        {
            GUI.backgroundColor = m_themeConfig.ActionFieldActionBufferBackgroundColor;
            EditorGUILayout.BeginHorizontal(m_themeConfig.HeaderStyle);

            EditorGUILayout.LabelField($"Editing Buffer: '{m_behaviourObject.name}'", m_themeConfig.ActionFieldStyle);

            if (GUILayout.Button("Exit"))
            {
                m_currentActionBuffer = m_behaviourObject.BehaviourActionBuffer;
            }

            EditorGUILayout.EndHorizontal();
        }

        //Main Body

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

            AssetDatabase.AddObjectToAsset(action, m_behaviourObject);

            m_currentActionBuffer.Add(action);

            EditorUtility.SetDirty(m_behaviourObject);
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

        DrawBehaviourActionBuffer(m_behaviourObject, m_currentActionBuffer);

        EditorGUILayout.EndVertical();
    }

    private void DrawEventsBox()
    {
        //Events Box

        //Header
        GUI.backgroundColor = m_themeConfig.Background2Color;
        EditorGUILayout.BeginVertical(m_themeConfig.HeaderStyle);

        EditorGUILayout.LabelField("Events", m_themeConfig.HeaderStyle);

        EditorGUILayout.EndVertical();

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.Background1Style);

        EditorGUILayout.BeginHorizontal();

        GUI.backgroundColor = m_themeConfig.Button1Color;
        string[] eventscategorized = BehaviourUtility.AllAvailableEventsCategorized();
        m_selectedEventIndex = EditorGUILayout.Popup(m_selectedEventIndex, eventscategorized);

        if (GUILayout.Button("+"))
        {
            Type eventType = BehaviourUtility.EventTypeFromCategorized(eventscategorized[m_selectedEventIndex]);
            BehaviourEvent behaviourEvent = (BehaviourEvent)ScriptableObject.CreateInstance(eventType);
            behaviourEvent.name = eventType.Name + "_" + Guid.NewGuid();

            AssetDatabase.AddObjectToAsset(behaviourEvent, m_behaviourObject);

            m_behaviourObject.AddEvent(behaviourEvent);

            EditorUtility.SetDirty(m_behaviourObject);
            AssetDatabase.SaveAssets();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();

        DrawBehaviourEvents();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();
    }

    private void DrawBlackboardBox()
    {
        //Blackboard Box

        //Header
        GUI.backgroundColor = m_themeConfig.Background2Color;
        EditorGUILayout.BeginVertical(m_themeConfig.HeaderStyle);

        EditorGUILayout.LabelField("Blackboards", m_themeConfig.HeaderStyle);

        EditorGUILayout.EndVertical();

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.Background1Style);

        Func<bool> blackboardAllowFunc = () =>
        {
            var selectedObjects = DragAndDrop.objectReferences;
            return selectedObjects.Length > 0 && selectedObjects.All(x => x is BehaviourBlackboard);
        };

        if (BehaviourEditorUtility.DrawDropArea("Drop Blackboards there", DragAndDropVisualMode.Generic, blackboardAllowFunc))
        {
            var selectedObjects = DragAndDrop.objectReferences;
            foreach (var obj in selectedObjects)
            {
                if (obj is BehaviourBlackboard blackboard)
                {
                    m_behaviourObject.AddBlackboard(blackboard);
                }
            }
            EditorUtility.SetDirty(m_behaviourObject);
            AssetDatabase.SaveAssets();
        }

        //Blackboards list

        EditorGUILayout.BeginVertical();

        Stack<BehaviourBlackboard> behaviourBlackboardsQueue = new();
        foreach(var blackboard in m_behaviourObject.BehaviourBlackboards)
        {
             behaviourBlackboardsQueue.Push(blackboard);
        }

        BehaviourBlackboard blackboardToRemove = null;

        foreach (var blackboard in m_behaviourObject.BehaviourBlackboards)
        {
            GUI.backgroundColor = m_themeConfig.ActionFieldVariableBackgroundColor;
            EditorGUILayout.BeginHorizontal(m_themeConfig.ActionFieldStyle);

            EditorGUILayout.LabelField(blackboard.name, GUILayout.Width(180));

            int bufferModeCount = blackboard.BehaviourVariables.Count(x => x.VariableMode == BehaviourVariableMode.Buffer);
            int configurableModeCount = blackboard.BehaviourVariables.Count(x => x.VariableMode == BehaviourVariableMode.Configurable);

            EditorGUILayout.LabelField("Buffer: "+bufferModeCount, GUILayout.Width(100));
            EditorGUILayout.LabelField("Configurable: "+configurableModeCount, GUILayout.Width(100));

            GUI.backgroundColor = m_themeConfig.Button1Color;
            if (GUILayout.Button("X", GUILayout.Width(40)))
            {
                blackboardToRemove = blackboard;
            }

            EditorGUILayout.EndHorizontal();

        }

        if (blackboardToRemove)
        {
            m_behaviourObject.RemoveBlackboard(blackboardToRemove);
        }
            
        BehaviourBlackboard duplicate1 = null;
        BehaviourBlackboard duplicate2 = null;

        if(behaviourBlackboardsQueue.Count > 0) duplicate1 = behaviourBlackboardsQueue.Pop();

        while(behaviourBlackboardsQueue.Count > 0)
        {
            duplicate2 = behaviourBlackboardsQueue.Pop();

            var duplicateVariable = duplicate1.BehaviourVariables.FirstOrDefault(x => duplicate2.BehaviourVariables.Any(y => x.Name == y.Name ));

            if(duplicateVariable != null)
            {
                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField(
                    $"Duplicate variable '{duplicateVariable.Name}' between\n"
                    +$"Blackboard '{duplicate1.name}' and '{duplicate2.name}'."
                , GUILayout.Height(60));

                EditorGUILayout.EndVertical();
            }

            duplicate1 = duplicate2;
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();
    }

    private void DrawInstructions()
    {
        //Instructions Box

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.InstructionStyle);

        GUI.backgroundColor = m_themeConfig.Details1Color;
        m_instructionsFoldout = EditorGUILayout.Foldout(m_instructionsFoldout, "Instructions");

        if (m_instructionsFoldout)
        {
            EditorGUILayout.LabelField(
                "Expand width to read fully.\n\n"
                + "BLACKBOARDS.\n\n"
                + "EVENTS.\n\n"
                + "ACTIONS.\n"
            , GUILayout.Height(260));
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawBehaviourEvents()
    {
        BehaviourEventDrag currentEventDrag = (BehaviourEventDrag)DragAndDrop.GetGenericData("Behaviour Event Drag");

        BehaviourEvent eventToRemove = null;

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.Background1Style);

        var behaviourEvents = m_behaviourObject.BehaviourEvents;

        for (int i = 0; i < behaviourEvents.Length; i++)
        {
            var behaviourEvent = behaviourEvents[i];

            if(currentEventDrag == null || currentEventDrag.behaviourEvent != behaviourEvent)
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

                    EditorGUILayout.LabelField(behaviourEvent.EventName, m_themeConfig.ActionNameStyle);

                    GUI.backgroundColor = m_themeConfig.Button1Color;

                    if (GUILayout.Button("X", GUILayout.Width(40)))
                    {
                        eventToRemove = behaviourEvent;
                    }

                EditorGUILayout.EndHorizontal();

                DrawBehaviourEventFields(behaviourEvent);

            EditorGUILayout.EndVertical();

            if (BehaviourEditorUtility.LastRectClicked())
            {
                DragAndDrop.SetGenericData("Behaviour Event Drag", null);
                this.Repaint();
            }

            if (BehaviourEditorUtility.LastRectDragStart())
            {
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.StartDrag("Behaviour Event Drag");

                BehaviourEventDrag eventDrag = new();
                eventDrag.behaviourEvent = behaviourEvent;

                DragAndDrop.SetGenericData("Behaviour Event Drag", eventDrag);
                this.Repaint();
            }

            //Before Indicator

            if(currentEventDrag != null)
            {
                Rect lastRect = GUILayoutUtility.GetLastRect();
                Rect indicatorRect = new Rect(lastRect.x, lastRect.y - 2, lastRect.width, 4);
                var rectHitbox = new Rect(indicatorRect.x, indicatorRect.y-16, indicatorRect.width, 32);

                if(rectHitbox.Contains(Event.current.mousePosition)) DrawIndicator(indicatorRect);

                if (BehaviourEditorUtility.DropArea(rectHitbox, DragAndDropVisualMode.Move, null))
                {
                    int currentIndex = System.Array.IndexOf(behaviourEvents, currentEventDrag.behaviourEvent);
                    int targetIndex;
                    if (currentIndex < i)
                    {
                        targetIndex = i - 1;
                    }
                    else
                    {
                        targetIndex = i;
                    }

                    m_behaviourObject.InsertEventTo(currentEventDrag.behaviourEvent, targetIndex);

                    EditorUtility.SetDirty(m_behaviourObject);
                    AssetDatabase.SaveAssets();

                    DragAndDrop.SetGenericData("Behaviour Event Drag", null);
                    this.Repaint();
                }
            }
        }

        if(currentEventDrag != null)
        {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect indicatorRect = new Rect(lastRect.x, lastRect.y + lastRect.height + 2, lastRect.width, 4);
            var lastRectHitbox = new Rect(indicatorRect.x, indicatorRect.y-16, indicatorRect.width, 32);

            if(lastRectHitbox.Contains(Event.current.mousePosition)) DrawIndicator(indicatorRect);

            if (BehaviourEditorUtility.DropArea(lastRectHitbox, DragAndDropVisualMode.Move, null))
            {
                m_behaviourObject.InsertEventTo(currentEventDrag.behaviourEvent, behaviourEvents.Length);

                EditorUtility.SetDirty(m_behaviourObject);
                AssetDatabase.SaveAssets();

                DragAndDrop.SetGenericData("Behaviour Event Drag", null);
                this.Repaint();
            }
        }

        if(eventToRemove != null)
        {
            m_behaviourObject.RemoveEvent(eventToRemove);

            AssetDatabase.RemoveObjectFromAsset(eventToRemove);

            EditorUtility.SetDirty(m_behaviourObject);
            AssetDatabase.SaveAssets();
        }

        EditorGUILayout.EndVertical();

    }

    private void DrawBehaviourEventFields(BehaviourEvent behaviourEvent)
    {
        behaviourEvent.UpdateBindings();

        EditorGUILayout.BeginHorizontal();
        
        int count = 0;
        foreach (var eventVariable in behaviourEvent.EventVariables)
        {
            // New row every 2 fields
            if (count == 2)
            {
                count = 0;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }

            EditorGUILayout.BeginHorizontal();

                GUI.backgroundColor = m_themeConfig.ActionFieldVariableBackgroundColor;
                var width = EditorGUIUtility.currentViewWidth * m_themeConfig.FieldWidthRatio;
                EditorGUILayout.BeginHorizontal(m_themeConfig.ActionFieldStyle, GUILayout.Width(width));

                    var variableDisplayName = BehaviourUtility.DisplayNameFromType(eventVariable.Type);

                    EditorGUILayout.LabelField($"{eventVariable.Name} [{variableDisplayName}]");

                EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndHorizontal();

            count++;
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawBehaviourActionBuffer(BehaviourObject behaviourObject, BehaviourActionBuffer behaviourActionBuffer)
    {
        BehaviourActionDrag currentActionDrag = (BehaviourActionDrag)DragAndDrop.GetGenericData("Behaviour Action Drag");

        BehaviourAction actionToRemove = null;

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.Background1Style);

        var actions = behaviourActionBuffer.BehaviourActions;
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
                actionToRemove = behaviourAction;
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

            if(currentActionDrag != null)
            {
                Rect lastRect = GUILayoutUtility.GetLastRect();
                Rect indicatorRect = new Rect(lastRect.x, lastRect.y - 2, lastRect.width, 4);
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
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect indicatorRect = new Rect(lastRect.x, lastRect.y + lastRect.height + 2, lastRect.width, 4);
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

        if(actionToRemove != null)
        {
            m_currentActionBuffer.Remove(actionToRemove);

            AssetDatabase.RemoveObjectFromAsset(actionToRemove);

            EditorUtility.SetDirty(behaviourObject);
            AssetDatabase.SaveAssets();
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

        List<string> variableStringOptions = new();
        List<string> variableCategorizedOptions = new();

        foreach(var blackboard in m_behaviourObject.BehaviourBlackboards)
        {
            foreach(var variable in blackboard.BehaviourVariables)
            {
                if( (actionVariable.ReadMode == IBehaviourActionReadMode.Output
                    && variable.VariableMode == BehaviourVariableMode.Configurable)
                    || variable.Type != actionVariable.VariableType
                ) continue;

                variableStringOptions.Add(variable.Name);
                variableCategorizedOptions.Add(blackboard.name+"/"+variable.Name);
            }
        }

        var currentVariableIndex = variableStringOptions.IndexOf(actionVariable.TargetVariableName);

        if(currentVariableIndex != -1)
        {
            var resultIndex = EditorGUILayout.Popup(currentVariableIndex, variableCategorizedOptions.ToArray());

            if(resultIndex != currentVariableIndex)
            {
                actionVariable.TargetVariableName = variableStringOptions[resultIndex];

                EditorUtility.SetDirty(m_behaviourObject);
                AssetDatabase.SaveAssets();
            }
        }
        else
        {
            variableCategorizedOptions.Insert(0, $"NOT FOUND ({actionVariable.TargetVariableName})");

            var resultIndex = EditorGUILayout.Popup(currentVariableIndex, variableCategorizedOptions.ToArray());

            if(resultIndex != currentVariableIndex && resultIndex != -1)
            {
                actionVariable.TargetVariableName = variableStringOptions[resultIndex-1];
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawActionEnum(BehaviourActionEnum actionEnum)
    {
        GUI.backgroundColor = m_themeConfig.ActionFieldEnumBackgroundColor;
        var width = EditorGUIUtility.currentViewWidth * m_themeConfig.FieldWidthRatio;
        EditorGUILayout.BeginHorizontal(m_themeConfig.ActionFieldStyle, GUILayout.Width(width));

        EditorGUILayout.LabelField($"{actionEnum.Name}", GUILayout.Width(m_themeConfig.FieldLabelWidth));

        GUI.backgroundColor = m_themeConfig.Button1Color;
        int beforeEnum = actionEnum.CurrentOptionIndex;
        actionEnum.CurrentOptionIndex = EditorGUILayout.Popup(actionEnum.CurrentOptionIndex, actionEnum.Options);

        if(beforeEnum != actionEnum.CurrentOptionIndex)
        {
            EditorUtility.SetDirty(m_behaviourObject);
            AssetDatabase.SaveAssets();
        }
    
        EditorGUILayout.EndHorizontal();
    }

    private void DrawActionBuffer(BehaviourActionBuffer actionBuffer)
    {
        GUI.backgroundColor = m_themeConfig.ActionFieldActionBufferBackgroundColor;
        var width = EditorGUIUtility.currentViewWidth * m_themeConfig.FieldWidthRatio;
        EditorGUILayout.BeginHorizontal(m_themeConfig.ActionFieldStyle, GUILayout.Width(width));

        EditorGUILayout.LabelField($"{actionBuffer.Name}", GUILayout.Width(m_themeConfig.FieldLabelWidth));

        GUI.backgroundColor = m_themeConfig.Button1Color;
        if(GUILayout.Button("Open"))
        {
            m_currentActionBuffer = actionBuffer;
        }

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
        int beforeOption = actionNumberOption.Value;
        actionNumberOption.Value = EditorGUILayout.IntField(actionNumberOption.Value);

        if(beforeOption != actionNumberOption.Value)
        {
            EditorUtility.SetDirty(m_behaviourObject);
            AssetDatabase.SaveAssets();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawIndicator(Rect rect)
    {
        EditorGUI.DrawRect(rect, m_themeConfig.IndicatorColor);
    }
}