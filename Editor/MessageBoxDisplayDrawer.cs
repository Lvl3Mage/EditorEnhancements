using System.Collections.Generic;
using Lvl3Mage.EditorDevToolkit.Editor;
using Lvl3Mage.EditorEnhancements.Runtime;
using UnityEditor;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Editor
{
	[CustomPropertyDrawer(typeof(IMessageBoxDisplay), true)]
	public class MessageBoxDisplayDrawer : PropertyDrawer
	{
		static Dictionary<MessageBox.Type, MessageType> messageTypeMap = new(){
			{MessageBox.Type.Info, MessageType.Info},
			{MessageBox.Type.Warning, MessageType.Warning},
			{MessageBox.Type.Error, MessageType.Error}
		};
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent content)
		{
			VerticalLayout layout = GetLayout(property);
			layout.Draw(position);
		}

		VerticalLayout GetLayout(SerializedProperty property)
		{
			VerticalLayout layout = new();
			IMessageBoxDisplay attr = attribute as IMessageBoxDisplay;
			MessageBox[] messages = attr.GetMessageBoxes(EditorUtils.GetParentObject(property));
			if(messages.Length == 0){
				return layout;
			}
			foreach(MessageBox box in messages){
				MessageType messageType = messageTypeMap[box.MessageType];
				layout.Add(rect => EditorGUI.HelpBox(rect, box.Message, messageType),
					rect => EditorStyles.helpBox.CalcHeight(new GUIContent(box.Message), EditorGUIUtility.currentViewWidth));
			}
			if(!attr.HideField){
				layout.Add(rect => EditorGUI.PropertyField(rect, property, new GUIContent(property.displayName), true),
					rect => EditorGUI.GetPropertyHeight(property), attr.AfterField ? -1 : 1);
			}

			return layout;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent content)
		{
			VerticalLayout layout = GetLayout(property);
			return layout.GetHeight();
		}
	}
}