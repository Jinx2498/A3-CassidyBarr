using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.Extensions
{
    public static partial class EditorGUIExtensions
    {
        static EditorGUIExtensions()
        {
            FoldoutHash = "Foldout".GetHashCode();
            VerticalSpacingMultiField = 2;
            TextGUIContents = new Hashtable();
            EnabledStack = new Stack<bool>();
            XZLabels = new []
            {
                TextContent("X"),
                TextContent("Z")
            };
            XYZLabels = new []
            {
                TextContent("X"),
                TextContent("Y"),
                TextContent("Z")
            };

            VectorXZFloats = new float[2];
            VectorXYZFloats = new float[3];
        }

        static readonly float[] VectorXZFloats;
        static readonly float[] VectorXYZFloats;
        public static GUIContent[] XZLabels;
        public static GUIContent[] XYZLabels;

        static readonly int FoldoutHash;
        static float Indent => EditorGUI.indentLevel * 15f;
        static float VerticalSpacingMultiField; // What's the right value?
        static bool LabelHasContent(GUIContent label)
            => label == null || (label.text != string.Empty || label.image != null);

        static readonly Hashtable TextGUIContents;
        static readonly Stack<bool> EnabledStack;

        public static GUIContent[] GetLabels(string[] labels)
        {
            GUIContent[] labelsArray = new GUIContent[labels.Length];

            for (int i = 0; i < labels.Length; i++)
            {
                labelsArray[i] = TextContent(labels[i]);
            }

            return labelsArray;
        }

        public static GUIContent TextContent(string textAndTooltip)
        {
            if (textAndTooltip == null)
                textAndTooltip = "";
            string str = textAndTooltip;
            GUIContent guiContent = (GUIContent) TextGUIContents[str];
            if (guiContent == null)
            {
                string[] andTooltipString = GetNameAndTooltipString(textAndTooltip);
                guiContent = new GUIContent(andTooltipString[1]);
                if (andTooltipString[2] != null)
                    guiContent.tooltip = andTooltipString[2];
                TextGUIContents[str] = guiContent;
            }

            return guiContent;
        }

        static string[] GetNameAndTooltipString(string nameAndTooltip)
        {
            string[] strArray1 = new string[3];
            string[] strArray2 = nameAndTooltip.Split('|');
            switch (strArray2.Length)
            {
                case 0:
                    strArray1[0] = "";
                    strArray1[1] = "";
                    break;
                case 1:
                    strArray1[0] = strArray2[0].Trim();
                    strArray1[1] = strArray1[0];
                    break;
                case 2:
                    strArray1[0] = strArray2[0].Trim();
                    strArray1[1] = strArray1[0];
                    strArray1[2] = strArray2[1].Trim();
                    break;
                default:
                    Debug.LogError("Error in Tooltips: Too many strings in line beginning with '" + strArray2[0] + "'");
                    break;
            }

            return strArray1;
        }

        static Rect MultiFieldPrefixLabel(
            Rect totalPosition,
            int id,
            GUIContent label,
            int columns)
        {
            if (!LabelHasContent(label))
                return EditorGUI.IndentedRect(totalPosition);
            if (EditorGUIUtility.wideMode)
            {
                Rect labelPosition
                    = new Rect(
                        totalPosition.x + Indent,
                        totalPosition.y,
                        EditorGUIUtility.labelWidth - Indent, 18f);
                Rect rect = totalPosition;
                rect.xMin += EditorGUIUtility.labelWidth + 2f;
                if (columns == 2)
                {
                    float num = (float) ((rect.width - 8.0) / 3.0);
                    rect.xMax -= num + 4f;
                }
                EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, id);
                return rect;
            }
            Rect labelPosition1
                = new Rect(
                    totalPosition.x + Indent,
                    totalPosition.y,
                    totalPosition.width - Indent,
                    18f);
            Rect rect1 = totalPosition;
            rect1.xMin += Indent + 15f;
            rect1.yMin += 18f + VerticalSpacingMultiField;
            EditorGUI.HandlePrefixLabel(totalPosition, labelPosition1, label, id);
            return rect1;
        }

        static void MultiPropertyFieldInternal(
            Rect position,
            GUIContent[] subLabels,
            SerializedProperty valuesIterator,
            PropertyVisibility visibility,
            bool[] disabledMask = null,
            float prefixLabelWidth = -1f)
        {
            int length = subLabels.Length;
            float num = (position.width - (length - 1) * 4f) / length;
            Rect position1 = new Rect(position)
            {
                width = num
            };
            float labelWidth = EditorGUIUtility.labelWidth;
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            for (int index = 0; index < subLabels.Length; ++index)
            {
                EditorGUIUtility.labelWidth = prefixLabelWidth > 0.0
                    ? prefixLabelWidth
                    : CalcPrefixLabelWidth(subLabels[index]);
                if (disabledMask != null)
                    BeginDisabled(disabledMask[index]);
                EditorGUI.PropertyField(position1, valuesIterator, subLabels[index]);
                if (disabledMask != null)
                    EndDisabled();
                position1.x += num + 4f;
                switch (visibility)
                {
                    case PropertyVisibility.All:
                        valuesIterator.Next(false);
                        break;
                    case PropertyVisibility.OnlyVisible:
                        valuesIterator.NextVisible(false);
                        break;
                }
            }
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indentLevel;
        }

        enum PropertyVisibility
        {
            All,
            OnlyVisible,
        }

        static float CalcPrefixLabelWidth(GUIContent label, GUIStyle style = null)
        {
            if (style == null)
                style = EditorStyles.label;
            return style.CalcSize(label).x;
        }

        static void BeginDisabled(bool disabled)
        {
            EnabledStack.Push(UnityEngine.GUI.enabled);
            UnityEngine.GUI.enabled &= !disabled;
        }

        internal static void EndDisabled()
        {
            if (EnabledStack.Count <= 0)
                return;
            UnityEngine.GUI.enabled = EnabledStack.Pop();
        }
    }
}