/*
 Copyright (c) 2012 Seth Juarez

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
*/

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

                    yield return FastReflection.Convert(item);

                    // should pull no more than specified length
                    if (++i == Length)
                        break;
                }

                for (int j = i + 1; i < Length; i++)
                    yield return 0;
            }
            else
                throw new InvalidCastException(
                    string.Format("Cannot cast {0} to an IEnumerable", o.GetType().Name));
        }
    }
}
