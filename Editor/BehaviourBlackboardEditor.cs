using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using com.Sal77.BehaviourExecution;
using System.IO;

[CustomEditor(typeof(BehaviourBlackboard))]
public class BehaviourBlackboardEditor : Editor
{
    private bool m_instructionsFoldout;

    private int m_selectedTypeIndex;
    private string m_currentAddingVariableName;

    private int m_variableCounter;

    private BehaviourExecutionThemeConfig m_themeConfig;
    private BehaviourBlackboard m_behaviourBlackboard;

    public class BehaviourBlackboardVariableDrag
    {
        public BehaviourVariable behaviourVariable;
    }

    public override void OnInspectorGUI()
    {
        if(m_themeConfig == null)
        {
            m_themeConfig = BehaviourEditorUtility.GetThemeConfig();
        }

        m_behaviourBlackboard = (BehaviourBlackboard)target;

        //Instructions Box

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.InstructionStyle);

        GUI.backgroundColor = m_themeConfig.Details1Color;
        m_instructionsFoldout = EditorGUILayout.Foldout(m_instructionsFoldout, "Instructions");

        if (m_instructionsFoldout)
        {
            EditorGUILayout.LabelField(
                "Expand width to read fully.\n\n"
                +"To add a variable, use the type popup and write the name of the variable.\n"
                +"and then press the '+' button.\n"
                +"Do note that you have to avoid having duplicate named variables across\n"
                +"all behaviour blackboards that will be used in a behaviour object.\n\n"
                +"You can drag a variable in front or after another to change the list order.\n\n"
                +"Variables will store a value in execution objects that can be read or written to.\n"
                +"You can set a variable as configurable which will set the variable\n"
                +"starting value when executing the behaviour.\n\n"
                +"(wip) Configurable variables will be exposed to be affected with behaviour modifiers\n"
                +"values overlapped depending on the modifier priority while\n"
                +"Integers and Floats modifiers may feature extra functionality."
            , GUILayout.Height(260));
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        //First Main Box

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.Background1Style, GUILayout.MinHeight(120));

        //Variable Button Group

        EditorGUILayout.BeginHorizontal();

        GUI.backgroundColor = m_themeConfig.Button1Color;
        string[] typescategorized = BehaviourUtility.AllAvailableTypesCategorized();
        m_selectedTypeIndex = EditorGUILayout.Popup(m_selectedTypeIndex, typescategorized);

        m_currentAddingVariableName = EditorGUILayout.TextField(m_currentAddingVariableName);

        if (GUILayout.Button("+"))
        {
            Type actionType = BehaviourUtility.TypeFromCategorized(typescategorized[m_selectedTypeIndex]).Type;
            BehaviourVariable variable = new BehaviourVariable(m_currentAddingVariableName, actionType);

            m_behaviourBlackboard.Add(variable);

            EditorUtility.SetDirty(m_behaviourBlackboard);
            AssetDatabase.SaveAssets();
        }

        EditorGUILayout.EndHorizontal();

        //Variable Instances

        GUI.backgroundColor = m_themeConfig.Background1Color;
        EditorGUILayout.BeginVertical(m_themeConfig.Background1Style);

        m_variableCounter = 0;

        foreach(var variable in m_behaviourBlackboard.BehaviourVariables)
        {
            DrawBehaviourVariable(m_behaviourBlackboard, variable);
        }
        
        //Last Indicator

        BehaviourBlackboardVariableDrag blackboardDrag = (BehaviourBlackboardVariableDrag)DragAndDrop.GetGenericData("Blackboard Variable Drag");

        if(blackboardDrag != null){
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect indicatorRect = new Rect(lastRect.x, lastRect.y + lastRect.height + 2, lastRect.width, 4);
            var rectHitbox = new Rect(indicatorRect.x, indicatorRect.y-16, indicatorRect.width, 32);

            if(rectHitbox.Contains(Event.current.mousePosition)) DrawIndicator(indicatorRect);

            if (BehaviourEditorUtility.DropArea(rectHitbox, DragAndDropVisualMode.Move, null))
            {
                m_behaviourBlackboard.InsertTo(blackboardDrag.behaviourVariable, m_variableCounter);

                EditorUtility.SetDirty(m_behaviourBlackboard);
                AssetDatabase.SaveAssets();

                DragAndDrop.SetGenericData("Blackboard Variable Drag", null);
                this.Repaint();
            }
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();

        //Drag Exit

        if (Event.current.type == EventType.DragExited)
        {
            DragAndDrop.SetGenericData("Blackboard Variable Drag", null);
            this.Repaint();
        }
    }

