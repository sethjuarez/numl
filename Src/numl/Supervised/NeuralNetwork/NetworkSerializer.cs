using System;
using System.IO;
using System.Linq;
using numl.Serialization;
using System.Collections.Generic;

namespace numl.Supervised.NeuralNetwork
{
    public class NetworkSerializer : ISerializer
    {
        public bool CanConvert(Type type)
        {
            return typeof(Network).IsAssignableFrom(type);
        }

        public object Read(TextReader reader)
        {
            var dictionary = Serializer.Read(reader);


            return dictionary;
        }

        public void Write(TextWriter writer, object o)
        {
            if (o == null)
                writer.WriteNull();
            else
            {
                var network = (Network)o;

                writer.WriteStartObject();

                // write out nodes
                writer.WriteArrayProperty("Nodes", 
                        network.GetNodes().ToArray(), 
                        new NodeSerializer());

                // write out all edges
                writer.WriteArrayProperty("Edges", 
                        network.GetEdges().ToArray(),
                        new EdgeSerializer());

                // write out IN edges
                writer.WriteArrayProperty("In", network.In.Select(n => n.Id).ToArray());

                // write out OUT edges
                writer.WriteArrayProperty("Out", network.Out.Select(n => n.Id).ToArray());

                writer.WriteEndObject();
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class NetworkConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public bool CanConvert(Type objectType)
        {
            return typeof(Network).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //{
        //    if (reader.TokenType == JsonToken.Null) return null;

        //    reader.Read(); // start object
        //    reader.Read(); // start property "Nodes"
        //    var nodes = serializer.Deserialize<Node[]>(reader);
        //    reader.Read(); // end property
        //    reader.Read(); // start property "Edges"
        //    var edges = serializer.Deserialize<Edge[]>(reader);
        //    reader.Read(); // end property
        //    reader.Read(); // start property "In"
        //    var inNodes = serializer.Deserialize<string[]>(reader);
        //    reader.Read(); // end property
        //    reader.Read(); // start property "Out"
        //    var outNodes = serializer.Deserialize<string[]>(reader);
        //    reader.Read(); // end property

        //    Network network = new Network();

        //    // rebuild edges
        //    foreach (var edge in edges)
        //    {
        //        var source = nodes.Where(n => n.Id == edge.SourceId).First();
        //        var target = nodes.Where(n => n.Id == edge.TargetId).First();
        //        Edge.Create(source, target, edge.Weight);
        //    }

        //    // rebuild in
        //    network.In = new Node[inNodes.Length];
        //    for (int i = 0; i < inNodes.Length; i++)
        //        network.In[i] = nodes.Where(n => n.Id == inNodes[i]).First();

        //    // rebuid out
        //    network.Out = new Node[outNodes.Length];
        //    for (int i = 0; i < outNodes.Length; i++)
        //        network.Out[i] = nodes.Where(n => n.Id == outNodes[i]).First();


        //    return network;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        //public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //{
        //    if (value == null)
        //        writer.WriteNull();
        //    else
        //    {
        //        var network = (Network)value;

        //        writer.WriteStartObject();

        //        // write out nodes
        //        writer.WritePropertyName("Nodes");
        //        serializer.Serialize(writer, network.GetNodes().ToArray());

        //        // write out all edges
        //        writer.WritePropertyName("Edges");
        //        serializer.Serialize(writer, network.GetEdges().ToArray());


        //        // write out IN edges
        //        writer.WritePropertyName("In");
        //        serializer.Serialize(writer, network.In.Select(n => n.Id).ToArray());


        //        // write out OUT edges
        //        writer.WritePropertyName("Out");
        //        serializer.Serialize(writer, network.Out.Select(n => n.Id).ToArray());


        //        writer.WriteEndObject();
        //    }
        //}
    }
}
