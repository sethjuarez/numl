using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using numl.Utils;

namespace numl.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class NumlAttribute : Attribute 
    {
        public virtual Property GenerateProperty(PropertyInfo property)
        {
            return TypeHelpers.GenerateFeature(property.PropertyType, property.Name);
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FeatureAttribute : NumlAttribute { }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LabelAttribute : NumlAttribute 
    {
        public override Property GenerateProperty(PropertyInfo property)
        {
            return TypeHelpers.GenerateLabel(property.PropertyType, property.Name);
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringFeatureAttribute : FeatureAttribute
    {
        public StringSplitType SplitType { get; set; }
        public string Separator { get; set; }
        public string ExclusionFile { get; set; }
        public bool AsEnum { get; set; }

        public StringFeatureAttribute()
        {
            AsEnum = false;
            SplitType = StringSplitType.Word;
            Separator = " ";
        }

        public StringFeatureAttribute(StringSplitType splitType, string separator = " ", string exclusions = null)
        {
            SplitType = splitType;
            Separator = separator;
            ExclusionFile = exclusions;
        }

        public StringFeatureAttribute(bool asEnum)
        {
            AsEnum = asEnum;
        }

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

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringLabelAttribute : LabelAttribute { }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateFeatureAttribute : FeatureAttribute
    {
        DateTimeProperty dp;
        public DateFeatureAttribute(DateTimeFeature features)
        {
            dp = new DateTimeProperty(features);
        }

        public DateFeatureAttribute(DatePortion portion)
        {
            dp = new DateTimeProperty(portion);
        }

        public override Property GenerateProperty(PropertyInfo property)
        {
            if (property.PropertyType != typeof(DateTime))
                throw new InvalidOperationException("Invalid datetime property.");

            dp.Discrete = true;
            dp.Name = property.Name;
            return dp;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EnumerableFeatureAttribute : FeatureAttribute
    {
        private readonly int _length;
        public EnumerableFeatureAttribute(int length)
        {
            _length = length;
        }

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
