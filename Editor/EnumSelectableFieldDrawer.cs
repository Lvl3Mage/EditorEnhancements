using System;
using System.Text;
using Lvl3Mage.EditorDevToolkit.Editor;
using Lvl3Mage.EditorEnhancements.Runtime;
using UnityEditor;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Editor
{
	[CustomPropertyDrawer(typeof(EnumSelectableField))]
	public class EnumSelectableFieldDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			bool visible = CheckCondition(property);
			if(visible){
				var propRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(property));
				EditorGUI.PropertyField(propRect, property, new GUIContent(property.displayName), true);
			}
		}

		//This will need to be adjusted based on what you are displaying
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{	
			bool visible = CheckCondition(property);
			return visible ? EditorGUI.GetPropertyHeight(property) : -EditorGUIUtility.standardVerticalSpacing;
		}
		bool CheckCondition(SerializedProperty property){
			EnumSelectableField attrib = attribute as EnumSelectableField;

			SerializedProperty enumProp = EditorUtils.GetSiblingProperty(property, attrib.enumName);
			if(enumProp == null){
				Debug.LogError($"Cannot find property with name {attrib.enumName}");
			}
			return attrib.CheckCondition(enumProp.enumValueIndex);
		}
	}
}