using System;
using numl.Utils;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace numl.Model
{
    public class EnumerableProperty : Property
    {
        private readonly int _length;
        public EnumerableProperty(int length)
        {
            _length = length;
        }

        public override int Length
        {
            get
            {
                return _length;
            }
        }

        public override IEnumerable<string> GetColumns()
        {
            for (int i = 0; i < _length; i++)
                yield return i.ToString();
        }

        public override object Convert(double val)
        {
            return val;
        }

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
                    if (i == 0 && !FastReflection.CanUseSimpleType(item.GetType()))
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

                    yield return FastReflection.Convert(item);

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
