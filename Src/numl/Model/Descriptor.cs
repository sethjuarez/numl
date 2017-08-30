// file:	Model\Descriptor.cs
//
// summary:	Implements the descriptor class
using System;
using System.IO;
using numl.Utils;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using numl.Math.LinearAlgebra;
using System.Linq.Expressions;
using System.Collections.Generic;
using numl.Serialization;

namespace numl.Model
{
	/// <summary>
	/// This class is designed to describe the underlying types that will be used in the machine
	/// learning process. Any machine learning process requires a set of <see cref="Features"/> that
	/// will be used to discriminate the <see cref="Label"/>. The <see cref="Label"/> itself is the
	/// target element that the machine learning algorithms learn to predict.
	/// </summary>
	public class Descriptor
	{
		/// <summary>Default constructor.</summary>
		public Descriptor()
		{
			Name = "";
			Features = new Property[] { };
		}

		/// <summary>Descriptor name.</summary>
		/// <value>The name.</value>
		public string Name { get; set; }
		/// <summary>
		/// Set of features used to discriminate or learn about the <see cref="Label"/>.
		/// </summary>
		/// <value>The features.</value>
		public Property[] Features { get; set; }
		/// <summary>Target property that is the target of machine learning.</summary>
		/// <value>The label.</value>
		public Property Label { get; set; }
		/// <summary>Index into features (for convenience)</summary>
		/// <param name="i">Feature index.</param>
		/// <returns>Feature Property.</returns>
		public Property this[int i]
		{
			get
			{
				if (i >= Features.Length) throw new IndexOutOfRangeException();
				else return Features[i];
			}
		}
		/// <summary>Index intor features (for convenience)</summary>
		/// <param name="name">Feature name.</param>
		/// <returns>Feature Property.</returns>
		public Property this[string name]
		{
			get
			{
				if (Features.Where(p => p.Name == name).Count() == 1)
					return Features.Where(p => p.Name == name).First();
				else
					return null;
			}
		}
		/// <summary>
		/// Available column names used to discriminate or learn about <see cref="Label"/>. The number of
		/// columns does not necessarily equal the number of <see cref="Features"/> given that there
		/// might exist multi-valued features.
		/// </summary>
		/// <returns>
		/// An enumerator that allows foreach to be used to process the columns in this collection.
		/// </returns>
		public IEnumerable<string> GetColumns()
		{
			foreach (var p in Features)
				foreach (var s in p.GetColumns())
					yield return s;
		}

