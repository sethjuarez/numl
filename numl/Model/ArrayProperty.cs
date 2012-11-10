using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using numl.Utils;

namespace numl.Model
{
    public class ArrayProperty : Property
    {
        private readonly int _length;
        public ArrayProperty(int length)
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

        public override IEnumerable<double> Convert(object o)
        {
            if (o.GetType().IsArray)
            {
                var a = (Array)o;

                // should pull no more than specified length
                var max = a.Length > Length ? Length : a.Length;

                for (int i = 0; i < max; i++)
                {
                    // if on first try we can't do anything, just bail
                    if (!FastReflection.CanUseType(a.GetValue(i).GetType()))
                        throw new InvalidCastException(
                           string.Format("Cannot properly cast {0} to a number", i.GetType()));

                    yield return FastReflection.Convert(a.GetValue(i));
                }
            }
            else
                throw new InvalidCastException(
                    string.Format("Cannot cast {0} to array", o.GetType().Name));
        }
    }
}
