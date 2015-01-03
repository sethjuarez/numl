// file:	Model\NumlAttributes.cs
//
// summary:	Implements the numl attributes class
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using numl.Utils;

namespace numl.Model
{
    /// <summary>Attribute for numl.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class NumlAttribute : Attribute 
    {
        /// <summary>Generates a property.</summary>
        /// <param name="property">The property.</param>
        /// <returns>The property.</returns>
        public virtual Property GenerateProperty(PropertyInfo property)
        {
            return TypeHelpers.GenerateFeature(property.PropertyType, property.Name);
        }
    }

    /// <summary>Attribute for feature.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FeatureAttribute : NumlAttribute { }

    /// <summary>Attribute for label.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LabelAttribute : NumlAttribute 
    {
        /// <summary>Generates a property.</summary>
        /// <param name="property">The property.</param>
        /// <returns>The property.</returns>
        public override Property GenerateProperty(PropertyInfo property)
        {
            return TypeHelpers.GenerateLabel(property.PropertyType, property.Name);
        }
    }

    /// <summary>Attribute for string feature.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringFeatureAttribute : FeatureAttribute
    {
        /// <summary>Gets or sets the type of the split.</summary>
        /// <value>The type of the split.</value>
        public StringSplitType SplitType { get; set; }
        /// <summary>Gets or sets the separator.</summary>
        /// <value>The separator.</value>
        public string Separator { get; set; }
        /// <summary>Gets or sets the exclusion file.</summary>
        /// <value>The exclusion file.</value>
        public string ExclusionFile { get; set; }
        /// <summary>Gets or sets a value indicating whether as enum.</summary>
        /// <value>true if as enum, false if not.</value>
        public bool AsEnum { get; set; }

        /// <summary>Default constructor.</summary>
        public StringFeatureAttribute()
        {
            AsEnum = false;
            SplitType = StringSplitType.Word;
            Separator = " ";
        }
        /// <summary>Constructor.</summary>
        /// <param name="splitType">Type of the split.</param>
        /// <param name="separator">(Optional) the separator.</param>
        /// <param name="exclusions">(Optional) the exclusions.</param>
        public StringFeatureAttribute(StringSplitType splitType, string separator = " ", string exclusions = null)
        {
            SplitType = splitType;
            Separator = separator;
            ExclusionFile = exclusions;
        }
        /// <summary>Constructor.</summary>
        /// <param name="asEnum">true to as enum.</param>
        public StringFeatureAttribute(bool asEnum)
        {
            AsEnum = asEnum;
        }
        /// <summary>Generates a property.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="property">The property.</param>
        /// <returns>The property.</returns>
        public override Property GenerateProperty(PropertyInfo property)
        {
            if (property.PropertyType != typeof(string))
                throw new InvalidOperationException("Must use a string property.");

            var sp = new StringProperty
            {
                Name = property.Name,
                SplitType = SplitType,
                Separator = Separator,
                AsEnum = AsEnum,
                Discrete = true
            };

            sp.ImportExclusions(ExclusionFile);

            return sp;
        }
    }

    /// <summary>Attribute for string label.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringLabelAttribute : LabelAttribute { }

    /// <summary>Attribute for date feature.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateFeatureAttribute : FeatureAttribute
    {
        /// <summary>The dp.</summary>
        DateTimeProperty dp;
        /// <summary>Constructor.</summary>
        /// <param name="features">The features.</param>
        public DateFeatureAttribute(DateTimeFeature features)
        {
            dp = new DateTimeProperty(features);
        }
        /// <summary>Constructor.</summary>
        /// <param name="portion">The portion.</param>
        public DateFeatureAttribute(DatePortion portion)
        {
            dp = new DateTimeProperty(portion);
        }
        /// <summary>Generates a property.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="property">The property.</param>
        /// <returns>The property.</returns>
        public override Property GenerateProperty(PropertyInfo property)
        {
            if (property.PropertyType != typeof(DateTime))
                throw new InvalidOperationException("Invalid datetime property.");

            dp.Discrete = true;
            dp.Name = property.Name;
            return dp;
        }
    }

    /// <summary>Attribute for enumerable feature.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EnumerableFeatureAttribute : FeatureAttribute
    {
        /// <summary>The length.</summary>
        private readonly int _length;
        /// <summary>Constructor.</summary>
        /// <param name="length">The length.</param>
        public EnumerableFeatureAttribute(int length)
        {
            _length = length;
        }
        /// <summary>Generates a property.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="property">The property.</param>
        /// <returns>The property.</returns>
        public override Property GenerateProperty(PropertyInfo property)
        {
            if (!property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                throw new InvalidOperationException("Invalid Enumerable type.");

            if (_length <= 0)
                throw new InvalidOperationException("Cannot have an enumerable feature of 0 or less.");

            Type type = property.PropertyType;
            var ep = new EnumerableProperty(_length);
            // good assumption??
            ep.Discrete = type.BaseType == typeof(Enum) ||
                          type == typeof(bool) ||
                          type == typeof(char);
            ep.Name = property.Name;
            
            ep.Type = type.GetElementType();
            return ep;
        }
    }
}
