using Newtonsoft.Json;
using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Serialization
{
    public class MatrixConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Matrix);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            return new Matrix(serializer.Deserialize<double[][]>(reader));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteNull();
            else
                serializer.Serialize(writer, ((Matrix)value).ToArray());
        }
    }
}
