using GameBrains.Editor.Extensions;
using GameBrains.Extensions.Vectors;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(VectorXZ))]
    public class VectorXZDrawer : PropertyDrawer
    {
        SerializedProperty X, Z;
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

                //get the X and Z values
                property.Next(true);
                X = property.Copy();
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

            VectorXZ vectorXZ = EditorGUIExtensions.VectorXZField(
                contentPosition,
                GUIContent.none,
                new VectorXZ(X.floatValue, Z.floatValue));

            X.floatValue = vectorXZ.x;
            Z.floatValue = vectorXZ.z;

            EditorGUI.indentLevel = indentLevel; // restore
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            int factor = EditorGUIUtility.wideMode ? 1 : 2;
            return factor * base.GetPropertyHeight( property, label );
        }
    }
}