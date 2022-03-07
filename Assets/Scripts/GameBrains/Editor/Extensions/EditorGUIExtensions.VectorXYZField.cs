using GameBrains.Extensions.Vectors;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.Extensions
{
    public static partial class EditorGUIExtensions
    {
        /// <summary>
        ///   <para>Makes an X, Y, and Z field for entering a VectorXYZ.</para>
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the field.</param>
        /// <param name="label">Label to display above the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <returns>
        ///   <para>The value entered by the user.</para>
        /// </returns>
        public static VectorXYZ VectorXYZField(Rect position, string label, VectorXYZ value)
            => VectorXYZField(position, new GUIContent(label), value);

        /// <summary>
        ///   <para>Makes an X, Y, and Z field for entering a VectorXYZ.</para>
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the field.</param>
        /// <param name="label">Label to display above the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <returns>
        ///   <para>The value entered by the user.</para>
        /// </returns>
        public static VectorXYZ VectorXYZField(Rect position, GUIContent label, VectorXYZ value)
        {
            int controlId = GUIUtility.GetControlID(FoldoutHash, FocusType.Keyboard, position);
            position = MultiFieldPrefixLabel(position, controlId, label, 3);
            position.height = 18f;
            return VectorXYZField(position, value);
        }

        public static VectorXYZ VectorXYZField(Rect position, VectorXYZ value)
        {
            VectorXYZFloats[0] = value.x;
            VectorXYZFloats[1] = value.y;
            VectorXYZFloats[2] = value.z;
            position.height = 18f;
            EditorGUI.BeginChangeCheck();
            EditorGUI.MultiFloatField(position, XYZLabels, VectorXYZFloats);
            if (EditorGUI.EndChangeCheck())
            {
                value.x = VectorXYZFloats[0];
                value.y = VectorXYZFloats[1];
                value.z = VectorXYZFloats[2];
            }

            return value;
        }

        public static void VectorXYZField(Rect position, SerializedProperty property, GUIContent label)
        {
            int controlId = GUIUtility.GetControlID(FoldoutHash, FocusType.Keyboard, position);
            position = MultiFieldPrefixLabel(position, controlId, label, 3);
            position.height = 18f;
            SerializedProperty valuesIterator = property.Copy();
            valuesIterator.Next(true);
            MultiPropertyFieldInternal(position, XYZLabels, valuesIterator, PropertyVisibility.All);
        }
    }
}