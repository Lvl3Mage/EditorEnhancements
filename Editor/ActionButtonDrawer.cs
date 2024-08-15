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
			if(attr.afterField && !attr.hideField){
				Rect propRect = EditorUtils.ReservePropertyRect(ref position, property);
				EditorGUI.PropertyField(propRect, property, new GUIContent(property.displayName), true);
			}
			GUIStyle style = new GUIStyle(attr.style);
			// if (attr.fullWidth){
			// 	style.wid = true;
			// }
			GUIContent content = new GUIContent(attr.buttonText);

			Rect buttonRect = EditorGUI.IndentedRect(EditorUtils.ReserveContentRect(ref position, content, style));
			if (GUI.Button(buttonRect, attr.buttonText, style)){
				var targetObj = attr.methodSource == SourceType.Field ? EditorUtils.GetPropertyObject(property) : EditorUtils.GetParentObject(property);
				var type = targetObj.GetType();
				var method = type.GetMethod(attr.methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if(method == null){
					Debug.LogError($"Cannot find method '{attr.methodName}' in type '{type}'");
					return;
				}
				method.Invoke(targetObj, null);
			}

			if (!attr.afterField && !attr.hideField){
				EditorUtils.DrawProperty(ref position, property);
			}
		}
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			ActionButton attr = attribute as ActionButton;
			if (attr.hideField) return EditorGUIUtility.singleLineHeight;
			return EditorGUI.GetPropertyHeight(property) +  EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}
	}
}