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
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace numl.Model
{
    public class Property
    {
        public string Name { get; set; }
        public virtual ItemType Type { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Type);
        }

        internal static ItemType FindItemType(Type t)
        {
            if (TypeDescriptor.GetConverter(t).CanConvertTo(typeof(double)))
                return ItemType.Numeric;
            else
            {
                if (t == typeof(string) || t == typeof(char))
                    return ItemType.String;
                else if (t == typeof(bool))
                    return ItemType.Boolean;
                else if (t.BaseType == typeof(Enum))
                    return ItemType.Enumeration;
                else
                    throw new InvalidCastException(string.Format("{0} is an invalid integral type and cannot be used as a property.", t));
            }
        }

        internal static ItemType FindItemType(object o)
        {
            Type t = o.GetType();
            return FindItemType(t);
        }
    }
}
