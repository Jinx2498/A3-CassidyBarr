using GameBrains.Extensions.Vectors;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.Extensions
{
    public static partial class EditorGUIExtensions
    {
        /// <summary>
        ///   <para>Makes an X and Z field for entering a VectorXZ.</para>
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the field.</param>
        /// <param name="label">Label to display above the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <returns>
        ///   <para>The value entered by the user.</para>
        /// </returns>
        public static VectorXZ VectorXZField(Rect position, string label, VectorXZ value)
            => VectorXZField(position, new GUIContent(label), value);

        /// <summary>
        ///   <para>Makes an X and Y field for entering a Vector2.</para>
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the field.</param>
        /// <param name="label">Label to display above the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <returns>
        ///   <para>The value entered by the user.</para>
        /// </returns>
        public static VectorXZ VectorXZField(Rect position, GUIContent label, VectorXZ value)
        {
            int controlId = GUIUtility.GetControlID(FoldoutHash, FocusType.Keyboard, position);
            position = MultiFieldPrefixLabel(position, controlId, label, 2);
            position.height = 18f;
            return VectorXZField(position, value);
        }

        public static VectorXZ VectorXZField(Rect position, VectorXZ value)
        {
            VectorXZFloats[0] = value.x;
            VectorXZFloats[1] = value.z;
            position.height = 18f;
            EditorGUI.BeginChangeCheck();
            EditorGUI.MultiFloatField(position, XZLabels, VectorXZFloats);
            if (EditorGUI.EndChangeCheck())
            {
                value.x = VectorXZFloats[0];
                value.z = VectorXZFloats[1];
            }
            return value;
        }
        
        public static void VectorXZField(Rect position, SerializedProperty property, GUIContent label)
        {
            int controlId = GUIUtility.GetControlID(FoldoutHash, FocusType.Keyboard, position);
            position = MultiFieldPrefixLabel(position, controlId, label, 2);
            position.height = 18f;
            SerializedProperty valuesIterator = property.Copy();
            valuesIterator.Next(true);
            MultiPropertyFieldInternal(
                position, XZLabels, valuesIterator, PropertyVisibility.All);
        }
    }
}