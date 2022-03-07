using GameBrains.Extensions.Vectors;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.Extensions
{
    public static partial class EditorGUILayoutExtensions
    {
        /// <summary>
        ///   <para>Make an X &amp; Z field for entering a VectorXZ.</para>
        /// </summary>
        /// <param name="label">Label to display above the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any values passed in here will override settings defined by the style.&lt;br&gt;</param>
        /// <returns>
        ///   <para>The value entered by the user.</para>
        /// </returns>
        public static VectorXZ VectorXZField(
            string label,
            VectorXZ value,
            params GUILayoutOption[] options)
        {
            return VectorXZField(new GUIContent(label), value, options);
        }

        /// <summary>
        ///   <para>Make an X &amp; Z field for entering a VectorXZ.</para>
        /// </summary>
        /// <param name="label">Label to display above the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any values passed in here will override settings defined by the style.&lt;br&gt;</param>
        /// <returns>
        ///   <para>The value entered by the user.</para>
        /// </returns>
        public static VectorXZ VectorXZField(
            GUIContent label,
            VectorXZ value,
            params GUILayoutOption[] options)
        {
            return EditorGUIExtensions.VectorXZField(
                LastRect = EditorGUILayout.GetControlRect(
                    true,
                    EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, label),
                    EditorStyles.numberField,
                    options),
                label,
                value);
        }
    }
}