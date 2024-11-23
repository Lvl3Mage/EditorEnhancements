#nullable enable
using System;
using System.Collections.Generic;
using Lvl3Mage.EditorDevToolkit.Runtime;
using UnityEngine;
using Nullable = System.Nullable;

namespace Lvl3Mage.EditorEnhancements.Runtime
{

	public interface IMessageBoxDisplay
	{
		public MessageBox[] GetMessageBoxes(object? obj)
		{
			return Array.Empty<MessageBox>();
		}
		public bool HideField { get; }
		public bool AfterField { get; }
	}


	public class MessageBox
	{
		public MessageBox(string message, Type type)
		{
			MessageType = type;
			Message = message;
		}

		public enum Type
		{
			Info,
			Warning,
			Error
		}

		public readonly Type MessageType;
		public readonly string Message;
	}

	public class MessageBoxDisplay : PropertyAttribute, IMessageBoxDisplay
	{
		readonly EditorDataSource<MessageBox[]> messageSource;

		public MessageBoxDisplay(string[] messages, MessageBox.Type[] types, bool hideField = false, bool afterField = false)
		{
			MessageBox[] boxes = new MessageBox[messages.Length];
			for (int i = 0; i < messages.Length; i++){
				boxes[i] = new MessageBox(messages[i], types[i]);
			}
			messageSource = new EditorDataSource<MessageBox[]>(boxes);
			HideField = hideField;
			AfterField = afterField;
		}
		public MessageBoxDisplay(string sourceName, bool hideField = false, bool afterField = false)
		{
			messageSource = new EditorDataSource<MessageBox[]>(sourceName);
			HideField = hideField;
			AfterField = afterField;
		}
		public MessageBox[] GetMessageBoxes(object? obj)
		{
			if(obj == null){
				return Array.Empty<MessageBox>();
			}
			if (messageSource.Get(obj, out MessageBox[] result)){
				return result;
			}
			return Array.Empty<MessageBox>();
		}

		public bool HideField { get; }
		public bool AfterField { get; }
	}
}