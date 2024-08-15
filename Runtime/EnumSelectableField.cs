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
}