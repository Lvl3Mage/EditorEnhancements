using System.Reflection;
using Lvl3Mage.EditorDevToolkit.Editor;
using Lvl3Mage.EditorEnhancements.Runtime;
using UnityEditor;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Editor
{
	[CustomPropertyDrawer(typeof(ActionButton), true)]
	public class ActionButtonDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			
				// var propRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(property));
				// EditorGUI.PropertyField(propRect, property, new GUIContent(property.displayName), true);
			ActionButton attr = attribute as ActionButton;
			float offset = position.y;
			if(attr.afterProperty && !attr.hideProperty){
				Rect propRect = EditorUtils.PropertyRect(property, position.x, position.width, ref offset);
				EditorGUI.PropertyField(propRect, property, new GUIContent(property.displayName), true);
			}

			if (GUI.Button(EditorGUI.IndentedRect(EditorUtils.LineRect(position.x, position.width, ref offset)), attr.buttonText)){
				var type = fieldInfo.ReflectedType;
				if(type == null){
					Debug.LogError("Cannot find type for fieldInfo");
					return;
				}
				var method = type.GetMethod(attr.methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				
				var parentObj = EditorUtils.GetParentObject(property);
				method.Invoke(parentObj, null);
			}

			if (!attr.afterProperty && !attr.hideProperty){
				Rect propRect = EditorUtils.PropertyRect(property, position.x, position.width, ref offset);
				EditorGUI.PropertyField(propRect, property, new GUIContent(property.displayName), true);
			}
		}
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			ActionButton attr = attribute as ActionButton;
			if (attr.hideProperty) return EditorGUIUtility.singleLineHeight;
			return EditorGUI.GetPropertyHeight(property) +  EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}
	}
}