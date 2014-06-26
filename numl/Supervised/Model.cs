using System;
using System.IO;
using numl.Utils;
using numl.Model;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace numl.Supervised
{
    public abstract class Model : IModel, IXmlSerializable
    {
        public Descriptor Descriptor { get; set; }

        public abstract double Predict(Vector y);

        public object Predict(object o)
        {
            if (Descriptor.Label == null)
                throw new InvalidOperationException("Empty label precludes prediction!");

            var y = Descriptor.Convert(o, false).ToVector();
            var val = Predict(y);
            var result = Descriptor.Label.Convert(val);
            Ject.Set(o, Descriptor.Label.Name, result);
            return o;
        }

        public T Predict<T>(T o)
        {
            return (T)Predict((object)o);
        }

        // ----- saving stuff

        public virtual void Save(string file)
        {
            Xml.Save(file, this, GetType());
        }

        public virtual void Save(Stream stream)
        {
            Xml.Save(stream, this, GetType());
        }

        public virtual string ToXml()
        {
            return Xml.ToXmlString(this, GetType());
        }

        public virtual IModel Load(string file)
        {
            return (IModel)Xml.Load(file, GetType());
        }

        public virtual IModel Load(Stream stream)
        {
            return (IModel)Xml.Load(stream, GetType());
        }

        public virtual IModel LoadXml(string xml)
        {
            return (IModel)Xml.LoadXmlString(xml, GetType());
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public abstract void WriteXml(XmlWriter writer);

        public abstract void ReadXml(XmlReader reader);
    }
}
