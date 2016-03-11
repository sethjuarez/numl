using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using numl.Serialization;
using numl.Supervised.NeuralNetwork;
using numl.Supervised.NeuralNetwork.Recurrent;
using numl.Utils;
using numl.Math.Functions;


namespace numl.Serialization.Supervised.NeuralNetwork
{
    /// <summary>
    /// Recurrent Node serializer.
    /// </summary>
    public class RecurrentNeuronSerializer : JsonSerializer
    {
        /// <summary>
        /// Returns True if the specified type can be assigned.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanConvert(Type type)
        {
            return typeof(RecurrentNeuron).IsAssignableFrom(type);
        }

        /// <summary>
        /// Creates a default Node object.
        /// </summary>
        /// <returns></returns>
        public override object Create()
        {
            return new RecurrentNeuron();
        }

        /// <summary>
        /// Deserializes the object from the stream.
        /// </summary>
        /// <param name="reader">Stream to read from.</param>
        /// <returns>Node object.</returns>
        public override object Read(JsonReader reader)
        {
            if (reader.IsNull()) return null;
            else
            {
                var node = (RecurrentNeuron)this.Create();

                node.Label = reader.ReadProperty().Value.ToString();

                node.Id = reader.ReadProperty().Value.ToString();
                node.NodeId = (int)reader.ReadProperty().Value;
                node.LayerId = (int)reader.ReadProperty().Value;
                node.IsBias = (bool)reader.ReadProperty().Value;

                var activation = reader.ReadProperty().Value;
                if (activation != null)
                    node.ActivationFunction = Ject.FindType(activation.ToString()).CreateDefault<IFunction>();

                var output = reader.ReadProperty().Value;
                if (output != null)
                    node.ActivationFunction = Ject.FindType(output.ToString()).CreateDefault<IFunction>();

                node.Constrained = (bool) reader.ReadProperty().Value;
                node.delta = (double)reader.ReadProperty().Value;
                node.Delta = (double)reader.ReadProperty().Value;
                node.Input = (double)reader.ReadProperty().Value;
                node.Output = (double)reader.ReadProperty().Value;

                //TODO: Read in RNN properties

                return node;
            }
        }

        /// <summary>
        /// Writes the Node object to the stream.
        /// </summary>
        /// <param name="writer">Stream to write to.</param>
        /// <param name="value">Node object to write.</param>
        public override void Write(JsonWriter writer, object value)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                var node = (RecurrentNeuron)value;

                writer.WriteProperty(nameof(Node.Label), node.Label);

                writer.WriteProperty(nameof(Node.Id), node.Id);
                writer.WriteProperty(nameof(Node.NodeId), node.NodeId);
                writer.WriteProperty(nameof(Node.LayerId), node.LayerId);
                writer.WriteProperty(nameof(Node.IsBias), node.IsBias);

                writer.WriteProperty(nameof(Node.ActivationFunction), node.ActivationFunction.GetType().FullName);

                writer.WriteProperty(nameof(Node.OutputFunction), node.OutputFunction?.GetType().FullName);

                writer.WriteProperty(nameof(Node.Constrained), node.Constrained);

                writer.WriteProperty(nameof(Node.delta), node.delta);
                writer.WriteProperty(nameof(Node.Delta), node.Delta);

                writer.WriteProperty(nameof(Node.Input), node.Input);
                writer.WriteProperty(nameof(Node.Output), node.Output);

                //TODO: Write out RNN properties
            }
        }
    }
}
