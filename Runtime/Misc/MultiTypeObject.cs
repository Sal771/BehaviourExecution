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

        [SerializeField] private MultiTypeObjectMode m_typeMode;
        [SerializeField] private Type m_type;
        [SerializeField] private string m_serializedType;

        [SerializeField] private UnityEngine.Object m_objectValue;
        [SerializeField] private int m_intValue;
        [SerializeField] private float m_floatValue;
        [SerializeField] private float MultiTypeObjectModem_floatValue;
        [SerializeField] private bool m_boolValue;
        [SerializeField] private string m_stringValue;
        [SerializeField] private Vector2 m_vector2Value;
        [SerializeField] private Vector3  m_vector3Value;
        [SerializeField] private Color m_colorValue;
        #if UNITY_EDITOR
        /// <summary>
        /// Draw the multi-type object within a OnInspectorGUI
        /// </summary>
        public void DrawField(string label = "", params GUILayoutOption[] options)
        {
            switch (m_typeMode)
            {
                case MultiTypeObjectMode.Object:
                    m_objectValue = EditorGUILayout.ObjectField(label, m_objectValue, Type, false, options);
                    return;
                case MultiTypeObjectMode.Int:
                    m_intValue = EditorGUILayout.IntField(label, m_intValue, options);
                    return;
                case MultiTypeObjectMode.Float:
                    m_floatValue = EditorGUILayout.FloatField(label, m_floatValue, options);
                    return;
                case MultiTypeObjectMode.Bool:
                    m_boolValue = EditorGUILayout.Toggle(label, m_boolValue, options);
                    return;
                    case MultiTypeObjectMode.String:
                    m_stringValue = EditorGUILayout.TextField(label, m_stringValue, options);
                    return;
                case MultiTypeObjectMode.Vector2:
                    m_vector2Value = EditorGUILayout.Vector2Field(label, m_vector2Value, options);
                    return;
                case MultiTypeObjectMode.Vector3:
                    m_vector3Value = EditorGUILayout.Vector3Field(label, m_vector3Value, options);
                    return;
                case MultiTypeObjectMode.Color:
                    m_colorValue = EditorGUILayout.ColorField(label, m_colorValue, options);
                    return;
            }
        }
        #endif

        private void SetType(Type type)
        {
            if(typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                m_typeMode = MultiTypeObjectMode.Object;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
            }
            if(type == typeof(int))
            {
                m_typeMode = MultiTypeObjectMode.Int;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(float))
            {
                m_typeMode = MultiTypeObjectMode.Float;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(bool))
            {
                m_typeMode = MultiTypeObjectMode.Bool;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(string))
            {
                m_typeMode = MultiTypeObjectMode.String;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(Vector2))
            {
                m_typeMode = MultiTypeObjectMode.Vector2;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(Vector3))
            {
                m_typeMode = MultiTypeObjectMode.Vector3;
                m_type = type;
                m_serializedType = type.AssemblyQualifiedName;
                return;
            }
            if(type == typeof(Color))
            {
                m_typeMode = MultiTypeObjectMode.Color;
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
                case MultiTypeObjectMode.Object:
                    return m_objectValue;
                case MultiTypeObjectMode.Int:
                    return m_intValue;
                case MultiTypeObjectMode.Float:
                    return m_floatValue;
                case MultiTypeObjectMode.Bool:
                    return m_boolValue;
                case MultiTypeObjectMode.String:
                    return m_stringValue;
                case MultiTypeObjectMode.Vector2:
                    return m_vector2Value;
                case MultiTypeObjectMode.Vector3:
                    return m_vector3Value;
                case MultiTypeObjectMode.Color:
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
                case MultiTypeObjectMode.Object:
                    m_objectValue = (UnityEngine.Object)value;
                    return;
                case MultiTypeObjectMode.Int:
                    m_intValue = (int)value;
                    return;
                case MultiTypeObjectMode.Float:
                    m_floatValue = (float)value;
                    return;
                case MultiTypeObjectMode.Bool:
                    m_boolValue = (bool)value;
                    return;
                case MultiTypeObjectMode.String:
                    m_stringValue = (string)value;
                    return;
                case MultiTypeObjectMode.Vector2:
                    m_vector2Value = (Vector2)value;
                    return;
                case MultiTypeObjectMode.Vector3:
                    m_vector3Value = (Vector3)value;
                    return;
                case MultiTypeObjectMode.Color:
                    m_colorValue = (Color)value;
                    return;
            }
            throw new Exception($"Multitype Object of type '{m_type}' attempted to write with the not available request type '{type}'");
        }
        public void SetValue(MultiTypeObject multiTypeObject)
        {
            switch (m_typeMode)
            {
                case MultiTypeObjectMode.Object:
                    m_objectValue = multiTypeObject.m_objectValue;
                    return;
                case MultiTypeObjectMode.Int:
                    m_intValue = multiTypeObject.m_intValue;
                    return;
                case MultiTypeObjectMode.Float:
                    m_floatValue = multiTypeObject.m_floatValue;
                    return;
                case MultiTypeObjectMode.Bool:
                    m_boolValue = multiTypeObject.m_boolValue;
                    return;
                case MultiTypeObjectMode.String:
                    m_stringValue = multiTypeObject.m_stringValue;
                    return;
                case MultiTypeObjectMode.Vector2:
                    m_vector2Value = multiTypeObject.m_vector2Value;
                    return;
                case MultiTypeObjectMode.Vector3:
                    m_vector3Value = multiTypeObject.m_vector3Value;
                    return;
                case MultiTypeObjectMode.Color:
                    m_colorValue = multiTypeObject.m_colorValue;
                    return;
            }
        }

        public void Reset()
        {
            switch (m_typeMode)
            {
                case MultiTypeObjectMode.Object:
                    return;
                case MultiTypeObjectMode.Int:
                    m_intValue = 0;
                    return;
                case MultiTypeObjectMode.Float:
                    m_floatValue = 0f;
                    return;
                case MultiTypeObjectMode.Bool:
                    m_boolValue = false;
                    return;
                case MultiTypeObjectMode.String:
                    m_stringValue = String.Empty;
                    return;
                case MultiTypeObjectMode.Vector2:
                    m_vector2Value = Vector2.zero;
                    return;
                case MultiTypeObjectMode.Vector3:
                    m_vector3Value = Vector3.zero;
                    return;
                case MultiTypeObjectMode.Color:
                    m_colorValue = Color.white;
                    return;
            }
        }
    }
}