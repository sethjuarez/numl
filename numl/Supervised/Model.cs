using System;
using System.IO;
using numl.Utils;
using numl.Model;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace numl.Supervised
{
    public abstract class Model : IModel
    {
        public Descriptor Descriptor { get; set; }

        public abstract double Predict(Vector y);

        public object Predict(object o)
        {
            var vector = Descriptor.Convert(o).ToVector();

            throw new NotImplementedException();
        }

        public T Predict<T>(T o)
        {
            return (T)Predict((object)o);
        }


        public void Save(string file)
        {
            using (var stream = File.OpenWrite(file))
                Save(stream);
        }

        public void Save(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(stream, this, ns);
        }

        public IModel Load(string file)
        {
            using (var stream = File.OpenRead(file))
                return Load(stream);
        }

        public IModel Load(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            var o = serializer.Deserialize(stream);
            return (IModel)o;
        }
    }
}
