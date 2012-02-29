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
using System.Linq;
using System.Text;
using numl.Exceptions;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using numl.Attributes;
using numl.Supervised;

namespace numl
{
    public static class NConvert
    {
        /// <summary>
        /// Used to create a machine learning type description
        /// based upon the type T. The examples should be provided
        /// to account for the case of using StringProperties and
        /// enables the creation of the corresponding dictionaries.
        /// It is assumed that the types are correctly marked up with
        /// Property and Label attributes.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="examples">Examples</param>
        /// <returns>Populated Description</returns>
        public static Description ToDescription<T>(this IEnumerable<T> examples)
            where T : class
        {
            var description = typeof(T).ToDescription();
            return description.BuildDictionaries(examples);
        }

        public static Description ToDescription<T>()
        {
            return typeof(T).ToDescription();
        }

        public static Description ToDescription(this Type t)
        {
            List<Property> items = new List<Property>();
            Property label = null;

            foreach (var property in t.GetProperties())
            {
                var feature = property.GetCustomAttributes(typeof(NumlAttribute), false);

                // not marked
                if (feature.Length == 0) continue;

                if (feature.Length == 1)
                {
                    var attrib = feature[0];
                    var type = Property.FindItemType(property.PropertyType);
                    Property p = type == ItemType.String ?
                        // default to word separation if nothing else is specified
                        new StringProperty { Type = type, Name = property.Name, SplitType = StringSplitType.Word, Separator = " " } :
                        new Property { Type = type, Name = property.Name };

                    if (attrib is StringAttribute)
                    {
                        var sf = (StringFeatureAttribute)attrib;
                        var sp = (StringProperty)p;
                        sp.Separator = sf.Separator;
                        sp.SplitType = sf.SplitType;
                        sp.AsEnum = sf.AsEnum;

                        // load exclusion file if it exists
                        sp.ImportExclusions(sf.ExclusionFile);
                    }

                    if (attrib is FeatureAttribute || attrib is StringFeatureAttribute)
                        items.Add(p);
                    else if (attrib is LabelAttribute || attrib is StringLabelAttribute)
                        label = p;
                }
            }

            //if (items.Count < 2)
            //    throw new InvalidOperationException("Type has less than two attributes!");

            Description description = label == null ?
                new Description { Features = items.OrderBy(c => c.Name).ToArray() } :
                new LabeledDescription { Features = items.ToArray(), Label = label };

            return description;
        }

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
                        .Aggregate(0, (no, p) =>
                            {
                                var sp = (p as StringProperty);
                                if (sp.AsEnum)
                                    no += 1;
                                else
                                    no += sp.Dictionary.Length;
                                return no;
                            }
                        );

            Vector vector = Vector.Zeros(d);

