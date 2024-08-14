using UnityEngine;
using System;
using System.Linq;

namespace Lvl3Mage.EditorEnhancements.Runtime
{
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
	public class EnumSelectableField : PropertyAttribute
	{
		public readonly string enumName;
		public readonly int[] enumTargetValues;
		public EnumSelectableField(string enumName, int enumTargetValue){
			this.enumName = enumName;
			this.enumTargetValues = new[]{enumTargetValue};
		}
		public EnumSelectableField(string enumName, int[] enumTargetValues){
			this.enumName = enumName;
			this.enumTargetValues = enumTargetValues;
		}
		public bool CheckCondition(int enumValue){
			return enumTargetValues.Contains(enumValue);
		}
	}
	
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
	public class Texture2DPreview : PropertyAttribute
	{
		public readonly string labelText;
		public readonly float previewWidth;
		public readonly bool hideProperty;
		public readonly bool showDropdown;
		public readonly string updateMethodName;
		
		public Texture2DPreview(bool hideProperty, bool showDropdown = false, string text = "Preview:", float width = -1, string updateMethodName = ""){
			this.hideProperty = hideProperty;
			this.showDropdown = showDropdown;
			this.labelText = text;
			this.previewWidth = width;
			this.updateMethodName = updateMethodName;
		}
		public float GetPreviewWidth()
		{
			return previewWidth < 0 ? 70 : previewWidth;
		}
	}
	
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
	public class ActionButton : PropertyAttribute
	{
		public readonly string buttonText;
		public readonly string methodName;
		public readonly bool hideProperty;
		public readonly bool afterProperty;

		public ActionButton(string buttonText, string methodName, bool hideProperty = false, bool afterProperty = false){
			this.buttonText = buttonText;
			this.methodName = methodName;
			this.hideProperty = hideProperty;
			this.afterProperty = afterProperty;

		}
	}
}