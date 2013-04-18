using System;
using System.IO;
using numl.Utils;
using numl.Model;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace numl.Supervised
{
    public abstract class Model : IModel
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
            FastReflection.Set(o, Descriptor.Label.Name, result);
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
    }
}
