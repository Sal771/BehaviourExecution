using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using com.Sal77.BehaviourExecution;
using com.Sal77.GameCore;
using System.IO;

public class BehaviourExecutionThemeConfig : ScriptableObject
{
    public Color Background1Color = Color.white;
    public Color Background2Color = Color.white;
    public Color Details1Color = Color.white;
    public Color ActionDragTint = Color.white;
    public Color Button1Color = Color.white;

    public Color ActionNameBackgroundColor = Color.white;
    public Color ActionFieldVariableBackgroundColor;
    public Color ActionFieldEnumBackgroundColor;
    public Color ActionFieldNumberOptionBackgroundColor;
    public Color ActionFieldActionBufferBackgroundColor;
    public Color IndicatorColor = Color.white;

    public GUIStyle Background1Style;
    public GUIStyle Background2Style;
    public GUIStyle Button1Style;

    public GUIStyle HeaderStyle;
    public GUIStyle InstructionStyle;
    public GUIStyle ActionNameStyle;
    public GUIStyle ActionFieldStyle;

    public float FieldWidthRatio;
    public int FieldLabelWidth;
}
