#nullable enable
using System.Linq;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Runtime
{
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
	public class EnumSelectableField : PropertyAttribute
	{
		public readonly string enumName;
		public readonly int[] enumTargetValues;

		/// <summary>
		/// Creates a new EnumSelectableField attribute.
		/// </summary>
		/// <param name="enumName">
		/// The name of the enum field to check.
		/// </param>
		/// <param name="enumTargetValue">
		/// The value of the enum field that will allow the field to be displayed.
		/// </param>
		public EnumSelectableField(string enumName, int enumTargetValue){
			this.enumName = enumName;
			this.enumTargetValues = new[]{enumTargetValue};
		}

		/// <summary>
		/// Creates a new EnumSelectableField attribute.
		/// </summary>
		/// <param name="enumName">
		/// The name of the enum field to check.
		/// </param>
		/// <param name="enumTargetValues">
		/// The values of the enum field that will allow the field to be displayed.
		/// </param>
		public EnumSelectableField(string enumName, int[] enumTargetValues){
			this.enumName = enumName;
			this.enumTargetValues = enumTargetValues;
		}
		public bool CheckCondition(int enumValue){
			return enumTargetValues.Contains(enumValue);
		}
	}
}