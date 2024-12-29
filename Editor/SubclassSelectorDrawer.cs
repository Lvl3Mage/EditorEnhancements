using System;
using System.Collections.Generic;
using System.Linq;
using Lvl3Mage.EditorDevToolkit.Editor;
using Lvl3Mage.EditorEnhancements.Runtime;
using UnityEditor;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Editor
{

	[CustomPropertyDrawer(typeof(SubclassSelector), true)]
	public class SubclassSelectorDrawer : PropertyDrawer
	{

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent content)
		{
			VerticalLayout layout = GetLayout(property);
			layout.Draw(position);
		}

		VerticalLayout GetLayout(SerializedProperty property)
		{
			VerticalLayout layout = new();
			SubclassSelector attr = attribute as SubclassSelector;
			Type type = fieldInfo.FieldType;

			if (!type.IsClass && !type.IsInterface){
				Debug.LogWarning("SubclassSelector attribute can only be used on class or interface fields.");
			}
			Type[] types = attr.GetTypes(type);

			if(types.Length == 0){
				Debug.LogWarning(type.IsInterface
					? $"No classes implement the interface {type.Name}"
					: $"No subclasses found for {type.Name}");
			}
			property.managedReferenceValue ??= Activator.CreateInstance(types[0]);
			string[] options = types.Select(subclass => subclass.Name).ToArray();
			layout.Add(rect => {
				EditorGUI.BeginChangeCheck();
				Type storedType = property.managedReferenceValue?.GetType();
				int selected = Array.FindIndex(types, t => t == storedType);
				if(selected == -1){
					selected = 0;
				}
				selected = EditorGUI.Popup(rect, attr.FieldName, selected, options);

				if (EditorGUI.EndChangeCheck())
				{
					property.managedReferenceValue = Activator.CreateInstance(types[selected]);
					property.serializedObject.ApplyModifiedProperties();
				}

			}, EditorGUIUtility.singleLineHeight);
			layout.Add(rect => EditorGUI.PropertyField(rect, property, new GUIContent(property.displayName), true),
				EditorGUI.GetPropertyHeight(property), 0);

			return layout;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent content)
		{
			VerticalLayout layout = GetLayout(property);
			return layout.GetHeight();
		}
	}
}