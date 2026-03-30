using System;
using UnityEditor;
using UnityEngine;

public static class BehaviourEditorUtility
{
    public static BehaviourExecutionThemeConfig GetThemeConfig()
    {
        BehaviourExecutionThemeConfig themeConfig = null;
        string[] guids = AssetDatabase.FindAssets("t:BehaviourExecutionThemeConfig");
        if (guids.Length > 0)
        {
            themeConfig = AssetDatabase.LoadAssetAtPath<BehaviourExecutionThemeConfig>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        if (themeConfig == null)
        {
            themeConfig = ScriptableObject.CreateInstance<BehaviourExecutionThemeConfig>();
            AssetDatabase.CreateAsset(themeConfig, "Assets/Settings/BehaviourObject Editor Theme Config.asset");
        }

        return themeConfig;
    }

    public static bool LastRectClicked()
    {
        return RectClicked(GUILayoutUtility.GetLastRect());
    }
    public static bool LastRectDragStart()
    {
        return RectDragStart(GUILayoutUtility.GetLastRect());
    }
    public static bool RectClicked(Rect rect)
    {
        var currentEvent = Event.current;
        if (rect.Contains(currentEvent.mousePosition))
        {
            if(currentEvent.type == EventType.MouseUp)
            {
                return true;
            }
        }
        return false;
    }

    public static bool RectDragStart(Rect rect)
    {
        var currentEvent = Event.current;
        if (rect.Contains(currentEvent.mousePosition))
        {
            if(currentEvent.type == EventType.MouseDrag)
            {
                return true;
            }
        }
        return false;
    }

    public static bool DrawDropArea(Func<bool> allowFunc)
    {
        return DrawDropArea("", DragAndDropVisualMode.Generic, allowFunc);
    }

    public static bool DrawDropArea(DragAndDropVisualMode defaultDropMode, Func<bool> allowFunc)
    {
        return DrawDropArea("", defaultDropMode, allowFunc);
    }

    public static bool DrawDropArea(string label, DragAndDropVisualMode defaultDropMode, Func<bool> allowFunc, params GUILayoutOption[] options)
    {
        var rect = GUILayoutUtility.GetRect(50f, 50f, options);
        GUI.Box(rect, label);

        return DropArea(rect, defaultDropMode, allowFunc);
    }

    public static bool DropArea(Rect rect, DragAndDropVisualMode defaultDropMode, Func<bool> allowFunc)
    {
        var currentEvent = Event.current;

        switch (currentEvent.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!rect.Contains(currentEvent.mousePosition))
                {
                    break;
                }

                if(allowFunc != null && !allowFunc.Invoke())
                {
                    break;
                }

                DragAndDrop.visualMode = defaultDropMode;

                if(currentEvent.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    Event.current.Use();

                    return true;
                }
            break;
        }
        return false;
    }
        
}