using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace com.Sal77.BehaviourExecution
{
    /// <summary>
    /// A class that can be used to store information of any type of class.
    /// </summary>
    [Serializable]
    public class MultiTypeObject
    {
        public Type Type
        {
            get
            {
                if(m_type == null)
                {
                    m_type = Type.GetType(m_serializedType);
                }
                return m_type;
            }
            set => SetType(value);
        }

        [SerializeField] private TypeMode m_typeMode;
        [SerializeField] private Type m_type;
        [SerializeField] private string m_serializedType;

        [SerializeField] private UnityEngine.Object m_objectValue;
        [SerializeField] private int m_intValue;
        [SerializeField] private float m_floatValue;
        [SerializeField] private bool m_boolValue;
        [SerializeField] private string m_stringValue;
        [SerializeField] private Vector2 m_vector2Value;
        [SerializeField] private Vector3  m_vector3Value;
        [SerializeField] private Color m_colorValue;
        public enum TypeMode
        {
            Object,
            Int,
            Float,
            Bool,
            String,
            Vector2,
            Vector3,
            Color
        }

        #if UNITY_EDITOR
        /// <summary>
        /// Draw the multi-type object within a OnInspectorGUI
        /// </summary>
        public void DrawField(string label = "", params GUILayoutOption[] options)
        {
            switch (m_typeMode)
            {
                case TypeMode.Object:
                    m_objectValue = EditorGUILayout.ObjectField(label, m_objectValue, Type, false, options);
                    return;
                case TypeMode.Int:
                    m_intValue = EditorGUILayout.IntField(label, m_intValue, options);
                    return;
                case TypeMode.Float:
                    m_floatValue = EditorGUILayout.FloatField(label, m_floatValue, options);
                    return;
                case TypeMode.Bool:
                    m_boolValue = EditorGUILayout.Toggle(label, m_boolValue, options);
                    return;
                    case TypeMode.String:
                    m_stringValue = EditorGUILayout.TextField(label, m_stringValue, options);
                    return;
                case TypeMode.Vector2:
                    m_vector2Value = EditorGUILayout.Vector2Field(label, m_vector2Value, options);
                    return;
                case TypeMode.Vector3:
                    m_vector3Value = EditorGUILayout.Vector3Field(label, m_vector3Value, options);
                    return;
                case TypeMode.Color:
                    m_colorValue = EditorGUILayout.ColorField(label, m_colorValue, options);
                    return;
            }
        }
        #endif

        private void SetType(Type type)
        {
            if(typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                m_typeMode = TypeMode.Object;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
            }
            if(type == typeof(int))
            {
                m_typeMode = TypeMode.Int;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(float))
            {
                m_typeMode = TypeMode.Float;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(bool))
            {
                m_typeMode = TypeMode.Bool;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(string))
            {
                m_typeMode = TypeMode.String;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(Vector2))
            {
                m_typeMode = TypeMode.Vector2;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(Vector3))
            {
                m_typeMode = TypeMode.Vector3;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(Color))
            {
                m_typeMode = TypeMode.Color;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
        }
        public object GetValue()
        {
            return GetValue(m_type);
        }
        public T GetValue<T>()
        {
            return (T)GetValue(typeof(T));
        }
        public object GetValue(Type type)
        {
            if(m_type != type)
            {
                throw new Exception($"Multitype Object of type '{m_type}' attempted to read with mismatching type '{type}'");
            }

            switch (m_typeMode)
            {
                case TypeMode.Object:
                    return m_objectValue;
                case TypeMode.Int:
                    return m_intValue;
                case TypeMode.Float:
                    return m_floatValue;
                case TypeMode.Bool:
                    return m_boolValue;
                case TypeMode.String:
                    return m_stringValue;
                case TypeMode.Vector2:
                    return m_vector2Value;
                case TypeMode.Vector3:
                    return m_vector3Value;
                case TypeMode.Color:
                    return m_colorValue;
            }
            throw new Exception($"Multitype Object of type '{m_type}' attempted to read with the not available request type '{type}'");
        }
        public void SetValue<T>(object value)
        {
            SetValue(value, typeof(T));
        }
        public void SetValue(object value, Type type)
        {
            if(m_type != Type)
            {
                throw new Exception($"Multitype Object of type '{m_type}' attempted to write with mismatching type '{type}'");
            }
            
            switch (m_typeMode)
            {
                case TypeMode.Object:
                    m_objectValue = (UnityEngine.Object)value;
                    return;
                case TypeMode.Int:
                    m_intValue = (int)value;
                    return;
                case TypeMode.Float:
                    m_floatValue = (float)value;
                    return;
                case TypeMode.Bool:
                    m_boolValue = (bool)value;
                    return;
                case TypeMode.String:
                    m_stringValue = (string)value;
                    return;
                case TypeMode.Vector2:
                    m_vector2Value = (Vector2)value;
                    return;
                case TypeMode.Vector3:
                    m_vector3Value = (Vector3)value;
                    return;
                case TypeMode.Color:
                    m_colorValue = (Color)value;
                    return;
            }
            throw new Exception($"Multitype Object of type '{m_type}' attempted to write with the not available request type '{type}'");
        }
        public void SetValue(MultiTypeObject multiTypeObject)
        {
            switch (m_typeMode)
            {
                case TypeMode.Object:
                    m_objectValue = multiTypeObject.m_objectValue;
                    return;
                case TypeMode.Int:
                    m_intValue = multiTypeObject.m_intValue;
                    return;
                case TypeMode.Float:
                    m_floatValue = multiTypeObject.m_floatValue;
                    return;
                case TypeMode.Bool:
                    m_boolValue = multiTypeObject.m_boolValue;
                    return;
                case TypeMode.String:
                    m_stringValue = multiTypeObject.m_stringValue;
                    return;
                case TypeMode.Vector2:
                    m_vector2Value = multiTypeObject.m_vector2Value;
                    return;
                case TypeMode.Vector3:
                    m_vector3Value = multiTypeObject.m_vector3Value;
                    return;
                case TypeMode.Color:
                    m_colorValue = multiTypeObject.m_colorValue;
                    return;
            }
        }

        public void Reset()
        {
            switch (m_typeMode)
            {
                case TypeMode.Object:
                    return;
                case TypeMode.Int:
                    m_intValue = 0;
                    return;
                case TypeMode.Float:
                    m_floatValue = 0f;
                    return;
                case TypeMode.Bool:
                    m_boolValue = false;
                    return;
                case TypeMode.String:
                    m_stringValue = String.Empty;
                    return;
                case TypeMode.Vector2:
                    m_vector2Value = Vector2.zero;
                    return;
                case TypeMode.Vector3:
                    m_vector3Value = Vector3.zero;
                    return;
                case TypeMode.Color:
                    m_colorValue = Color.white;
                    return;
            }
        }
    }
}