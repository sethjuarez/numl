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
using numl.Math;
using numl.Model;
using System.Text;
using System.Linq;
using numl.Exceptions;
using System.ComponentModel;
using System.Collections.Generic;

namespace numl
{
    public static class Convert
    {
        public static Vector ToVector(this object o, Description description)
        {
            Property[] features = description.Features;
            if (description == null || features == null || features.Length == 0)
                throw new InvalidDescriptionException();

            foreach (Property p in features.Where(p => p is StringProperty))
            {
                // looking for valid dictionaries for string properties
                StringProperty property = (StringProperty)p;
                if (property.Dictionary == null || property.Dictionary.Length == 0)
                    throw new InvalidOperationException(string.Format("Cannot convert StringProperty {0} with an empty property dictionary!", p.Name));
            }

            // number of features (including expanded string features)
            int d = features
                        .Where(p => !(p is StringProperty)).Count() +
                    features
                        .Where(p => p is StringProperty)
                        .Aggregate(0, (no, p) => no += (p as StringProperty).Dictionary.Length);

            Vector vector = Vector.Zeros(d);

            int i = -1;
            foreach (var f in features)
            {
                object val = GetItem(o, f.Name);
                if (f is StringProperty)
                {
                    var wc = StringHelpers.GetWordCount((string)val, (StringProperty)f);
                    for (int k = 0; k < wc.Length; k++)
                        vector[++i] = wc[k];
                }
                else
                    vector[++i] = ConvertObject(val);
            }

            return vector;
        }

        public static Matrix ToMatrix(this IEnumerable<object> objects, Description description)
        {
            Property[] features = description.Features;
            if (description == null || features == null || features.Length == 0)
                throw new InvalidDescriptionException();

            // build dictionaries if not already built
            description.BuildDictionaries(objects);

            // number of examples
            int n = objects.Count();

            // number of features (including expanded string features)
            int d = features
                        .Where(p => !(p is StringProperty)).Count() +
                    features
                        .Where(p => p is StringProperty)
                        .Aggregate(0, (no, p) => no += (p as StringProperty).Dictionary.Length);

            double[][] matrix = new double[n][];

            int i = -1;
            int j = -1;
            foreach (object o in objects)
            {
                matrix[++i] = new double[d];
                j = -1;
                foreach (var f in features)
                {
                    object val = GetItem(o, f.Name);
                    if (f is StringProperty)
                    {
                        var wc = StringHelpers.GetWordCount((string)val, (StringProperty)f);
                        for (int k = 0; k < wc.Length; k++)
                            matrix[i][++j] = wc[k];
                    }
                    else
                        matrix[i][++j] = ConvertObject(val);
                }
            }

            return new Matrix(matrix);

        }

        #region Internal Conversion Utility Methods
        /// <summary>
        /// Get a property or dictionary value in an object by name
        /// </summary>
        /// <param name="o">object</param>
        /// <param name="name">key</param>
        /// <returns>value of keyed item</returns>
        internal static object GetItem(object o, string name)
        {
            var type = o.GetType();
            object val = null;
            var prop = type.GetProperty(name);
            if (prop == null)
            {
                // treat as string indexable for objects with
                // o["Feature"] type lookups.
                var method = type.GetMethod("get_Item", new Type[] { typeof(string) });
                if (method == null)
                    throw new InvalidOperationException(string.Format("Could not find a property named \"{0}\" in containing object.", name));

                val = method.Invoke(o, new object[] { name });
            }
            else
                val = prop.GetValue(o, new object[] { });

            return val;
        }

        internal static double ConvertBoolPlusMinus(bool o)
        {
            return o ? 1 : -1;
        }

        internal static double ConvertBoolOneZero(bool o)
        {
            return o ? 1 : 0;
        }

        internal static double ConvertEnum(Enum o)
        {
            return (int)System.Convert.ChangeType(o, typeof(int));
        }

        internal static double ConvertString(string o)
        {
            return o.Length;
        }

        internal static double ConvertNumber(object o)
        {
            if (o.GetType() == typeof(double))
                return (double)o;
            else if (o.GetType() == typeof(char))
                return (int)Encoding.ASCII.GetBytes(new char[] { (char)o })[0];
            else
            {
                var converter = TypeDescriptor.GetConverter(o.GetType());
                if (converter.CanConvertTo(typeof(double)))
                    return (double)converter.ConvertTo(o, typeof(double));
                else
                    throw new InvalidOperationException(string.Format("Cannot convert {0} to double!", o.GetType()));
            }
        }

        internal static double ConvertObject(object o)
        {
            Type t = o.GetType();
            TypeConverter converter = TypeDescriptor.GetConverter(t);
            if (converter.CanConvertTo(typeof(double)) || t == typeof(char))
                return ConvertNumber(o);
            else
            {
                if (t == typeof(string))
                    return ConvertString((string)o);
                else if (t == typeof(bool))
                    return ConvertBoolPlusMinus((bool)o);
                else if (t.BaseType == typeof(Enum))
                    return ConvertEnum((Enum)o);
                else
                    throw new InvalidCastException(string.Format("Cannot convert {0} to double", t));
            }
        }
        #endregion
    }
}
