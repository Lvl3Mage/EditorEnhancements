#nullable enable
using System;
using Lvl3Mage.EditorDevToolkit.Runtime;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Runtime
{
	public enum SourceType
	{
		Field,
		Parent
	}
	/// <summary>
	/// An attribute that adds a button to the inspector. The button will call a method on the target object when clicked.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class ActionButton : PropertyAttribute
	{
		public readonly string buttonText;
		public readonly string methodName;
		public readonly bool hideField;
		public readonly bool afterField;
		public readonly SourceType methodSource;
		public readonly string style;
		// public readonly bool fullWidth;

		/// <summary>
		/// Creates a new ActionButton attribute.
		/// </summary>
		/// <param name="buttonText">
		///     The text to display on the button.
		/// </param>
		/// <param name="methodName">
		///     The name of the method to call when the button is clicked. The method will be called on the field object or the parent object, depending on the value of <see cref="methodSource"/>.
		/// </param>
		/// <param name="hideField"></param>
		/// <param name="afterField"></param>
		/// <param name="methodSource"></param>
		/// <param name="style"></param>
		public ActionButton(string buttonText,
			string methodName,
			bool hideField = false,
			bool afterField = false,
			SourceType methodSource = SourceType.Parent,
			string style = EditorStyleTypes.miniButtonMid)
		{
			this.buttonText = buttonText;
			this.methodName = methodName;
			this.hideField = hideField;
			this.afterField = afterField;
			this.methodSource = methodSource;
			this.style = style;
			// this.fullWidth = fullWidth; // TODO: Implement full width since atm it's not working (text is cut off)
		}
	}
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class FieldActionButton : ActionButton
	{
		public FieldActionButton(string buttonText,
			string methodName,
			bool hideField = false,
			bool afterField = false,
			string style = EditorStyleTypes.miniButtonMid) : base(buttonText, methodName, hideField, afterField, SourceType.Field, style)
		{
		}
	}
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class ParentActionButton : ActionButton
	{
		public ParentActionButton(string buttonText,
			string methodName,
			bool hideField = false,
			bool afterField = false,
			string style = EditorStyleTypes.miniButtonMid) : base(buttonText, methodName, hideField, afterField, SourceType.Parent, style)
		{
		}
	}
}