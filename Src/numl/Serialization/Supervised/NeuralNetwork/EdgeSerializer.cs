using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using numl.Serialization;
using numl.Supervised.NeuralNetwork;

namespace numl.Serialization.Supervised.NeuralNetwork
{
    /// <summary>
    /// Edge serializer.
    /// </summary>
    public class EdgeSerializer : JsonSerializer
    {
        /// <summary>
        /// Returns True if the specified type can be assigned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanConvert(Type type)
        {
            return typeof(Edge).IsAssignableFrom(type);
        }

        /// <summary>
        /// Creates a default Edge object.
        /// </summary>
        /// <returns></returns>
        public override object Create()
        {
            return new Edge();
        }

        /// <summary>
        /// Deserializes the object from the stream.
        /// </summary>
        /// <param name="reader">Stream to read from.</param>
        /// <returns>Edge object.</returns>
        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var edge = (Edge)this.Create();

                edge.ParentId = (int)reader.ReadProperty().Value;
                edge.ChildId = (int)reader.ReadProperty().Value;
                edge.Weight = (double)reader.ReadProperty().Value;
                
                return edge;
            }
        }

        /// <summary>
        /// Writes the Edge object to the stream.
        /// </summary>
        /// <param name="writer">Stream to write to.</param>
        /// <param name="value">Edge object to write.</param>
        public override void Write(JsonWriter writer, object value)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                var edge = (Edge)value;

                writer.WriteProperty(nameof(edge.ParentId), edge.ParentId);
                writer.WriteProperty(nameof(edge.ChildId), edge.ChildId);
                writer.WriteProperty(nameof(edge.Weight), edge.Weight);
            }
        }
    }
}
