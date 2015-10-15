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
                writer.WritePropertyName("Nodes");
                serializer.Serialize(writer, network.GetNodes().ToArray());

                // write out all edges
                writer.WritePropertyName("Edges");
                serializer.Serialize(writer, network.GetEdges().ToArray());


                // write out IN edges
                writer.WritePropertyName("In");
                serializer.Serialize(writer, network.In.Select(n=>n.Id).ToArray());
                

                // write out OUT edges
                writer.WritePropertyName("Out");
                serializer.Serialize(writer, network.Out.Select(n => n.Id).ToArray());
                

                writer.WriteEndObject();
            }
        }
    }
}
