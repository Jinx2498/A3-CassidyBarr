using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class MultiPropertyAttribute : PropertyAttribute
    {
#if UNITY_EDITOR
        public IOrderedEnumerable<object> stored = null;

        public virtual void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position,property,label);
        }

        public virtual void OnPreGUI(Rect position, SerializedProperty property){}
        public virtual void OnPostGUI(Rect position, SerializedProperty property){}

        public virtual bool IsVisible(SerializedProperty property){return true;}

        public virtual float? GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return null;
        }
#endif
    }
}