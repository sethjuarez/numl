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
using System.Text;
using numl.Math;
using System.Collections;
using System.Drawing;
using numl.Attributes;

namespace numl.Model
{
    public class Description
    {
        public Description()
        {
            Dimensions = -1;
        }

        public Property[] Features { get; set; }
        public int Dimensions { get; private set; }

        public virtual Property this[int i]
        {
            get
            {
                if (i > -1 && i < Features.Length)
                    return Features[i];
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public virtual int Length
        {
            get
            {
                return Features.Length;
            }
        }

        public virtual bool Verify()
        {
            Property[] features = Features;
            if (features == null || features.Length == 0)
                return false;

            foreach (Property p in features.Where(p => p is StringProperty))
            {
                StringProperty property = (StringProperty)p;
                if (property.Dictionary == null || property.Dictionary.Length == 0)
                    return false;
            }

            return true;
        }

        public virtual Matrix ToMatrix(IEnumerable<object> collection)
        {
            // number of examples
            int n = collection.Count();

            double[][] matrix = new double[n][];

            int i = -1;
            foreach (object o in collection)
                matrix[++i] = CreateFeatureArray(o);

            return new Matrix(matrix);
        }

        public virtual Matrix ToMatrix(IEnumerable collection)
        {
            List<double[]> matrix = new List<double[]>();
            foreach (object o in collection)
                matrix.Add(CreateFeatureArray(o));

            return new Matrix(matrix.ToArray());
        }

        public virtual Vector ToVector(object o)
        {
            return new Vector(CreateFeatureArray(o));
        }

        internal double[] CreateFeatureArray(object o)
        {
            if(Dimensions == -1)
                Dimensions = Features.Select(p => p.Length).Sum();

            double[] vector = new double[Dimensions];
            int j = -1;

            for (int i = 0; i < Features.Length; i++)
            {
                var p = Features[i];
                object val = R.Get(o, p.Name);

                // type not set?
                if(p.Type == null)
                    p.Type = val.GetType();

                var vals = p.ToArray(val);

                // mapping into column
                if (p.Start == -1)
                    p.Start = j + 1;

                for (int k = 0; k < vals.Length; k++)
                    vector[++j] = vals[k];
            }

            return vector;
        }

        public static Description Create<T>()
            where T : class
        {
            return Create(typeof(T));
        }

        public static Description Create(Type t)
        {
            List<Property> items = new List<Property>();
            Property label = null;

            foreach (var property in t.GetProperties())
            {
                var feature = property.GetCustomAttributes(typeof(NumlAttribute), false);

                if (feature.Length == 1)
                {
                    var attrib = feature[0];
                    var type = property.PropertyType;
                    if (!R.CanUseType(type))
                        throw new InvalidCastException(string.Format("Cannot use {0} as a feature or label", type.Name));

                    Property p;
                    if (type == typeof(string)) // default if not marked explicitly
                        p = new StringProperty { Type = type, Name = property.Name, SplitType = StringSplitType.Word, Separator = " " };
                    else // other - perhaps check for propert types here??
                        p = new Property { Type = type, Name = property.Name };

                    // TODO: Add DateTime measures

                    // options if marked explicitly
                    if (attrib is StringAttribute)
                    {
                        var sf = (StringFeatureAttribute)attrib;
                        var sp = new StringProperty { Type = type, Name = property.Name, SplitType = StringSplitType.Word, Separator = " " };
                        sp.Separator = sf.Separator;
                        sp.SplitType = sf.SplitType;
                        sp.AsEnum = sf.AsEnum;

                        // load exclusion file if it exists
                        sp.ImportExclusions(sf.ExclusionFile);
                    }

                    if (attrib is FeatureAttribute || attrib is StringFeatureAttribute)
                        items.Add(p);
                    else if (attrib is LabelAttribute || attrib is StringLabelAttribute)
                    {
                        if (p is StringProperty)
                            ((StringProperty)p).AsEnum = true;
                        label = p;
                    }
                }
            }

            Description description = label == null ?
                new Description { Features = items.OrderBy(c => c.Name).ToArray() } :
                new LabeledDescription { Features = items.OrderBy(c => c.Name).ToArray(), Label = label };

            return description;
        }
    }
}