    private void DrawBehaviourVariable(BehaviourBlackboard behaviourBlackboard, BehaviourVariable behaviourVariable)
    {
        m_variableCounter++;

        BehaviourBlackboardVariableDrag blackboardDrag = (BehaviourBlackboardVariableDrag)DragAndDrop.GetGenericData("Blackboard Variable Drag");

        if(blackboardDrag == null || blackboardDrag.behaviourVariable != behaviourVariable)
        {
            GUI.backgroundColor = m_themeConfig.Background2Color;
        }
        else
        {
            GUI.backgroundColor = m_themeConfig.Background2Color * 0.5f + m_themeConfig.ActionDragTint * 0.5f;
        }

        EditorGUILayout.BeginVertical(m_themeConfig.Background2Style);

        EditorGUILayout.BeginHorizontal(m_themeConfig.Background2Style);
        
        GUI.backgroundColor = m_themeConfig.ActionNameBackgroundColor;

        EditorGUILayout.LabelField(behaviourVariable.Name, m_themeConfig.ActionNameStyle);

        EditorGUILayout.LabelField(BehaviourUtility.DisplayNameFromType(behaviourVariable.Type), GUILayout.Width(80));

        GUI.backgroundColor = m_themeConfig.Button1Color;

        if (GUILayout.Button("X", GUILayout.Width(40)))
        {
            behaviourBlackboard.Remove(behaviourVariable);

            EditorUtility.SetDirty(behaviourBlackboard);
            AssetDatabase.SaveAssets();
        }

        EditorGUILayout.EndHorizontal();

        var width = EditorGUIUtility.currentViewWidth * 0.45f;
        EditorGUILayout.BeginHorizontal(GUILayout.Width(width));

        GUI.backgroundColor = m_themeConfig.Button1Color;

        var variableModeOptions = Enum.GetNames(typeof(BehaviourVariableMode));
        var currentVariableMode = (int)behaviourVariable.VariableMode;
        var beforeVariableMode = behaviourVariable.VariableMode;

        behaviourVariable.VariableMode = (BehaviourVariableMode) EditorGUILayout.Popup((int)behaviourVariable.VariableMode, variableModeOptions, GUILayout.Width(80));

        if(beforeVariableMode != behaviourVariable.VariableMode)
        {
            EditorUtility.SetDirty(behaviourBlackboard);
            AssetDatabase.SaveAssets();
        }

        if(behaviourVariable.VariableMode == BehaviourVariableMode.Configurable)
        {
            var beforeValue = behaviourVariable.MultiTypeValue.GetValue();
            behaviourVariable.MultiTypeValue.DrawField("", GUILayout.ExpandWidth(true));

            if(beforeValue != behaviourVariable.MultiTypeValue.GetValue())
            {
                EditorUtility.SetDirty(behaviourBlackboard);
                AssetDatabase.SaveAssets();
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        if (BehaviourEditorUtility.LastRectClicked())
        {
            DragAndDrop.SetGenericData("Blackboard Variable Drag", null);
            this.Repaint();
        }

        if (BehaviourEditorUtility.LastRectDragStart())
            {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.StartDrag("Blackboard Variable Drag");

            BehaviourBlackboardVariableDrag blackboardVariableDrag = new();
            blackboardVariableDrag.behaviourVariable = behaviourVariable;

            DragAndDrop.SetGenericData("Blackboard Variable Drag", blackboardVariableDrag);
            this.Repaint();
        }

        //Before Indicator

        if(blackboardDrag != null){
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect indicatorRect = new Rect(lastRect.x, lastRect.y - 2, lastRect.width, 4);
            var rectHitbox = new Rect(indicatorRect.x, indicatorRect.y-16, indicatorRect.width, 32);

            if(rectHitbox.Contains(Event.current.mousePosition)) DrawIndicator(indicatorRect);

            if (BehaviourEditorUtility.DropArea(rectHitbox, DragAndDropVisualMode.Move, null))
            {
                int currentIndex = m_behaviourBlackboard.IndexOf(blackboardDrag.behaviourVariable);
                int targetIndex = m_variableCounter-1;
                if (currentIndex < m_variableCounter-1)
                {
                    targetIndex = m_variableCounter-2;
                } else
                {
                    targetIndex = m_variableCounter-1;
                }

                m_behaviourBlackboard.InsertTo(blackboardDrag.behaviourVariable, targetIndex);

                EditorUtility.SetDirty(m_behaviourBlackboard);
                AssetDatabase.SaveAssets();

                DragAndDrop.SetGenericData("Blackboard Variable Drag", null);
                this.Repaint();
            }
        }
    }

    private void DrawIndicator(Rect rect)
    {
        EditorGUI.DrawRect(rect, m_themeConfig.IndicatorColor);
    }
}