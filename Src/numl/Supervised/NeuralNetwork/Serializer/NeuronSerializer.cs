using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using numl.Math.Functions;
using numl.Serialization;
using numl.Supervised.NeuralNetwork;
using numl.Utils;

namespace numl.Supervised.NeuralNetwork.Serializer
{
    /// <summary>
    /// Node serializer.
    /// </summary>
    public class NeuronSerializer : JsonSerializer<Neuron>
    {
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
                var node = (Neuron)this.Create();

                node.Label = reader.ReadProperty().Value.ToString();

                node.Id = (int)(double)reader.ReadProperty().Value;
                node.NodeId = (int)(double)reader.ReadProperty().Value;
                node.LayerId = (int)(double)reader.ReadProperty().Value;
                node.IsBias = (bool)reader.ReadProperty().Value;

                var activation = reader.ReadProperty().Value;
                if (activation != null)
                    node.ActivationFunction = Ject.FindType(activation.ToString()).CreateDefault<IFunction>();

                node.Constrained = (bool)reader.ReadProperty().Value;
                node.delta = (double)reader.ReadProperty().Value;
                node.Delta = (double)reader.ReadProperty().Value;
                node.Input = (double)reader.ReadProperty().Value;
                node.Output = (double)reader.ReadProperty().Value;

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
                var node = (Neuron)value;

                writer.WriteProperty(nameof(Neuron.Label), node.Label);

                writer.WriteProperty(nameof(Neuron.Id), node.Id);
                writer.WriteProperty(nameof(Neuron.NodeId), node.NodeId);
                writer.WriteProperty(nameof(Neuron.LayerId), node.LayerId);
                writer.WriteProperty(nameof(Neuron.IsBias), node.IsBias);

                writer.WriteProperty(nameof(Neuron.ActivationFunction), node.ActivationFunction.GetType().FullName);

                writer.WriteProperty(nameof(Neuron.Constrained), node.Constrained);

                writer.WriteProperty(nameof(Neuron.delta), node.delta);
                writer.WriteProperty(nameof(Neuron.Delta), node.Delta);

                writer.WriteProperty(nameof(Neuron.Input), node.Input);
                writer.WriteProperty(nameof(Neuron.Output), node.Output);
            }
        }
    }
}
