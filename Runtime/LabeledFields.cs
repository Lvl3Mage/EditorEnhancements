#nullable enable
using UnityEngine;
using System;
using System.Reflection;
using Lvl3Mage.EditorDevToolkit.Runtime;

namespace Lvl3Mage.EditorEnhancements.Runtime
{
	
	/// <summary>
	/// An interface that defines a labeled field.
	/// </summary>
	public interface ISourceLabeledField
	{
		/// <summary>
		/// The configuration for the labeled field
		/// </summary>
		bool UnderProperty { get; }
		bool HideProperty { get; }
		SourceType LabelSource { get; }
		string Style { get;}

		/// <summary>
		/// Gets a label for a field.
		/// </summary>
		/// <param name="type">
		/// The type source of the label
		/// </param>
		/// <param name="obj">
		/// The source object of the label
		/// </param>
		/// <returns>
		/// The label to display. If null, the label should not be shown
		/// </returns>
		public string? GetLabel(Type type, object? obj);
	}
	
	
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class PropertySourceLabeledField : PropertyAttribute, ISourceLabeledField
	{
		public bool UnderProperty { get; }
		public bool HideProperty { get; }
		public SourceType LabelSource { get; }
		public string Style { get; }
		readonly string sourceName;
		readonly string format;


		public PropertySourceLabeledField(string sourceName,
			string style = EditorStyleTypes.WhiteLargeLabel,
			string format = "{0}",
			bool underProperty = false,
			bool hideProperty = false,
			SourceType sourceType = SourceType.Parent)
		{
			this.sourceName = sourceName;
			this.format = format;
			Style = style;
			UnderProperty = underProperty;
			HideProperty = hideProperty;
			LabelSource = sourceType;
		}


		public string? GetLabel(Type type, object? obj)
		{
			if (obj == null){
				return null;
			}
			FieldInfo? field = type.GetField(sourceName,BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			PropertyInfo? prop = type.GetProperty(sourceName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			
			if (field == null && prop == null){
				Debug.LogError($"Could not find field or property with name '{sourceName}' in type '{type}'");
				return null;
			}
			object? result = field != null ? field.GetValue(obj) : prop?.GetValue(obj);
			string name = result == null ? null : result.ToString();
			
			return string.Format(format, name);
		}
	}
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class MethodSourceLabeledField : PropertyAttribute, ISourceLabeledField
	{
		readonly object[] parameters;
		readonly string sourceName;
		readonly string format;
		public bool UnderProperty { get; }
		public bool HideProperty { get; }
		public SourceType LabelSource { get; }
		public string Style { get; }


		public MethodSourceLabeledField(string sourceName,
			object[] parameters = null,
			string style = EditorStyleTypes.WhiteLargeLabel,
			string format = "{0}",
			bool underProperty = false,
			bool hideProperty = false,
			SourceType sourceType = SourceType.Parent)
		{
			this.sourceName = sourceName;
			this.parameters = parameters;
			this.format = format;
			Style = style;
			UnderProperty = underProperty;
			HideProperty = hideProperty;
			LabelSource = sourceType;
		}

		public string? GetLabel(Type type, object? obj)
		{
			if(obj == null){
				return null;
			}
			MethodInfo? method = type.GetMethod(sourceName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			if (method == null){
				Debug.LogError($"Could not find method with name '{sourceName}' in type '{type}'");
				return null;
			}
			object result = method.Invoke(obj, parameters);
			string name = result == null ? "" : result.ToString();
			return string.Format(format, name);
		}
	}
}