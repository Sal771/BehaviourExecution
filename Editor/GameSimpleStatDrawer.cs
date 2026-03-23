using com.Sal77.GameVariables;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GameSimpleStat<>))]
public class GameSimpleStatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
    }
}