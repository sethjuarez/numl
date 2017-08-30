// file:	Utils\TypeHelpers.cs
//
// summary:	Implements the type helpers class
using System;
using numl.Model;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace numl.Utils
{
	/// <summary>A type helpers.</summary>
	public static class TypeHelpers
	{
		/// <summary>Generates a feature.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="type">The type.</param>
		/// <param name="name">The name.</param>
		/// <returns>The feature.</returns>
		public static Property GenerateFeature(this Type type, string name)
		{
			Property p;
			if (type == typeof(string))
				p = new StringProperty();
			else if (type == typeof(DateTime))
				p = new DateTimeProperty();
			else if (type == typeof(Guid))
				p = new GuidProperty();
			else if (type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IEnumerable)))
				throw new InvalidOperationException(
					string.Format("Property {0} needs to be labeled as an EnumerableFeature", name));
			else
				p = new Property();

			p.Discrete = type.GetTypeInfo().BaseType == typeof(Enum) ||
						 type == typeof(bool) ||
						 type == typeof(string) ||
						 type == typeof(Guid) ||
						 type == typeof(char) ||
						 type == typeof(DateTime);

			p.Type = type;
			p.Name = name;

			return p;
		}

		/// <summary>
		/// Returns the first property of the given type from the Properties source.
		/// </summary>
		/// <param name="properties">Properties source.</param>
		/// <returns>Property</returns>
		public static Property GetPropertyOfType<T>(this IEnumerable<Property> properties)
		{
			return properties.FirstOrDefault(f => f.Type.IsAssignableFrom(typeof(T)));
		}

		/// <summary>Generates a label.</summary>
		/// <param name="type">The type.</param>
		/// <param name="name">The name.</param>
		/// <returns>The label.</returns>
		public static Property GenerateLabel(this Type type, string name)
		{
			var p = GenerateFeature(type, name);
			if (p is StringProperty) // if it is a label, must be enum
				((StringProperty) p).AsEnum = true;
			return p;
		}

		/// <summary>
		/// Returns a value indicating whether the type is an inbuilt runtime value type, i.e. string, number, DateTime, timespan.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="excludeTypes">Array of types to exclude when checking the type.</param>
		/// <returns></returns>
		public static bool IsSimpleType(this Type t, params Type[] excludeTypes)
		{
			return ((excludeTypes != null ? !excludeTypes.Contains(t) : true) &&
				   (t == typeof(string) ||
					t == typeof(bool) ||
					t == typeof(char) ||
					t.GetTypeInfo().BaseType == typeof(Enum) ||
					t == typeof(DateTime) ||
					t == typeof(TimeSpan) ||
					t == typeof(int) ||
					t == typeof(double) ||
					t == typeof(Guid) ||
					t == typeof(decimal) ||
					t == typeof(byte) ||
					t == typeof(sbyte) ||
					t == typeof(Single) ||
					t == typeof(Int16) ||
					t == typeof(UInt16) ||
					t == typeof(UInt32) ||
					t == typeof(Int64) ||
					t == typeof(UInt64)));
		}

		/// <summary>
		/// Creates a default instance using a parameterless constructor of the given type.
		/// </summary>
		/// <typeparam name="T">Expected type of the object to be returned.</typeparam>
		/// <param name="type">The underlying Type.</param>
		/// <returns>Instantiated object of type <typeparamref name="T"/></returns>
		public static T CreateDefault<T>(this Type type) where T : class
		{
			if (type == null) throw new ArgumentNullException("The supplied type parameter was not specified");

			return (T) Activator.CreateInstance(type) ?? default(T);
		}
	}
}
