using System;
using System.Linq;
using System.Reflection;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using System.IO;

namespace numl.Serialization
{
    public class VectorSerializer : ISerializer
    {
        public bool CanConvert(Type type)
        {
            return typeof(Vector).IsAssignableFrom(type);
        }

        public object Read(TextReader reader)
        {
            if (reader.IsNull())
                return null;
            else
                return new Vector((from i in reader.ReadArray() select (double)i).ToArray());
        }

        public void Write(TextWriter writer, object value)
        {
            if (value == null)
                writer.WriteNull();
            else
                Serializer.Write(writer, ((Vector)value).ToArray());
        }
    }
}
