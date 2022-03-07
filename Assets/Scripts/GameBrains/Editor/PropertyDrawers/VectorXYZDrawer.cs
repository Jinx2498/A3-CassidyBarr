using GameBrains.Editor.Extensions;
using GameBrains.Extensions.Vectors;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(VectorXYZ))]
    public class VectorXYZDrawer : PropertyDrawer
    {
        SerializedProperty X, Y, Z;
        string name;
        bool cache;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width = Screen.width;
            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            float standardVerticalSpacing = EditorGUIUtility.standardVerticalSpacing;
            float labelWidth = 14f;
            float indent = EditorGUI.indentLevel * 15f;

            if (!cache)
            {
                //get the name before it's gone
                name = property.displayName;

                //get the X, Y and Z values
                property.Next(true);
                X = property.Copy();
                property.Next(true);
                Y = property.Copy();
                property.Next(false);
                Z = property.Copy();

                cache = true;
            }

            int indentLevel = EditorGUI.indentLevel; // save to restore at end

            Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(name));

            //Check if there is enough space to put the name on the same line (to save space)
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

            VectorXYZ vectorXYZ =EditorGUIExtensions.VectorXYZField(
                contentPosition,
                GUIContent.none,
                new VectorXYZ(X.floatValue, Y.floatValue, Z.floatValue));

            X.floatValue = vectorXYZ.x;
            Y.floatValue = vectorXYZ.y;
            Z.floatValue = vectorXYZ.z;

            EditorGUI.indentLevel = indentLevel; // restore
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            int factor = EditorGUIUtility.wideMode ? 1 : 2;
            return factor * base.GetPropertyHeight( property, label );
        }
    }
}