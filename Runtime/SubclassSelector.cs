using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Runtime
{
	public class SubclassSelector : PropertyAttribute
	{
		public string FieldName { get; protected set; }
		Type[] ExcludedTypes { get; }

		public SubclassSelector(string fieldName = "Selected Type", params Type[] excludedTypes)
		{
			FieldName = fieldName;
			excludedTypes ??= Type.EmptyTypes;
			ExcludedTypes = excludedTypes;
		}
		public virtual Type[] GetTypes(Type baseType)
		{
			return GetAllSubtypes(baseType).ToArray();
		}

		IEnumerable<Type> GetAllSubtypes(Type T)
		{
			IEnumerable<Type> types = T.IsInterface
				? T.Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(T))
				: T.Assembly.GetTypes().Where(t => t.IsSubclassOf(T));

			return types.Append(T)
				.Except(ExcludedTypes)
				.Where(t => !t.IsAbstract && !t.IsInterface && !t.IsSubclassOf(typeof(UnityEngine.Object)));
		}
	}
	public class CustomSubclassSelector : SubclassSelector
	{
		Type[] CustomTypes { get; }

		public CustomSubclassSelector(string fieldName = "Selected Type", params Type[] customTypes)
			: base(fieldName)
		{
			CustomTypes = customTypes;
		}


		public override Type[] GetTypes(Type baseType)
		{
			return CustomTypes;
		}
	}
}