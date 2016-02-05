using System;
using numl.Model;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace numl.Serialization
{
    public class DateTimePropertySerializer : ISerializer
    {
        public bool CanConvert(Type type)
        {
            return typeof(DateTimeProperty).IsAssignableFrom(type);
        }

        public object Deserialize(TextReader reader)
        {
            var o = Serializer.Parse(reader);
            // TODO: Normalize
            return o;
        }

        public void Write(TextWriter writer, object value)
        {
            if (value == null)
                writer.WriteNull();
            else
                Serializer.Write(writer, DateTimeProperty.GetColumns((DateTimeFeature)value).ToArray());

        }
    }
}