            int i = -1;
            foreach (var f in features)
            {
                object val = GetPropertyValue(o, f.Name);
                if (f is StringProperty)
                {
                    StringProperty sf = (StringProperty)f;
                    if (sf.AsEnum)
                        vector[++i] = StringHelpers.GetWordPosition(val.ToString(), sf);
                    else
                    {
                        var wc = StringHelpers.GetWordCount(val.ToString(), sf);
                        for (int k = 0; k < wc.Length; k++)
                            vector[++i] = wc[k];
                    }
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

            // check for proper dictionaries
            foreach (Property p in features.Where(p => p is StringProperty))
            {
                // looking for valid dictionaries for string properties
                StringProperty property = (StringProperty)p;
                if (property.Dictionary == null || property.Dictionary.Length == 0)
                    throw new InvalidOperationException(string.Format("Cannot convert StringProperty \"{0}\" with an empty property dictionary!", p.Name));
            }

            // TODO: This is being rebuilt *EACH TIME* need to fix this
            // description.BuildDictionaries(objects);

            // number of examples
            int n = objects.Count();

            // number of features (including expanded string features)
            int d = features
                        .Where(p => !(p is StringProperty)).Count() +
                    features
                        .Where(p => p is StringProperty)
                        .Aggregate(0, (no, p) =>
                            {
                                var sp = (p as StringProperty);
                                if (sp.AsEnum)
                                    no += 1;
                                else
                                    no += sp.Dictionary.Length;
                                return no;
                            }
                        );

            double[][] matrix = new double[n][];

            int i = -1;
            int j = -1;
            foreach (object o in objects)
            {
                matrix[++i] = new double[d];
                j = -1;
                foreach (var f in features)
                {
                    object val = GetPropertyValue(o, f.Name);
                    if (f is StringProperty)
                    {
                        StringProperty sf = (StringProperty)f;
                        if (sf.AsEnum)
                            matrix[i][++j] = StringHelpers.GetWordPosition(val.ToString(), sf);
                        else
                        {
                            var wc = StringHelpers.GetWordCount(val.ToString(), (StringProperty)f);
                            for (int k = 0; k < wc.Length; k++)
                                matrix[i][++j] = wc[k];
                        }
                    }
                    else
                        matrix[i][++j] = ConvertObject(val);
                }
            }

            return new Matrix(matrix);

        }

        public static Tuple<Matrix, Vector> ToExamples(this LabeledDescription description, IEnumerable<object> examples)
        {
            return examples.ToExamples(description);
        }

        public static Tuple<Matrix, Vector> ToExamples(this IEnumerable<object> examples, LabeledDescription description)
        {
            Property[] features = description.Features;
            if (description == null || features == null || features.Length == 0)
                throw new InvalidDescriptionException();

            // check for proper dictionaries
            foreach (Property p in features.Where(p => p is StringProperty))
            {
                // looking for valid dictionaries for string properties
                StringProperty property = (StringProperty)p;
                if (property.Dictionary == null || property.Dictionary.Length == 0)
                    throw new InvalidOperationException(string.Format("Cannot convert StringProperty {0} with an empty property dictionary!", p.Name));
            }

            // TODO: This is being rebuilt *EACH TIME* need to fix this
            // description.BuildDictionaries(objects);

            // number of examples
            int n = examples.Count();

            // number of features (including expanded string features)
            int d = features
                        .Where(p => !(p is StringProperty)).Count() +
                    features
                        .Where(p => p is StringProperty)
                        .Aggregate(0, (no, p) =>
                            {
                                var sp = (p as StringProperty);
                                if (sp.AsEnum)
                                    no += 1;
                                else
                                    no += sp.Dictionary.Length;
                                return no;
                            }
                        );

            double[][] matrix = new double[n][];
            Vector y = new Vector(n);

            int i = -1;
            int j = -1;
            foreach (object o in examples)
            {
                matrix[++i] = new double[d];
                j = -1;
                foreach (var f in features)
                {
                    object val = GetPropertyValue(o, f.Name);
                    if (f is StringProperty)
                    {
                        StringProperty sf = (StringProperty)f;
                        if (sf.AsEnum)
                            matrix[i][++j] = StringHelpers.GetWordPosition(val.ToString(), sf);
                        else
                        {
                            var wc = StringHelpers.GetWordCount(val.ToString(), (StringProperty)f);
                            for (int k = 0; k < wc.Length; k++)
                                matrix[i][++j] = wc[k];
                        }
                    }
                    else
                        matrix[i][++j] = ConvertObject(val);
                }

                object yval = GetPropertyValue(o, description.Label.Name);
                if (description.Label is StringProperty)
                    y[i] = (double)StringHelpers.GetWordPosition((string)yval, (StringProperty)description.Label);
                else
                    y[i] = ConvertObject(yval);

            }

            return new Tuple<Matrix, Vector>(new Matrix(matrix), y);
        }

        public static T Predict<T>(this IModel model, T o)
        {
            Vector y = o.ToVector(model.Description);
            double value = model.Predict(y);
            return SetItem(o, model.Description.Label, value);
        }


        #region Internal Conversion Utility Methods
        internal static void SetItem(object o, Property label, double value, Type oType)
        {
            var p = oType.GetProperty(label.Name);
            switch (label.Type)
            {
                case ItemType.Boolean:
                    p.SetValue(o, value > 0, null);
                    break;
                case ItemType.Numeric:
                    p.SetValue(o, Convert.ChangeType(value, p.PropertyType), null);
                    break;
                case ItemType.Enumeration:
                    var numericValue = Convert.ChangeType(value, System.Enum.GetUnderlyingType(p.PropertyType));
                    object enumValue = System.Enum.ToObject(p.PropertyType, numericValue);
                    p.SetValue(o, enumValue, null);
                    break;
                case ItemType.String:
                    if (!(label is StringProperty))
                        throw new InvalidOperationException("Type property mismatch!");
                    string s = (label as StringProperty).Dictionary[(int)value];
                    p.SetValue(o, s, null);
                    break;
            }
        }

        internal static T SetItem<T>(T o, Property label, double value)
        {
            Type oType = typeof(T);
            SetItem(o, label, value, oType);
            return o;
        }

        /// <summary>
        /// Get a property or dictionary value in an object by name
        /// </summary>
        /// <param name="o">object</param>
        /// <param name="name">key</param>
        /// <returns>value of keyed item</returns>
        public static object GetPropertyValue(object o, string name)
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


        public static void SetPropertyValue(object o, string name, object value)
        {
            var type = o.GetType();
            var prop = type.GetProperty(name);
            if (prop == null)
            {
                // treat as string indexable for objects with
                // o["Feature"] type sets.
                var method = type.GetMethod("set_Item", new Type[] { typeof(string), typeof(object) });
                if (method == null)
                    throw new InvalidOperationException(string.Format("Could not find a property named \"{0}\" in containing object.", name));

                method.Invoke(o, new object[] { name, value });
            }
            else
                prop.SetValue(o, value, new object[] { });
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
