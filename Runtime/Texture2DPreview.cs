#nullable enable
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Runtime
{
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
}