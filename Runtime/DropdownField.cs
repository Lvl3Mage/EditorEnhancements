using System;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Runtime
{
	/// <summary>
	/// An attribute that adds a dropdown field to the inspector. Replaces the standard string field with a dropdown menu.
	/// </summary>
	public class DropdownField : PropertyAttribute
	{
		EditorDataSource<string[]> optionSource;
		string emptyValue;

		/// <summary>
		/// Creates a new DropdownField attribute.
		/// </summary>
		/// <param name="options">
		/// The options to display in the dropdown menu.
		/// </param>
		/// <param name="emptyValue">
		/// The value to display when options are empty.
		/// </param>
		public DropdownField(string[] options, string emptyValue = "none")
		{
			optionSource = new EditorDataSource<string[]>(options);
			this.emptyValue = emptyValue;
		}

		/// <summary>
		/// Creates a new DropdownField attribute.
		/// </summary>
		/// <param name="sourceName">
		/// The name of the method to call to get the options to display in the dropdown menu. The method should return a string array.
		/// </param>
		/// <param name="emptyValue">
		/// The value to display when options are empty.
		/// </param>
		public DropdownField(string sourceName, string emptyValue = "none")
		{
			optionSource = new EditorDataSource<string[]>(sourceName);
			this.emptyValue = emptyValue;

		}
		public string EmptyValue => emptyValue;

		public string[] GetOptions(object? obj)
		{
			if(obj == null){
				return Array.Empty<string>();
			}
			if (optionSource.Get(obj, out string[] result)){
				return result;
			}
			return Array.Empty<string>();
		}
	}
}