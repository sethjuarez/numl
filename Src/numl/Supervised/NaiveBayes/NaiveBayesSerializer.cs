using System;
using System.Reflection;
using numl.Supervised;
using numl.Supervised.NaiveBayes;

namespace numl.Serialization.Supervised.NaiveBayes
{
    public class NaiveBayesSerializer : ModelSerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(NaiveBayesModel).IsAssignableFrom(type);
        }

        public override object Create()
        {
            return new NaiveBayesModel();
        }

        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var d = base.Read(reader) as NaiveBayesModel;

                // naive reads properties here

                return d;
            }
        }

        public override void Write(JsonWriter writer, object value)
        {
            if (value == null) writer.WriteNull();
            else
            {
                var d = value as NaiveBayesModel;
                base.Write(writer, d);

                // naive bates writes here
            }
        }
    }
}
