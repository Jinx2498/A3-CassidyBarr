using GameBrains.Extensions.Vectors;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.Extensions
{
    public static partial class EditorGUILayoutExtensions
    {
        /// <summary>
        ///   <para>Make an X, Y &amp; Z field for entering a VectorXYZ.</para>
        /// </summary>
        /// <param name="label">Label to display above the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="options">An optional list of layout options that specify extra layout
        ///         properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
        /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
        /// <returns>
        ///   <para>The value entered by the user.</para>
        /// </returns>
        public static VectorXYZ VectorXYZField(
            string label,
            VectorXYZ value,
            params GUILayoutOption[] options)
        {
            return VectorXYZField(new GUIContent(label), value, options);
        }

        /// <summary>
        ///   <para>Make an X, Y &amp; Z field for entering a VectorXYZ.</para>
        /// </summary>
        /// <param name="label">Label to display above the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="options">An optional list of layout options that specify extra layout
        ///         properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
        /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
        /// <returns>
        ///   <para>The value entered by the user.</para>
        /// </returns>
        public static VectorXYZ VectorXYZField(
            GUIContent label,
            VectorXYZ value,
            params GUILayoutOption[] options)
        {
            return EditorGUIExtensions.VectorXYZField(
            LastRect = EditorGUILayout.GetControlRect(
                true,
                EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3, label),
                EditorStyles.numberField, options),
            label,
            value);
        }
    }
}