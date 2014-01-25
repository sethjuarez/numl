using System;
using numl.Utils;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using numl.Math.LinearAlgebra;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Linq.Expressions;
using System.Collections;

namespace numl.Model
{
    /// <summary>
    /// Fluent API addition for simplifying the process
    /// of adding features and labels to a descriptor
    /// </summary>
    public class DescriptorProperty
    {
        private readonly Descriptor _descriptor;
        private readonly string _name;
        private readonly bool _label;

        /// <summary>
        /// internal constructor used for
        /// creating chaining
        /// </summary>
        /// <param name="descriptor">descriptor</param>
        /// <param name="name">name of property</param>
        /// <param name="label">label property?</param>
        internal DescriptorProperty(Descriptor descriptor, string name, bool label)
        {
            _label = label;
            _name = name;
            _descriptor = descriptor;
        }

        /// <summary>
        /// Not ready
        /// </summary>
        /// <param name="conversion">Conversion method</param>
        /// <returns>Descriptor</returns>
        public Descriptor Use(Func<object, double> conversion)
        {
            throw new NotImplementedException("Not yet ;)");
            //return _descriptor;
        }

        /// <summary>
        /// Adds property to descriptor
        /// with chained name and type
        /// </summary>
        /// <param name="type">Property Type</param>
        /// <returns>descriptor with added property</returns>
        public Descriptor As(Type type)
        {
            Property p;
            if (_label)
                p = TypeHelpers.GenerateLabel(type, _name);
            else
                p = TypeHelpers.GenerateFeature(type, _name);
            AddProperty(p);
            return _descriptor;
        }

        /// <summary>
        /// Adds the default string property to 
        /// descriptor with previously chained name 
        /// </summary>
        /// <returns>descriptor with added property</returns>
        public Descriptor AsString()
        {
            StringProperty p = new StringProperty();
            p.Name = _name;
            p.AsEnum = _label;
            AddProperty(p);
            return _descriptor;
        }

        /// <summary>
        /// Adds string property to descriptor 
        /// with previously chained name 
        /// </summary>
        /// <param name="splitType">How to split string</param>
        /// <param name="separator">Separator to use</param>
        /// <param name="exclusions">file describing strings to exclude</param>
        /// <returns>descriptor with added property</returns>
        public Descriptor AsString(StringSplitType splitType, string separator = " ", string exclusions = null)
        {
            StringProperty p = new StringProperty();
            p.Name = _name;
            p.SplitType = splitType;
            p.Separator = separator;
            p.ImportExclusions(exclusions);
            p.AsEnum = _label;
            AddProperty(p);
            return _descriptor;
        }

        /// <summary>
        /// Adds string property to descriptor 
        /// with previously chained name 
        /// </summary>
        /// <returns>descriptor with added property</returns>
        public Descriptor AsStringEnum()
        {
            StringProperty p = new StringProperty();
            p.Name = _name;
            p.AsEnum = true;
            AddProperty(p);
            return _descriptor;
        }

        /// <summary>
        /// Adds DateTime property to descriptor
        /// with previously chained name
        /// </summary>
        /// <param name="features">Which date features to use (can pipe: DateTimeFeature.Year | DateTimeFeature.DayOfWeek)</param>
        /// <returns>descriptor with added property</returns>
        public Descriptor AsDateTime(DateTimeFeature features)
        {
            if (_label)
                throw new DescriptorException("Cannot use a DateTime property as a label");

            var p = new DateTimeProperty(features)
            {
                Discrete = true,
                Name = _name
            };

            AddProperty(p);
            return _descriptor;
        }

        /// <summary>
        /// Adds DateTime property to descriptor
        /// with previously chained name
        /// </summary>
        /// <param name="portion">Which date portions to use (can pipe: DateTimeFeature.Year | DateTimeFeature.DayOfWeek)</param>
        /// <returns>descriptor with added property</returns>
        public Descriptor AsDateTime(DatePortion portion)
        {
            if (_label)
                throw new DescriptorException("Cannot use an DateTime property as a label");

            var p = new DateTimeProperty(portion)
            {
                Discrete = true,
                Name = _name
            };

            AddProperty(p);
            return _descriptor;
        }

        /// <summary>
        /// Adds Enumerable property to descriptor
        /// with previousy chained name
        /// </summary>
        /// <param name="length">length of enumerable to expand</param>
        /// <returns>descriptor with added property</returns>
        public Descriptor AsEnumerable(int length)
        {
            if (_label)
                throw new DescriptorException("Cannot use an Enumerable property as a label");

            var p = new EnumerableProperty(length)
            {
                Name = _name,
                Discrete = false
            };

            AddProperty(p);

            return _descriptor;
        }

        private void AddProperty(Property p)
        {
            if (_label)
                _descriptor.Label = p;
            else
            {
                var features = new List<Property>(_descriptor.Features ?? new Property[] { });
                features.Add(p);
                _descriptor.Features = features.ToArray();
            }
        }
    }
}
