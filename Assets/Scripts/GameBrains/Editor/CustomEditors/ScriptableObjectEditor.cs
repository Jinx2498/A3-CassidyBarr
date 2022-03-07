using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameBrains.Extensions.Attributes;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.CustomEditors
{
	[CustomEditor(typeof(ScriptableObject), true, isFallback = true)]
	[CanEditMultipleObjects]
    public class ScriptableObjectEditor : UnityEditor.Editor
    {
        #region Hidden Properties Attribute

        bool hasHiddenProperties;
        string[] hiddenProperties;

        #endregion Hidden Properties Attribute

        #region Foldout Attribute

        readonly Dictionary<string, FoldoutRecord> foldoutRecords
	        = new Dictionary<string, FoldoutRecord>();
        readonly List<SerializedProperty> propertyList = new List<SerializedProperty>();
        int length;
        FieldInfo[] objectFields;
        bool foldoutRecordsInitialized;
        FoldoutAttribute previousFoldout;

        #endregion Foldout Attribute

        void OnEnable()
        {
            var type = target.GetType();

            if (type.GetCustomAttributes(typeof(HidePropertiesInInspectorAttribute), true)
                    is HidePropertiesInInspectorAttribute[] hidePropertiesInInspectorAttributes &&
                hidePropertiesInInspectorAttributes.Length > 0)
            {
                hasHiddenProperties = true;
                var propertiesToHideSet = new HashSet<string>();
                foreach (var customAttribute in hidePropertiesInInspectorAttributes)
                {
                    foreach (var propertyToHide in customAttribute.HiddenProperties)
                    {
                        propertiesToHideSet.Add(propertyToHide);
                    }
                }

                hiddenProperties = new string[propertiesToHideSet.Count];
                propertiesToHideSet.CopyTo(hiddenProperties);
            }
            else
            {
                foldoutRecordsInitialized = false;
                objectFields = target.GetType().GetFields(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                length = objectFields.Length;
            }
        }

        void OnDisable()
		{
			foreach (var keyValuePair in foldoutRecords)
			{
				keyValuePair.Value.Dispose();
			}
		}

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (hasHiddenProperties)
            {
                DrawDefaultInspectorExcept(hiddenProperties);
            }
            else
            {
                DrawDefaultInspectorWithFoldouts();
            }

            serializedObject.ApplyModifiedProperties();
        }

        public bool DrawDefaultInspectorExcept(params string[] propertiesNotToDraw)
        {
	        EditorGUI.BeginChangeCheck();

            var iterator = serializedObject.GetIterator();
            for (bool enterChildren = true;
                iterator.NextVisible(enterChildren);
                enterChildren = false)
            {
                if (propertiesNotToDraw == null ||
                    !propertiesNotToDraw.ToList().Contains(iterator.name))
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }
            }

            return EditorGUI.EndChangeCheck();
        }

        #region Foldout Attribute

        public void DrawDefaultInspectorWithFoldouts()
        {
	        if (!foldoutRecordsInitialized)
	        {
		        for (var i = 0; i < length; i++)
		        {
			        var foldoutAttribute
				        = Attribute.GetCustomAttribute(objectFields[i], typeof(FoldoutAttribute))
					        as FoldoutAttribute;
			        FoldoutRecord foldoutRecord;
			        if (foldoutAttribute == null)
			        {
				        if (previousFoldout != null && previousFoldout.foldEverything)
				        {
					        if (!foldoutRecords.TryGetValue(previousFoldout.name, out foldoutRecord))
					        {
						        foldoutRecords.Add(
							        previousFoldout.name,
							        new FoldoutRecord
							        {
								        foldoutAttribute = previousFoldout,
								        types = new HashSet<string> {objectFields[i].Name}
							        });
					        }
					        else
					        {
						        foldoutRecord.types.Add(objectFields[i].Name);
					        }
				        }

				        continue;
			        }

			        previousFoldout = foldoutAttribute;
			        if (!foldoutRecords.TryGetValue(foldoutAttribute.name, out foldoutRecord))
			        {
				        foldoutRecords.Add(foldoutAttribute.name, new FoldoutRecord
				        {
					        foldoutAttribute = foldoutAttribute,
					        types = new HashSet<string> {objectFields[i].Name}
				        });
			        }
			        else
			        {
				        foldoutRecord.types.Add(objectFields[i].Name);
			        }
		        }

		        var property = serializedObject.GetIterator();
		        var next = property.NextVisible(true);
		        if (next)
		        {
			        do
			        {
				        HandleProperty(property);
			        } while (property.NextVisible(false));
		        }
	        }

	        if (propertyList.Count == 0)
            {
                DrawDefaultInspector();
                return;
            }

            foldoutRecordsInitialized = true;

            using (new EditorGUI.DisabledScope("m_Script" == propertyList[0].propertyPath))
            {
	            EditorGUILayout.PropertyField(propertyList[0], true);
            }

            foreach (var foldoutRecord in foldoutRecords)
			{
				EditorGUILayout.BeginVertical();

				var controlRect = EditorGUILayout.GetControlRect();

				foldoutRecord.Value.expanded
					= EditorGUI.Foldout(
						new Rect(
							controlRect.x,
							controlRect.y,
							controlRect.width,
							controlRect.height),
						foldoutRecord.Value.expanded,
						new GUIContent(foldoutRecord.Value.foldoutAttribute.name),
						EditorStyles.foldout);

				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical();

				if (foldoutRecord.Value.expanded)
				{
					{
						for (int i = 0; i < foldoutRecord.Value.properties.Count; i++)
						{
							EditorGUI.indentLevel = 1;

							EditorGUILayout.PropertyField(
								foldoutRecord.Value.properties[i],
								new GUIContent(foldoutRecord.Value.properties[i].displayName),
								true);

							//if (i == pair.Value.properties.Count - 1) EditorGUILayout.Space();
						}
					}
				}

				EditorGUI.indentLevel = 0;
				EditorGUILayout.EndVertical();
			}

			for (var i = 1; i < propertyList.Count; i++)
			{
				EditorGUILayout.PropertyField(propertyList[i], true);
			}
        }

        public void HandleProperty(SerializedProperty property)
        {
	        bool shouldBeFolded = false;

	        foreach (var foldoutRecord in foldoutRecords)
	        {
		        if (foldoutRecord.Value.types.Contains(property.name))
		        {
			        shouldBeFolded = true;
			        foldoutRecord.Value.properties.Add(property.Copy());

			        break;
		        }
	        }

	        if (shouldBeFolded == false)
	        {
		        propertyList.Add(property.Copy());
	        }
        }

        class FoldoutRecord
        {
            public HashSet<string> types = new HashSet<string>();
            public readonly List<SerializedProperty> properties = new List<SerializedProperty>();
            public FoldoutAttribute foldoutAttribute;
            public bool expanded;

            public void Dispose()
            {
                properties.Clear();
                types.Clear();
                foldoutAttribute = null;
            }
        }

        #endregion Foldout Attribute
    }
}