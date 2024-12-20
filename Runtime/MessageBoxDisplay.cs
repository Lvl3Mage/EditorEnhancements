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



	/// <summary>
	/// A class that provides a simple way tp describe a message box to display in the inspector.
	/// </summary>
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

	/// <summary>
	/// An attribute that can be used to display message boxes in the inspector.
	/// </summary>
	public class MessageBoxDisplay : PropertyAttribute, IMessageBoxDisplay
	{
		readonly EditorDataSource<MessageBox[]> messageSource;


		/// <summary>
		/// Creates a new MessageBoxDisplay attribute.
		/// </summary>
		/// <param name="messages">
		/// The messages to display in the inspector.
		/// </param>
		/// <param name="types">
		/// The types of the messages to display in the inspector.
		/// </param>
		/// <param name="hideField">
		/// Whether to hide the field that this attribute is applied to.
		/// </param>
		/// <param name="afterField">
		/// Whether to display the message boxes after the field that this attribute is applied to.
		/// </param>
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

		/// <summary>
		/// Creates a new MessageBoxDisplay attribute.
		/// </summary>
		/// <param name="sourceName">
		/// The name of the method to call to get the message boxes to display in the inspector. The method should return a MessageBox array.
		/// </param>
		/// <param name="hideField">
		/// Whether to hide the field that this attribute is applied to.
		/// </param>
		/// <param name="afterField">
		/// Whether to display the message boxes after the field that this attribute is applied to.
		/// </param>
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