// file:	Model\EnumerableProperty.cs
//
// summary:	Implements the enumerable property class
using System;
using numl.Utils;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace numl.Model
{
    /// <summary>Enumerable property. Expanded feature.</summary>
    public class EnumerableProperty : Property
    {
        /// <summary>Default constructor.</summary>
        public EnumerableProperty() { }
        /// <summary>Constructor.</summary>
        /// <param name="length">The length.</param>
        public EnumerableProperty(int length)
        {
            Length = length;
        }
        /// <summary>Length of property.</summary>
        /// <value>The length.</value>
        public override int Length { get; set; }

        /// <summary>
        /// Retrieve the list of expanded columns. If there is a one-to-one correspondence between the
        /// type and its expansion it will return a single value/.
        /// </summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the columns in this collection.
        /// </returns>
        public override IEnumerable<string> GetColumns()
        {
            for (int i = 0; i < Length; i++)
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
            if (o is IEnumerable)
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
                        Discrete = item is Enum ||
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
    }
}
