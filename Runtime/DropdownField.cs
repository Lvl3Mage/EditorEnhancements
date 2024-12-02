using System;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Runtime
{
	public class DropdownField : PropertyAttribute
	{
		EditorDataSource<string[]> optionSource;
		public DropdownField(string[] options)
		{
			optionSource = new EditorDataSource<string[]>(options);
		}
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