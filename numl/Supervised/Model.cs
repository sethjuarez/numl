using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math;
using numl.Model;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace numl.Supervised
{
    public abstract class Model : IModel
    {
        public Description Description { get; set; }
        public abstract double Predict(Vector y);

        public object Predict(object o)
        {
            var label = Description.Label;
            var pred = Predict(Description.ToVector(o));
            var val = R.Convert(pred, label.Type);
            R.Set(o, label.Name, val);
            return o;
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
