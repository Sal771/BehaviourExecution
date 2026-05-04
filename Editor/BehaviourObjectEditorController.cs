using System;
using System.Collections.Generic;
using System.Linq;
using com.Sal77.BehaviourExecution;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(BehaviourObject))]
public class BehaviourObjectEditorController : Editor
{
    [SerializeField] private VisualTreeAsset m_visualTreeAsset;
    [SerializeField] private VisualTreeAsset m_behaviourActionAsset;
    [SerializeField] private VisualTreeAsset m_fieldRowAsset;
    [SerializeField] private VisualTreeAsset m_variableFieldAsset;
    [SerializeField] private VisualTreeAsset m_numberOptionFieldAsset;
    [SerializeField] private VisualTreeAsset m_enumFieldAsset;
    [SerializeField] private VisualTreeAsset m_actionBufferAsset;

    private BehaviourObject m_behaviourObject;
    private VisualElement m_root;
    private BehaviourActionBuffer m_currentActionBuffer;
    private Stack<BehaviourActionBuffer> m_upperActionBuffers = new();

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
        Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.sal77.behaviourexecution/Resources/BehaviourObjectIcon.png");

        return icon;
        if (icon != null)
        {
            Texture2D preview = new Texture2D(width, height);
            EditorUtility.CopySerialized(icon, preview);
            return preview;
        }
        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }

    public override VisualElement CreateInspectorGUI()
    {
        m_behaviourObject = (BehaviourObject)target;

        m_root = new VisualElement();

        m_visualTreeAsset.CloneTree(m_root);

        //

        m_root.Q<Label>("BehaviourName").text = m_behaviourObject.name;

        VisualElement tutorialFrame = m_root.Q("TutorialFrame");

        Toggle tutorialToggle = m_root.Q<Toggle>("TutorialToggle");

        tutorialToggle.RegisterValueChangedCallback((callback) => UpdateTutorial(tutorialFrame, callback.newValue));
        UpdateTutorial(tutorialFrame, tutorialToggle.value);

        UpdateActionBufferInfo();
        m_root.Q<Button>("ExitBufferButton").clicked += () =>
        {
            m_currentActionBuffer = m_upperActionBuffers.Pop();

            UpdateActionBufferInfo();
            UpdateActions();
        };

        DropdownField actionPopup = m_root.Q<DropdownField>("ActionPopup");

        actionPopup.choices = BehaviourUtility.AllAvailableActionsCategorized().ToList();

        if (actionPopup.value == null || actionPopup.value == string.Empty) actionPopup.value = actionPopup.choices.First();

        Button addButton = m_root.Q<Button>("AddButton");

        addButton.clicked += () =>
        {
            Type newActionType = BehaviourUtility.ActionTypeFromCategorized(actionPopup.value);
            BehaviourAction newAction = (BehaviourAction)ScriptableObject.CreateInstance(newActionType);
            newAction.name = newActionType.Name + "_" + Guid.NewGuid();

            AssetDatabase.AddObjectToAsset(newAction, m_behaviourObject);

            m_behaviourObject.AddAction(newAction);

            EditorUtility.SetDirty(m_behaviourObject);
            AssetDatabase.SaveAssets();

            UpdateActions();
        };

        //

        UpdateActions();

        return m_root;
    }

    private static void UpdateTutorial(VisualElement tutorialFrame, bool value)
    {
        if (!value)
        {
            tutorialFrame.AddToClassList("hidden");
        }
        else
        {
            tutorialFrame.RemoveFromClassList("hidden");
        }
    }

    private void UpdateActionBufferInfo()
    {
        VisualElement actionBufferInfo = m_root.Q("ActionBufferInfo");

        if (m_currentActionBuffer == null) m_currentActionBuffer = m_behaviourObject.BehaviourActionBuffer;

        if (m_currentActionBuffer == m_behaviourObject.BehaviourActionBuffer)
        {
            actionBufferInfo.AddToClassList("hidden");
        }
        else
        {
            actionBufferInfo.RemoveFromClassList("hidden");

            m_root.Q<Label>("ActionBufferInfoName").text = "Currently Editing Action Buffer: " + m_currentActionBuffer.Name;
        }
    }

    private void UpdateActions()
    {
        m_behaviourObject.Validate();

        VisualElement actionContent = m_root.Q("ActionContainer");

        actionContent.Clear();

        foreach (var action in m_currentActionBuffer.BehaviourActions)
        {
            if(action == null) continue;

            action.UpdateBindings();

            VisualElement actionAsset = m_behaviourActionAsset.CloneTree()[0];

            actionContent.hierarchy.Add(actionAsset);

            actionAsset.Q<Label>("ActionName").text = action.ActionName;

            Button removeButton = actionAsset.Q<Button>("RemoveButton");

            removeButton.clicked += () =>
            {

                m_behaviourObject.RemoveAction(action);

                AssetDatabase.RemoveObjectFromAsset(action);

                EditorUtility.SetDirty(m_behaviourObject);
                AssetDatabase.SaveAssets();

                UpdateActions();
            };

            UpdateActionFields(action, actionAsset);
        }
    }

    private void UpdateActionFields(BehaviourAction action, VisualElement actionAsset)
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

        VisualElement fieldRowContainer = actionAsset.Q("FieldRowContainer");
        
        int count = 0;
        Type previousActionType = null;
        VisualElement lastFieldRowContainer = default;
        
        foreach (var field in sortedFields)
        {
            var actionType = GetActionFieldType(field);

            // New row every 2 fields
            if (count == 2 || previousActionType != actionType)
            {
                count = 0;
            }

            if(count == 0)
            {
                lastFieldRowContainer = m_fieldRowAsset.CloneTree().Q("ActionFieldContainer");

                fieldRowContainer.hierarchy.Add(lastFieldRowContainer);
            }

            switch(field)
            {
                case BehaviourActionVariable actionVariable:
                    VisualElement variableFieldElement = m_variableFieldAsset.CloneTree()[0];

                    lastFieldRowContainer.hierarchy.Add(variableFieldElement);
                    UpdateVariableField(actionVariable, variableFieldElement, action, actionAsset);
                    break;

                case BehaviourActionEnum actionEnum:
                    VisualElement enumFieldElement = m_enumFieldAsset.CloneTree()[0];

                    lastFieldRowContainer.hierarchy.Add(enumFieldElement);
                    UpdateEnumField(actionEnum, enumFieldElement, action, actionAsset);
                    break;

                case BehaviourActionBuffer actionBuffer:
                    VisualElement actionBufferFieldElement = m_actionBufferAsset.CloneTree()[0];

                    lastFieldRowContainer.hierarchy.Add(actionBufferFieldElement);
                    UpdateActionBufferField(actionBuffer, actionBufferFieldElement, action, actionAsset);
                    break;

                case BehaviourNumberOption numberOption:
                    VisualElement numberOptionFieldElement = m_numberOptionFieldAsset.CloneTree()[0];

                    lastFieldRowContainer.hierarchy.Add(numberOptionFieldElement);
                    UpdateNumberOptionField(numberOption, numberOptionFieldElement, action, actionAsset);
                    break;
            };

            count++;

            previousActionType = actionType;
        }
    }
    
    private Type GetActionFieldType(BehaviourActionField actionField)
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

    private void UpdateVariableField(BehaviourActionVariable actionVariable, VisualElement visualElement, BehaviourAction action, VisualElement actionAsset)
    {
        visualElement.Q<Label>("FieldName").text = actionVariable.Name;
    }

    private void UpdateEnumField(BehaviourActionEnum actionEnum, VisualElement visualElement, BehaviourAction action, VisualElement actionAsset)
    {
        visualElement.Q<Label>("FieldName").text = actionEnum.Name;

        DropdownField enumField =  visualElement.Q<DropdownField>("EnumField");

        enumField.choices = actionEnum.Options.ToList();
        enumField.value = actionEnum.CurrentOption;

        enumField.RegisterValueChangedCallback((callback) =>
        {
            actionEnum.CurrentOptionIndex = Array.IndexOf(actionEnum.Options, callback.newValue);

            EditorUtility.SetDirty(action);
            AssetDatabase.SaveAssets();
        });

    }

    private void UpdateActionBufferField(BehaviourActionBuffer actionBuffer, VisualElement visualElement, BehaviourAction action, VisualElement actionAsset)
    {
        visualElement.Q<Label>("FieldName").text = actionBuffer.Name;

        visualElement.Q<Button>("EditButton").clicked += () =>
        {
            m_upperActionBuffers.Push(m_currentActionBuffer);
            m_currentActionBuffer = actionBuffer;

            UpdateActionBufferInfo();
            UpdateActions();
        };
    }

    private void UpdateNumberOptionField(BehaviourNumberOption numberOption, VisualElement visualElement, BehaviourAction action, VisualElement actionAsset)
    {
        visualElement.Q<Label>("FieldName").text = numberOption.Name;

        IntegerField numberOptionField =  visualElement.Q<IntegerField>("NumberOption");

        numberOptionField.value = numberOption.Value;

        numberOptionField.RegisterCallback<FocusOutEvent>((callback) =>
        {
            numberOption.Value = numberOptionField.value;

            UpdateActions();

            EditorUtility.SetDirty(action);
            AssetDatabase.SaveAssets();
        });
    }
}
