#nullable enable
using System;
using Lvl3Mage.EditorDevToolkit.Editor;
using Lvl3Mage.EditorEnhancements.Runtime;
using UnityEditor;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Editor
{
	[CustomPropertyDrawer(typeof(ISourceLabeledField), true)]
	public class LabeledFieldDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent content)
		{
			VerticalLayout layout = GetLayout(property);
			layout.Draw(position);
		}

		VerticalLayout GetLayout(SerializedProperty property)
		{
			VerticalLayout layout = new VerticalLayout();

			ISourceLabeledField attr = attribute as ISourceLabeledField;

			string? label = GetLabel(property);
			if (label != null){
				GUIContent labelContent = new(label);
				GUIStyle style = new(attr.Style){
					richText = attr.RichText
				};
				//Todo this breaks a little when the label wraps
				layout.Add(rect => EditorGUI.LabelField(rect, labelContent, style),
					rect => style.CalcHeight(labelContent, rect.width), 0);
			}

			if (!attr.HideProperty){
				layout.Add(rect => EditorGUI.PropertyField(rect, property, new GUIContent(property.displayName), true),
					EditorGUI.GetPropertyHeight(property), attr.UnderProperty ? 1 : 0);
			}

			return layout;
		}

		string? GetLabel(SerializedProperty property)
		{
			ISourceLabeledField attr = attribute as ISourceLabeledField;
			object propertyObject = EditorUtils.GetPropertyObject(property);
			
			// if (property.isArray){
			// 	
			// }
			if (propertyObject == null){
				return null;
			}
			Type sourceType = attr.LabelSource == SourceType.Field ? propertyObject.GetType() : fieldInfo.ReflectedType;
			object source = attr.LabelSource == SourceType.Field
				? propertyObject
				: EditorUtils.GetParentObject(property);
			return attr.GetLabel(sourceType, source);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent content)
		{
			VerticalLayout layout = GetLayout(property);
			return layout.GetHeight();
		}
	}
}