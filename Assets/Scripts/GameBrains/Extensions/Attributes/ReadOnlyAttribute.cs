using System;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        public override void OnPreGUI(Rect position, SerializedProperty property)
        {
            UnityEngine.GUI.enabled = false;
        }

        public override void OnPostGUI(Rect position, SerializedProperty property)
        {
            UnityEngine.GUI.enabled = true;
        }
#endif
    }
}