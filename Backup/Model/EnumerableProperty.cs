// file:	Model\EnumerableProperty.cs
//
// summary:	Implements the enumerable property class
using System;
using numl.Utils;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace numl.Model
{
    /// <summary>Enumerable property. Expanded feature.</summary>
    [XmlRoot("EnumerableProperty"), Serializable]
    public class EnumerableProperty : Property
    {
        /// <summary>The length.</summary>
        private int _length;
        /// <summary>Default constructor.</summary>
        internal EnumerableProperty() { }
        /// <summary>Constructor.</summary>
        /// <param name="length">The length.</param>
        public EnumerableProperty(int length)
        {
            _length = length;
        }
        /// <summary>Length of property.</summary>
        /// <value>The length.</value>
        public override int Length
        {
            get
            {
                return _length;
            }
        }
        /// <summary>
        /// Retrieve the list of expanded columns. If there is a one-to-one correspondence between the
        /// type and its expansion it will return a single value/.
        /// </summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the columns in this collection.
        /// </returns>
        public override IEnumerable<string> GetColumns()
        {
            for (int i = 0; i < _length; i++)
                yield return i.ToString();
        }
        /// <summary>Convert the numeric representation back to the original type.</summary>
        /// <param name="val">.</param>
        /// <returns>An object.</returns>
        public override object Convert(double val)
        {
            return val;
        }
        /// <summary>Convert an object to a list of numbers.</summary>
        /// <exception cref="InvalidCastException">Thrown when an object cannot be cast to a required
        /// type.</exception>
        /// <param name="o">Object.</param>
        /// <returns>Lazy list of doubles.</returns>
        public override IEnumerable<double> Convert(object o)
        {
            // is it some sort of enumeration?
            if (o.GetType().GetInterfaces().Contains(typeof(IEnumerable)))
            {
                var a = (IEnumerable)o;
                int i = 0;
                foreach (var item in a)
                {
                    // if on first try we can't do anything, just bail;
                    // needs to be an enumeration of a simple type
                    if (i == 0 && !Ject.CanUseSimpleType(item.GetType()))
                        throw new InvalidCastException(
                           string.Format("Cannot properly cast {0} to a number", item.GetType()));

                    // check if contained item is discrete
                    if (i == 0)
                    {
                        var type = item.GetType();
                        Discrete = type.BaseType == typeof(Enum) ||
                                   type == typeof(bool) ||
                                   type == typeof(string) ||
                                   type == typeof(char);
                    }

                    yield return Ject.Convert(item);

                    // should pull no more than specified length
                    if (++i == Length)
                        break;
                }

                // pad excess with 0's
                for (int j = i + 1; i < Length; i++)
                    yield return 0;
            }
            else
                throw new InvalidCastException(
                    string.Format("Cannot cast {0} to an IEnumerable", o.GetType().Name));
        }
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("ElementType", Type == null ? "None" : Type.Name);
            writer.WriteAttributeString("Discrete", Discrete.ToString());
            writer.WriteAttributeString("Start", Start.ToString());

            writer.WriteAttributeString("Length", _length.ToString());
        }
        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Name = reader.GetAttribute("Name");
            string elementType = reader.GetAttribute("ElementType");
            if (elementType != "None")
                Type = Ject.FindType(elementType);
            Discrete = bool.Parse(reader.GetAttribute("Discrete"));
            Start = int.Parse(reader.GetAttribute("Start"));
            _length = int.Parse(reader.GetAttribute("Length"));
        }
    }
}
