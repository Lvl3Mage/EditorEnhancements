using System.Reflection;
using Lvl3Mage.EditorDevToolkit.Editor;
using Lvl3Mage.EditorEnhancements.Runtime;
using UnityEditor;
using UnityEngine;
namespace Lvl3Mage.EditorEnhancements.Editor
{
	[CustomPropertyDrawer(typeof(Texture2DPreview), true)]
	public class Texture2DPreviewDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty parentProp, GUIContent label)
		{
			Texture2DPreview attr = attribute as Texture2DPreview;
			
			//Update Method
			if (attr.updateMethodName != ""){
				var type = fieldInfo.ReflectedType;
				if(type == null){
					Debug.LogError("Cannot find type for fieldInfo");
					return;
				}
				var method = type.GetMethod(attr.updateMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				
				var parentObj = EditorUtils.GetParentObject(parentProp);
				// object obj = parentProp.serializedObject.FindProperty(parentProp.propertyPath);
				method.Invoke(parentObj, null);
			}
			
			
			float offset = position.y;
			
			
			//Property
			if (!attr.hideProperty){
				var propRect = EditorUtils.PropertyRect(parentProp,position.x, position.width, ref offset);
				EditorGUI.PropertyField(propRect, parentProp, new GUIContent(parentProp.displayName), true);
			}

			
			//Label / dropdown
			if (attr.showDropdown){
				parentProp.isExpanded = EditorGUI.Foldout(EditorUtils.LineRect(position.x, position.width, ref offset), parentProp.isExpanded, attr.labelText);
				EditorGUI.indentLevel += 1;
			}
			else{
				if (attr.labelText != ""){
					EditorGUI.LabelField(EditorUtils.LineRect(position.x, position.width, ref offset), attr.labelText);
				}
			}
			
			//Preview
			if (parentProp.isExpanded || !attr.showDropdown){
				Texture2D texture = parentProp.objectReferenceValue as Texture2D;
				if (texture){
					float previewWidth = attr.GetPreviewWidth();
					float previewHeight = previewWidth * texture.height / texture.width;
					Rect textureRect = new Rect(
						EditorGUI.IndentedRect( // putting the whole indented rect breaks the texture, so it is only used for the x value
							new Rect(position.x, offset, previewWidth,
								previewHeight)
						).x, offset, previewWidth, previewHeight);
					
					EditorGUI.DrawPreviewTexture(textureRect,texture);
				}
			}

			if (attr.showDropdown){
				EditorGUI.indentLevel -= 1;
			}
		}
		
		public override float GetPropertyHeight(SerializedProperty parentProp, GUIContent label)
		{
			Texture2DPreview attr = attribute as Texture2DPreview;
			Texture2D texture = parentProp.objectReferenceValue as Texture2D;
			float offset = 0;
			if(!attr.hideProperty){
				offset += EditorGUI.GetPropertyHeight(parentProp) + EditorGUIUtility.standardVerticalSpacing;//property
			}
			
			if (attr.labelText != "" || attr.showDropdown){
				offset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;//label
			}

			if (texture){
				if (parentProp.isExpanded || !attr.showDropdown){
					offset += attr.GetPreviewWidth() * texture.height/texture.width;//texture
				}
			}
			return offset;
		}
	}
}