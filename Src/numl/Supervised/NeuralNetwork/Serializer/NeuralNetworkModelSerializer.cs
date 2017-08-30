using System;
using numl.Model;
using System.Linq;
using System.Reflection;
using numl.Serialization;

namespace numl.Supervised.NeuralNetwork.Serializer
{
    /// <summary>
    /// Serializer for neural network model
    /// </summary>
    public class NeuralNetworkModelSerializer : ModelSerializer
    {
        public override bool CanConvert(Type type)
        {
            return typeof(NeuralNetworkModel).IsAssignableFrom(type);
        }

        public override object Create()
        {
            return new NeuralNetworkModel();
        }
        /// <summary>
        /// Reads serialized neural network model
        /// </summary>
        /// <param name="reader">JsonReader</param>
        /// <returns>Deserialized neural network model</returns>
        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var model = base.Read(reader) as NeuralNetworkModel;

                model.Network = reader.ReadProperty().Value as Network;


                return model;
            }
        }

        /// <summary>
        /// Serializes neural network model
        /// </summary>
        /// <param name="writer">JsonWriter</param>
        /// <param name="value">Model to Serialize</param>
        public override void Write(JsonWriter writer, object value)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                var model = (NeuralNetworkModel)value;
                base.Write(writer, model);

                // write out network
                writer.WriteProperty(nameof(model.Network), model.Network);
            }
        }
    }
}
