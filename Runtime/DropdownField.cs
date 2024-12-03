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
		/// <summary>
		/// Creates a new DropdownField attribute.
		/// </summary>
		/// <param name="options">
		/// The options to display in the dropdown menu.
		/// </param>
		public DropdownField(string[] options)
		{
			optionSource = new EditorDataSource<string[]>(options);
		}
		/// <summary>
		/// Creates a new DropdownField attribute.
		/// </summary>
		/// <param name="sourceName">
		/// The name of the method to call to get the options to display in the dropdown menu. The method should return a string array.
		/// </param>
		public DropdownField(string sourceName)
		{
			optionSource = new EditorDataSource<string[]>(sourceName);

		}

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