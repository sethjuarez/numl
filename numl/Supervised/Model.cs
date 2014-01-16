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

        public virtual void Save(string file)
        {
            using (var stream = File.OpenWrite(file))
                Save(stream);
        }

        public virtual void Save(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            serializer.Serialize(stream, this, ns);
        }

        public virtual string ToXml()
        {
            XmlSerializer serializer = new XmlSerializer(GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            StringWriter textWriter = new StringWriter();
            ns.Add("", "");

            serializer.Serialize(textWriter, this, ns);
            return textWriter.ToString();
        }

        public virtual IModel Load(string file)
        {
            using (var stream = File.OpenRead(file))
                return Load(stream);
        }

        public virtual IModel Load(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(GetType());
            var o = serializer.Deserialize(stream);
            return (IModel)o;
        }

        public virtual IModel LoadXml(string xml)
        {
            TextReader reader = new StringReader(xml);
            XmlSerializer serializer = new XmlSerializer(GetType());
            var o = serializer.Deserialize(reader);
            return (IModel)o;
        }

        public virtual void WriteXml<T>(XmlWriter writer, T thing)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(writer, thing, ns);
        }

        public virtual T ReadXml<T>(XmlReader reader)
        {
            XmlSerializer dserializer = new XmlSerializer(typeof(T));
            T item = (T)dserializer.Deserialize(reader);
            // move to next thing
            reader.Read();
            return item;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public abstract void WriteXml(XmlWriter writer);

        public abstract void ReadXml(XmlReader reader);
    }
}
