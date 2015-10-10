using System;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections.Generic;
using numl.Supervised.NeuralNetwork;

namespace numl.Serialization
{
    public class NetworkConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Network).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                var network = (Network)value;

                writer.WriteStartObject();

                // write out nodes
                serializer.Serialize(writer, network.GetNodes().ToArray());
                writer.WriteEnd();

                // write out IN edges
                writer.WritePropertyName("In");
                serializer.Serialize(writer, network.In.ToArray());
                writer.WriteEnd();

                // write out OUT edges
                writer.WritePropertyName("Out");
                serializer.Serialize(writer, network.Out.ToArray());
                writer.WriteEnd();

                writer.WriteEndObject();
            }
        }
    }
}
