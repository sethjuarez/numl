// file:	Utils\TypeHelpers.cs
//
// summary:	Implements the type helpers class
using numl.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Utils
{
    /// <summary>A type helpers.</summary>
    internal class TypeHelpers
    {
        /// <summary>Generates a feature.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>The feature.</returns>
        public static Property GenerateFeature(Type type, string name)
        {
            Property p;
            if (type == typeof(string))
                p = new StringProperty();
            else if (type == typeof(DateTime))
                p = new DateTimeProperty();
            else if (type.GetInterfaces().Contains(typeof(IEnumerable)))
                throw new InvalidOperationException(
                    string.Format("Property {0} needs to be labeled as an EnumerableFeature", name));
            else
                p = new Property();


            p.Discrete = type.BaseType == typeof(Enum) ||
                         type == typeof(bool) ||
                         type == typeof(string) ||
                         type == typeof(char) ||
                         type == typeof(DateTime);

            p.Type = type;
            p.Name = name;

            return p;
        }
        /// <summary>Generates a label.</summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>The label.</returns>
        public static Property GenerateLabel(Type type, string name)
        {
            var p = GenerateFeature(type, name);
            if (p is StringProperty) // if it is a label, must be enum
                ((StringProperty)p).AsEnum = true;
            return p;
        }
    }
}