		/// <summary>Length of the vector.</summary>
		private int _vectorLength = -1;
		/// <summary>
		/// Total feature count The number of features does not necessarily equal the number of
		/// <see cref="Features"/> given that there might exist multi-valued features.
		/// </summary>
		/// <value>The length of the vector.</value>
		public int VectorLength
		{
			get
			{
				if (_vectorLength < 0)
					_vectorLength = Features.Select(f => f.Length).Sum();

				return _vectorLength;
			}
		}
		/// <summary>Base type of object being described. This could also be null.</summary>
		/// <value>The type.</value>
		public Type Type { get; set; }
		/// <summary>Gets related property given its offset within the vector representation.</summary>
		/// <exception cref="IndexOutOfRangeException">Thrown when the index is outside the required
		/// range.</exception>
		/// <param name="i">Vector Index.</param>
		/// <returns>Associated Feature.</returns>
		public Property At(int i)
		{
			if (i < 0 || i > VectorLength)
				throw new IndexOutOfRangeException(string.Format("{0} falls outside of the appropriate range", i));

			var q = (from p in Features
					 where i >= p.Start && i < p.Start + p.Length
					 select p);

			return q.First();
		}
		/// <summary>
		/// Returns the raw value from the object for the supplied property.
		/// </summary>
		/// <param name="o">Object to process.</param>
		/// <param name="property">Property value of the object to return.</param>
		/// <returns></returns>
		public object GetValue(object o, Property property)
		{
			return Ject.Get(o, property.Name);
		}
		/// <summary>
		/// Gets related property column name given its offset within the vector representation.
		/// </summary>
		/// <param name="i">Vector Index.</param>
		/// <returns>Associated Property Name.</returns>
		public string ColumnAt(int i)
		{
			var prop = At(i);
			var offset = i - prop.Start;
			var col = prop.GetColumns().ElementAt(offset);
			return col;
		}
		/// <summary>
		/// Converts a given example into a lazy list of doubles in preparation for vector conversion
		/// (both features and corresponding label)
		/// </summary>
		/// <param name="item">Example.</param>
		/// <returns>Lazy List of doubles.</returns>
		public IEnumerable<double> Convert(object item)
		{
			return Convert(item, true);
		}
		/// <summary>
		/// Converts a given example into a lazy list of doubles in preparation for vector conversion.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="item">Example.</param>
		/// <param name="withLabel">Should convert label as well.</param>
		/// <returns>Lazy List of doubles.</returns>
		public IEnumerable<double> Convert(object item, bool withLabel)
		{
			if (Features.Length == 0)
				throw new InvalidOperationException("Cannot convert item with an empty Feature set.");

			for (int i = 0; i < Features.Length; i++)
			{
				// current feature
				var feature = Features[i];

				// pre-process item
				feature.PreProcess(item);

				// start position
				if (feature.Start < 0)
					feature.Start = i == 0 ? 0 : Features[i - 1].Start + Features[i - 1].Length;

				// retrieve item
				var o = Ject.Get(item, feature.Name);

				// convert item
				foreach (double val in feature.Convert(o))
					yield return val;

				// post-process item
				feature.PostProcess(item);
			}

			// convert label (if available)
			if (Label != null && withLabel)
				foreach (double val in Label.Convert(Ject.Get(item, Label.Name)))
					yield return val;

		}
		/// <summary>Converts a list of examples into a lazy double list of doubles.</summary>
		/// <param name="items">Examples.</param>
		/// <param name="withLabels">True to include labels, otherwise False</param>
		/// <returns>Lazy double enumerable of doubles.</returns>
		public IEnumerable<IEnumerable<double>> Convert(IEnumerable<object> items, bool withLabels = true)
		{
			// Pre processing items
			foreach (Property feature in Features)
				feature.PreProcess(items);

			if (Label != null)
				Label.PreProcess(items);

			// convert items
			foreach (object o in items)
				yield return Convert(o, withLabels);

			// Post processing items
			foreach (Property feature in Features)
				feature.PostProcess(items);

			if (Label != null)
				Label.PostProcess(items);
		}
		/// <summary>Converts a list of examples into a Matrix/Vector tuple.</summary>
		/// <param name="examples">Examples.</param>
		/// <returns>Tuple containing Matrix and Vector.</returns>
		public (Matrix X, Vector Y) ToExamples(IEnumerable<object> examples)
		{
			return Convert(examples).ToExamples();
		}
		/// <summary>Converts a list of examples into a Matrix.</summary>
		/// <param name="examples">Examples.</param>
		/// <returns>Matrix representation.</returns>
		public Matrix ToMatrix(IEnumerable<object> examples)
		{
			return Convert(examples).ToMatrix();
		}
		/// <summary>
		/// Convert an object to its vector representation based on the descriptor properties.
		/// </summary>
		/// <param name="item">object to convert.</param>
		/// <returns>Vector representation.</returns>
		public Vector ToVector(object item)
		{
			return Convert(item).ToVector();
		}
		/// <summary>Pretty printed descriptor.</summary>
		/// <returns>Pretty printed string.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"Descriptor ({Type?.Name ?? Name}) {{");
			for (int i = 0; i < Features.Length; i++)
				sb.AppendLine($"   {Features[i]}");
			if (Label != null)
				sb.AppendLine($"  *{Label}");

			sb.AppendLine("}");
			return sb.ToString();
		}

		//---- Creational

		/// <summary>
		/// Creates a descriptor based upon a marked up concrete class.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>Descriptor.</returns>
		public static Descriptor Create<T>()
			where T : class
		{

			return Create(typeof(T));
		}
		/// <summary>Creates a descriptor based upon a marked up concrete type.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="t">Class Type.</param>
		/// <returns>Descriptor.</returns>
		public static Descriptor Create(Type t)
		{
			if (!t.GetTypeInfo().IsClass)
				throw new InvalidOperationException("Can only work with class types");

			List<Property> features = new List<Property>();
			Property label = null;

			foreach (PropertyInfo property in t.GetProperties())
			{
				var item = property.GetCustomAttributes(typeof(NumlAttribute), false);

				if (item.Count() == 1)
				{
					var attrib = (NumlAttribute)item.First();

					// generate appropriate property from attribute
					Property p = attrib.GenerateProperty(property);

					// feature
					if (attrib.GetType().GetTypeInfo().IsSubclassOf(typeof(FeatureAttribute)) ||
						attrib is FeatureAttribute)
						features.Add(p);
					// label
					else if (attrib.GetType().GetTypeInfo().IsSubclassOf(typeof(LabelAttribute)) ||
						attrib is LabelAttribute)
					{
						if (label != null)
							throw new InvalidOperationException("Cannot have multiple labels in a class");
						label = p;
					}
				}
			}

			return new Descriptor
			{
				Features = features.ToArray(),
				Label = label,
				Type = t,
			};
		}

