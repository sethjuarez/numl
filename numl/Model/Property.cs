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
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Reflection;

namespace numl.Model
{
    [XmlRoot("p"), Serializable]
    public class Property : IXmlSerializable
    {
        public Property()
        {
            Start = -1;
        }
        /// <summary>
        /// Property name that maps to object
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property Type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Numeric value counts produced by property
        /// </summary>
        public virtual int Length { get { return 1; } }

        /// <summary>
        /// Offset into feature vector
        /// </summary>
        public int Start { get; set; }

        public virtual double[] ToArray(object o)
        {
            return new[] { Convert(o) };
        }

        public virtual double Convert(object o)
        {
            return R.Convert(o);
        }

        public virtual object Convert(double val)
        {
            return R.Convert(val, Type);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Type.Name);
        }

        /// <summary>
        /// General purpose property storage
        /// (will not get serialized)
        /// </summary>
        public object Tag { get; set; }

        // xml serialization
        public XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("type", Type.AssemblyQualifiedName);
            writer.WriteAttributeString("start", Start.ToString());
        }

        public virtual void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Name = reader.GetAttribute("name");
            string type = reader.GetAttribute("type");
            Type = R.FindType(type);
            Start = int.Parse(reader.GetAttribute("start"));
            reader.ReadStartElement();
        }
    }
}
