#nullable enable
using System;
using System.Reflection;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Runtime
{
	public class EditorDataSource<T>
	{
		enum TargetType
		{
			Raw,
			Source
		}

		readonly TargetType targetType;
		readonly T rawSource;
		readonly string sourceName;


		public EditorDataSource(T rawSource)
		{
			this.rawSource = rawSource;
			targetType = TargetType.Raw;
		}

		public EditorDataSource(string sourceName)
		{
			this.sourceName = sourceName;
			targetType = TargetType.Source;
		}

		public bool Get(object obj, out T result)
		{
			result = default;
			return targetType switch{
				TargetType.Raw => GetRaw(out result),
				TargetType.Source => GetSource(obj, out result),
				_ => default
			};
		}

		bool GetRaw(out T result)
		{
			result = rawSource;
			return true;
		}

		bool GetSource(object obj, out T result)
		{
			Type type = obj.GetType();
			result = default;
			return GetMember(type, obj, out result);
		}

		bool GetMember(Type type, object obj, out T result)
		{
			FieldInfo? field = type.GetField(sourceName,
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			PropertyInfo? prop = type.GetProperty(sourceName,
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			MethodInfo? method = type.GetMethod(sourceName,
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			if (field == null && prop == null && method == null){
				Debug.LogWarning($"Could not find field/property/method with name '{sourceName}' in type '{type}'");
				result = default;
				return false;
			}

			object? resultObj = null;
			resultObj ??= field?.GetValue(obj);
			resultObj ??= prop?.GetValue(obj);
			resultObj ??= method?.Invoke(obj, null);

			if (resultObj is T validResult){
				result = validResult;
				return true;
			}
			Debug.LogWarning($"Property is not of type '{typeof(T)}'");
			result = default;
			return false;

		}

		bool GetMethodSource(Type type, object obj, out T result)
		{
			MethodInfo? method = type.GetMethod(sourceName,
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			if (method == null){
				Debug.LogWarning($"Could not find method with name '{sourceName}' in type '{type}'");
				result = default;
				return false;
			}

			object? resultObj = method.Invoke(obj, null);
			if (resultObj is T validResult){
				result = validResult;
				return true;
			}

			Debug.LogWarning($"Method does not return type '{typeof(T)}'");
			result = default;
			return false;

		}
	}
}