		/// <summary>
		/// Creates a new descriptor using a fluent approach. This initial descriptor is worthless
		/// without adding features.
		/// </summary>
		/// <returns>Empty Descriptor.</returns>
		public static Descriptor New()
		{
			return new Descriptor() { Features = new Property[] { } };
		}
		/// <summary>
		/// Creates a new descriptor using a fluent approach. This initial descriptor is worthless
		/// without adding features.
		/// </summary>
		/// <param name="name">Desired name.</param>
		/// <returns>Empty Named Descriptor.</returns>
		public static Descriptor New(string name)
		{
			return new Descriptor() { Name = name, Features = new Property[] { } };
		}
		/// <summary>
		/// Creates a new descriptor using a fluent approach. This initial descriptor is worthless
		/// without adding features.
		/// </summary>
		/// <param name="type">Type mapping.</param>
		/// <returns>A Descriptor.</returns>
		public static Descriptor New(Type type)
		{
			return new Descriptor() { Type = type, Features = new Property[] { } };
		}

		/// <summary>
		/// Creates a new descriptor using a strongly typed fluent approach. This initial descriptor is
		/// worthless without adding features.
		/// </summary>
		/// <typeparam name="T">Source Object Type</typeparam>
		/// <returns>Empty descriptor</returns>
		public static Descriptor<T> For<T>()
		{
			return new Descriptor<T>() { Type = typeof(T), Features = new Property[] { } };
		}



		/// <summary>
		/// Creates a new descriptor using a strongly typed fluent approach. This initial descriptor is
		/// worthless without adding features.
		/// </summary>
		/// <typeparam name="T">Source Object Type</typeparam>
		/// <param name="name">Desired Descriptor Name.</param>
		/// <returns>Empty descriptor</returns>
		public static Descriptor<T> For<T>(string name)
		{
			return new Descriptor<T>() { Name = name, Type = typeof(T), Features = new Property[] { } };
		}

		/// <summary>
		/// Load a descriptor from a stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <returns>Descriptor.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public static Descriptor Load(Stream stream)
		{
			throw new NotImplementedException();
		}
		/// <summary>Adds a new feature to descriptor.</summary>
		/// <param name="name">Name of feature (must match property name or dictionary key)</param>
		/// <returns>method for describing feature.</returns>
		public DescriptorProperty With(string name)
		{
			return new DescriptorProperty(this, name, false);
		}
		/// <summary>Adds (or replaces) a label to the descriptor.</summary>
		/// <param name="name">Name of label (must match property name or dictionary key)</param>
		/// <returns>A DescriptorProperty.</returns>
		public DescriptorProperty Learn(string name)
		{
			return new DescriptorProperty(this, name, true);
		}

		/// <summary>
		/// Equality test
		/// </summary>
		/// <param name="obj">object to compare</param>
		/// <returns>equality</returns>
		public override bool Equals(object obj)
		{
			if (obj is Descriptor)
			{
				var d = obj as Descriptor;
				if (Features.Length == d.Features.Length)
				{
					for (int i = 0; i < Features.Length; i++)
						if (!Features[i].Equals(d.Features[i]))
							return false;

					if ((Label != null &&
						d.Label != null))
						return Label.Equals(d.Label) &&
							   Name == d.Name &&
							   Type == d.Type &&
							   VectorLength == d.VectorLength &&
							   Features.Length == d.Features.Length;
					else
						return Label == null &&
							   d.Label == null &&
							   Name == d.Name &&
							   Type == d.Type &&
							   VectorLength == d.VectorLength &&
							   Features.Length == d.Features.Length;
				}
			}
			return false;
		}

		/// <summary>
		/// Return hash
		/// </summary>
		/// <returns>hash</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}


