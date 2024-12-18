﻿using System;
using Lvl3Mage.EditorDevToolkit.Editor;
using Lvl3Mage.EditorEnhancements.Runtime;
using UnityEditor;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Editor
{

	[CustomPropertyDrawer(typeof(DropdownField), true)]
	public class DropdownFieldDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent content)
		{
			VerticalLayout layout = GetLayout(property);
			layout.Draw(position);
		}

		VerticalLayout GetLayout(SerializedProperty property)
		{
			VerticalLayout layout = new();
			DropdownField attr = attribute as DropdownField;
			layout.Add(rect => {
				string[] options = attr.GetOptions(EditorUtils.GetParentObject(property));
				if(options.Length == 0){
					options = new []{attr.EmptyValue};
				}
				EditorGUI.BeginChangeCheck();
				int selected = Array.IndexOf(options,property.stringValue);
				bool found = true;
				if(selected == -1){
					selected = 0;
					found = false;
				}
				selected = EditorGUI.Popup(rect, property.displayName, selected, options);

				if (EditorGUI.EndChangeCheck() || !found)
				{
					property.stringValue = options[selected];
				}

			}, EditorGUIUtility.singleLineHeight,0,false);
			return layout;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent content)
		{
			VerticalLayout layout = GetLayout(property);
			return layout.GetHeight();
		}
	}
}