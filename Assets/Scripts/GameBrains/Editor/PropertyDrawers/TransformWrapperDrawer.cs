using GameBrains.Extensions.Transforms;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(TransformWrapper))]
    public class TransformWrapperDrawer : PropertyDrawer
    {
        string displayName;
        TransformWrapper transformWrapper;
        SerializedProperty transformWrapperProperty;
        Transform tempTransform;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width = Screen.width;

            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            float standardVerticalSpacing = EditorGUIUtility.standardVerticalSpacing;
            float labelWidth = 14f;

            //get the name before it's gone
            displayName = property.displayName;

            property.Next(true);
            transformWrapperProperty = property.Copy();

            tempTransform = transformWrapperProperty.objectReferenceValue as Transform;

            if (tempTransform != null) { transformWrapper = tempTransform; }

            int indentLevel = EditorGUI.indentLevel; // save to restore at end

            if (transformWrapper != null && transformWrapper.WrappedTransform != null)
            {
                Rect contentPosition;

                position.height = singleLineHeight;
                EditorGUI.LabelField(position, new GUIContent(displayName));
                position.y += singleLineHeight + standardVerticalSpacing;
                EditorGUI.indentLevel += 1;
                contentPosition = EditorGUI.PrefixLabel(position, new GUIContent("Position"));
                EditorGUI.indentLevel -= 1;

                if (EditorGUIUtility.wideMode)
                {
                    contentPosition.width = (contentPosition.width - contentPosition.x) / 2 - labelWidth;
                }
                else
                {
                    EditorGUI.indentLevel += 1;
                    contentPosition = EditorGUI.IndentedRect(position);
                    contentPosition.width = contentPosition.width / 2 - 2 * labelWidth;
                    contentPosition.y += singleLineHeight;
                }

                transformWrapper.Position = Extensions.EditorGUIExtensions.VectorXYZField(contentPosition, transformWrapper.Position);

                if (EditorGUIUtility.wideMode)
                {
                    position.y += singleLineHeight + standardVerticalSpacing;
                }
                else
                {
                    position.y += 2 * (singleLineHeight + standardVerticalSpacing);
                    EditorGUI.indentLevel -= 1;
                }

                EditorGUI.indentLevel += 1;
                contentPosition = EditorGUI.PrefixLabel(position, new GUIContent("Location"));
                EditorGUI.indentLevel -= 1;

                if (EditorGUIUtility.wideMode)
                {
                    contentPosition.width = (contentPosition.width - contentPosition.x) / 2 - labelWidth;
                }
                else
                {
                    EditorGUI.indentLevel += 1;
                    contentPosition = EditorGUI.IndentedRect(position);
                    contentPosition.width = contentPosition.width / 2 - 2 * labelWidth;
                    contentPosition.y += singleLineHeight;
                }

                transformWrapper.Location = Extensions.EditorGUIExtensions.VectorXZField(contentPosition, transformWrapper.Location);
            }

            EditorGUI.indentLevel = indentLevel; // restore
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // TODO: Needs explaining
            if (transformWrapper != null && transformWrapper.WrappedTransform != null)
            {
                int factor = EditorGUIUtility.wideMode ? 1 : 2;
                return 2 * factor * (base.GetPropertyHeight(property, label) + EditorGUIUtility.standardVerticalSpacing) + EditorGUIUtility.singleLineHeight;
            }
            else
            {
                int factor = EditorGUIUtility.wideMode ? 1 : 2;
                return base.GetPropertyHeight(property, label) - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}