	/// <summary>
	/// Class Descriptor.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Descriptor<T> : Descriptor
	{
		/// <summary>Initializes a new instance of the Descriptor class.</summary>
		public Descriptor()
		{
			Type = typeof(T);
			Features = new Property[] { };
		}
		/// <summary>Adds a property to 'label'.</summary>
		/// <param name="p">The Property to process.</param>
		/// <param name="label">true to label.</param>
		private void AddProperty(Property p, bool label)
		{
			if (label)
				Label = p;
			else
			{
				var features = new List<Property>(Features ?? new Property[] { });
				features.Add(p);
				Features = features.ToArray();
			}
		}
		/// <summary>Gets property information.</summary>
		/// <tparam name="K">Generic type parameter.</tparam>
		/// <param name="property">The property.</param>
		/// <returns>The property information.</returns>
		private static PropertyInfo GetPropertyInfo<K>(Expression<Func<T, K>> property)
		{
			PropertyInfo propertyInfo = null;
			if (property.Body is MemberExpression)
				propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;
			else
				propertyInfo = (((UnaryExpression)property.Body).Operand as MemberExpression).Member as PropertyInfo;

			return propertyInfo;
		}
		/// <summary>Withs the given property.</summary>
		/// <param name="property">The property.</param>
		/// <returns>A Descriptor&lt;T&gt;</returns>
		public Descriptor<T> With(Expression<Func<T, Object>> property)
		{
			var pi = GetPropertyInfo<Object>(property);
			AddProperty(TypeHelpers.GenerateFeature(pi.PropertyType, pi.Name), false);
			return this;
		}
		/// <summary>With string.</summary>
		/// <param name="property">The property.</param>
		/// <param name="splitType">Type of the split.</param>
		/// <param name="separator">(Optional) the separator.</param>
		/// <param name="asEnum">(Optional) true to as enum.</param>
		/// <param name="exclusions">(Optional) base 64 content string of the exclusions.</param>
		/// <returns>A Descriptor&lt;T&gt;</returns>
		public Descriptor<T> WithString(Expression<Func<T, string>> property, StringSplitType splitType, string separator = " ", bool asEnum = false, string exclusions = null)
		{
			var pi = GetPropertyInfo<string>(property);
			StringProperty p = new StringProperty();
			p.Name = pi.Name;
			p.SplitType = splitType;
			p.Separator = separator;
			p.ImportExclusions(exclusions);
			p.AsEnum = asEnum;
			AddProperty(p, false);
			return this;
		}
		/// <summary>With date time.</summary>
		/// <param name="property">The property.</param>
		/// <param name="features">The features.</param>
		/// <returns>A Descriptor&lt;T&gt;</returns>
		public Descriptor<T> WithDateTime(Expression<Func<T, DateTime>> property, DateTimeFeature features)
		{
			var pi = GetPropertyInfo<DateTime>(property);
			var p = new DateTimeProperty(features)
			{
				Discrete = true,
				Name = pi.Name
			};

			AddProperty(p, false);
			return this;
		}
		/// <summary>With date time.</summary>
		/// <param name="property">The property.</param>
		/// <param name="portion">The portion.</param>
		/// <returns>A Descriptor&lt;T&gt;</returns>
		public Descriptor<T> WithDateTime(Expression<Func<T, DateTime>> property, DatePortion portion)
		{
			var pi = GetPropertyInfo<DateTime>(property);
			var p = new DateTimeProperty(portion)
			{
				Discrete = true,
				Name = pi.Name
			};

			AddProperty(p, false);
			return this;
		}
		/// <summary>With guid.</summary>
		/// <param name="property">The property.</param>
		/// <returns>A Descriptor&lt;T&gt;</returns>
		public Descriptor<T> WithGuid(Expression<Func<T, Guid>> property)
		{
			var pi = GetPropertyInfo<Guid>(property);
			var p = new GuidProperty()
			{
				Discrete = true,
				Name = pi.Name
			};

			AddProperty(p, false);
			return this;
		}
		/// <summary>With enumerable.</summary>
		/// <param name="property">The property.</param>
		/// <param name="length">The length.</param>
		/// <returns>A Descriptor&lt;T&gt;</returns>
		public Descriptor<T> WithEnumerable(Expression<Func<T, IEnumerable>> property, int length)
		{
			var pi = GetPropertyInfo<IEnumerable>(property);
			var p = new EnumerableProperty(length)
			{
				Name = pi.Name,
				Discrete = false
			};

			AddProperty(p, false);
			return this;
		}
		/// <summary>Learns the given property.</summary>
		/// <param name="property">The property.</param>
		/// <returns>A Descriptor&lt;T&gt;</returns>
		public Descriptor<T> Learn(Expression<Func<T, Object>> property)
		{
			var pi = GetPropertyInfo(property);
			AddProperty(TypeHelpers.GenerateLabel(pi.PropertyType, pi.Name), true);
			return this;
		}
	}
}