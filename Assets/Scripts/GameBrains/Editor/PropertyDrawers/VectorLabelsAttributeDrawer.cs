using System;
using GameBrains.Extensions.Attributes;
using GameBrains.Extensions.Vectors;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(VectorLabelsAttribute))]
    public class VectorLabelsAttributeDrawer : PropertyDrawer
    {
        static readonly GUIContent[] DefaultLabels =
        {
            new GUIContent("X"),
            new GUIContent("Y"),
            new GUIContent("Z"),
            new GUIContent("W")
        };

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int factor = EditorGUIUtility.wideMode ? 1 : 2;
            return factor * base.GetPropertyHeight(property, label);
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            VectorLabelsAttribute vectorLabels = (VectorLabelsAttribute) attribute;
            string niceName = ObjectNames.NicifyVariableName(property.name);

            if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                int[] array = {property.vector2IntValue.x, property.vector2IntValue.y};
                array = DrawFields(position, array, niceName, EditorGUI.MultiIntField, vectorLabels);
                property.vector2IntValue = new Vector2Int(array[0], array[1]);
            }
            else if (property.propertyType == SerializedPropertyType.Vector3Int)
            {
                int[] array = {property.vector3IntValue.x, property.vector3IntValue.y, property.vector3IntValue.z};
                array = DrawFields(position, array, niceName, EditorGUI.MultiIntField, vectorLabels);
                property.vector3IntValue = new Vector3Int(array[0], array[1], array[2]);
            }
            else if (property.propertyType == SerializedPropertyType.Vector2)
            {
                float[] array = {property.vector2Value.x, property.vector2Value.y};
                array = DrawFields(position, array, niceName, EditorGUI.MultiFloatField, vectorLabels);
                property.vector2Value = new Vector2(array[0], array[1]);
            }
            else if (property.propertyType == SerializedPropertyType.Vector3)
            {
                float[] array = {property.vector3Value.x, property.vector3Value.y, property.vector3Value.z};
                array = DrawFields(position, array, niceName, EditorGUI.MultiFloatField, vectorLabels);
                property.vector3Value = new Vector3(array[0], array[1], array[2]);
            }
            else if (property.propertyType == SerializedPropertyType.Vector4)
            {
                float[] array =
                {
                    property.vector4Value.x,
                    property.vector4Value.y,
                    property.vector4Value.z,
                    property.vector4Value.w
                };
                array = DrawFields(position, array, niceName, EditorGUI.MultiFloatField, vectorLabels);
                property.vector4Value = new Vector4(array[0], array[1], array[2]);
            }
            else if (property.type == nameof(VectorXZ))
            {
                property.Next(true);
                SerializedProperty X = property.Copy();
                property.Next(false);
                SerializedProperty Z = property.Copy();
                float[] array = {X.floatValue, Z.floatValue};
                array = DrawFields(position, array, niceName, EditorGUI.MultiFloatField, vectorLabels);
                X.floatValue = array[0];
                Z.floatValue = array[1];
            }
            else if (property.type == nameof(VectorXYZ))
            {
                property.Next(true);
                SerializedProperty X = property.Copy();
                property.Next(false);
                SerializedProperty Y = property.Copy();
                property.Next(false);
                SerializedProperty Z = property.Copy();
                float[] array = {X.floatValue, Y.floatValue, Z.floatValue};
                array = DrawFields(position, array, niceName, EditorGUI.MultiFloatField, vectorLabels);
                X.floatValue = array[0];
                Y.floatValue = array[1];
                Z.floatValue = array[2];
            }
        }

        T[] DrawFields<T>(
            Rect position,
            T[] vector,
            string mainLabel,
            Action<Rect, GUIContent[], T[]> fieldDrawer,
            VectorLabelsAttribute vectorLabels)
        {
            T[] result = vector;

            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            float labelWidth = 14f;
            int indentLevel = EditorGUI.indentLevel; // save to restore at the end

            position.width = Screen.width;

            Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(mainLabel));

            position.height = EditorGUIUtility.singleLineHeight;

            if (EditorGUIUtility.wideMode)
            {
                contentPosition.width = (contentPosition.width - contentPosition.x) / 2 - labelWidth;
            }
            else
            {
                EditorGUI.indentLevel++;

                contentPosition = EditorGUI.IndentedRect(position);
                contentPosition.width = contentPosition.width / 2 - 2 * labelWidth;
                contentPosition.y += singleLineHeight;
            }

            GUIContent[] labelsArray = new GUIContent[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                GUIContent label = vectorLabels.Labels.Length > i
                    ? new GUIContent(vectorLabels.Labels[i])
                    : DefaultLabels[i];
                labelsArray[i] = label;
            }

            fieldDrawer(contentPosition, labelsArray, vector);

            EditorGUI.indentLevel = indentLevel; // restore

            return result;
        }
    }
}