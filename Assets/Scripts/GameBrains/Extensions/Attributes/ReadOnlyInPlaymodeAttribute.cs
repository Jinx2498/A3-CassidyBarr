using UnityEditor;
using UnityEngine;

namespace GameBrains.Extensions.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ReadOnlyInPlaymodeAttribute : MultiPropertyAttribute
    {
        public override void OnPreGUI(Rect position, SerializedProperty property)
        {
            UnityEngine.GUI.enabled = !Application.isPlaying;
        }

        public override void OnPostGUI(Rect position, SerializedProperty property)
        {
            UnityEngine.GUI.enabled = true;
        }
    }
}