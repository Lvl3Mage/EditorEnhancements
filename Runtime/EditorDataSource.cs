#nullable enable
using System;
using System.Reflection;
using UnityEngine;

namespace Lvl3Mage.EditorEnhancements.Runtime
{

	/// <summary>
	/// A class that provides a way to get data from a field, property, or method in the inspector. Also supports raw data as a source.
	/// </summary>
	/// <typeparam name="T">
	/// The type of data to get.
	/// </typeparam>
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


		/// <summary>
		/// Creates a new EditorDataSource with raw data as the source.
		/// </summary>
		/// <param name="rawSource">
		/// The raw data to use as the source.
		/// </param>
		public EditorDataSource(T rawSource)
		{
			this.rawSource = rawSource;
			targetType = TargetType.Raw;
		}
		/// <summary>
		/// Creates a new EditorDataSource with a field, property, or method as the source.
		/// </summary>
		/// <param name="sourceName">
		/// The name of the field, property, or method to get the data from. The source object must be of type <typeparamref name="T"/>.
		/// </param>
		public EditorDataSource(string sourceName)
		{
			this.sourceName = sourceName;
			targetType = TargetType.Source;
		}

		/// <summary>
		/// Returns the data from the source.
		/// </summary>
		/// <param name="obj">
		/// The object to get the data from. This object must have the field, property, or method with the name specified in the constructor if the source is not raw data.
		/// </param>
		/// <param name="result">
		/// The data from the source. If the source is not found or the data is not of type <typeparamref name="T"/>, this will be the default value of <typeparamref name="T"/>.
		/// </param>
		/// <returns>
		/// True if the data was successfully retrieved, false otherwise.
		/// </returns>
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