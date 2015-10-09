using System;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Serialization
{
    public class VectorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Vector).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            return new Vector(serializer.Deserialize<double[]>(reader));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteNull();
            else
                serializer.Serialize(writer, ((Vector)value).ToArray());
        }
    }
}
