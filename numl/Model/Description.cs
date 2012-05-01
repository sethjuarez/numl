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
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace numl.Model
{
    [XmlRoot("description"), Serializable]
    public class Description : IXmlSerializable
    {
        public Description()
        {
            Dimensions = -1;
        }

        public Property[] Features { get; set; }
        public Property Label { get; set; }
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

        public Tuple<Matrix, Vector> ToExamples(IEnumerable<object> collection)
        {
            // number of examples
            int n = collection.Count();
            Vector y = new Vector(n);

            double[][] matrix = new double[n][];

            int i = -1;
            foreach (object o in collection)
            {
                matrix[++i] = CreateFeatureArray(o);
                y[i] = Label.Convert(R.Get(o, Label.Name));
            }

            return new Tuple<Matrix, Vector>(new Matrix(matrix), y);
        }

        public Tuple<Matrix, Vector> ToExamples(IEnumerable collection)
        {
            List<double[]> matrix = new List<double[]>();
            List<double> y = new List<double>();
            foreach (object o in collection)
            {
                matrix.Add(CreateFeatureArray(o));
                y.Add(Label.ToArray(o)[0]);
            }

            return new Tuple<Matrix, Vector>(new Matrix(matrix.ToArray()), new Vector(y.ToArray()));
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

            Description description = new Description 
            { 
                Features = items.OrderBy(c => c.Name).ToArray(), 
                Label = label 
            };

            return description;
        }

        // serialization
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            if (Dimensions <= 0)
                Dimensions = Features.Sum(f => f.Length);

            writer.WriteAttributeString("dim", Dimensions.ToString());
            writer.WriteStartElement("features");
            writer.WriteAttributeString("length", Features.Length.ToString());
            for (int i = 0; i < Length; i++)
            {
                XmlSerializer ser = new XmlSerializer(this[i].GetType());
                ser.Serialize(writer, this[i]);
            }
   
            writer.WriteEndElement();

            writer.WriteStartElement("label");
            if (Label != null)
            {
                XmlSerializer ser = new XmlSerializer(Label.GetType());
                ser.Serialize(writer, Label);
            }
            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Dimensions = int.Parse(reader.GetAttribute("dim"));

            reader.ReadStartElement();
            int length = int.Parse(reader.GetAttribute("length"));
            Features = new Property[length];
            // begin reading features
            reader.ReadStartElement();
            for (int i = 0; i < length; i++)
            {
                if (reader.Name == "p")
                    Features[i] = new Property();
                else if (reader.Name == "sp")
                    Features[i] = new StringProperty();

                Features[i].ReadXml(reader);
            }
            reader.ReadEndElement();

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement();
                if (reader.Name == "p")
                    Label = new Property();
                else if (reader.Name == "sp")
                    Label = new StringProperty();

                Label.ReadXml(reader);
                reader.ReadEndElement(); // label
            }
            else
                reader.ReadStartElement();

            reader.ReadEndElement(); // description
        }
    }
}
