using System;
using System.Reflection;
using GameBrains.Extensions.Attributes;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(HideInSubClassAttribute))]
    public class HideInSubClassAttributeDrawer : PropertyDrawer
    {
        bool ShouldShow(SerializedProperty property)
        {
            Type type = property.serializedObject.targetObject.GetType();
            FieldInfo field = type.GetField(property.name,
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null) return false;

            Type declaringType = field.DeclaringType;

            return type == declaringType;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property)) EditorGUI.PropertyField(position, property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property)) return base.GetPropertyHeight(property, label);
            return 0;
        }
    }
}