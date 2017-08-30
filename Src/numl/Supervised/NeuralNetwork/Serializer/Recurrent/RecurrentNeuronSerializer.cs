using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using numl.Serialization;
using numl.Supervised.NeuralNetwork;
using numl.Supervised.NeuralNetwork.Recurrent;
using numl.Utils;
using numl.Math.Functions;


namespace numl.Supervised.NeuralNetwork.Serializer.Recurrent
{
    /// <summary>
    /// Recurrent Node serializer.
    /// </summary>
    public class RecurrentNeuronSerializer : JsonSerializer<RecurrentNeuron>
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
                var node = (RecurrentNeuron)this.Create();

                node.Label = reader.ReadProperty().Value.ToString();

                node.Id = (int)reader.ReadProperty().Value;
                node.NodeId = (int)reader.ReadProperty().Value;
                node.LayerId = (int)reader.ReadProperty().Value;
                node.IsBias = (bool)reader.ReadProperty().Value;

                var activation = reader.ReadProperty().Value;
                if (activation != null)
                    node.ActivationFunction = Ject.FindType(activation.ToString()).CreateDefault<IFunction>();

                node.Constrained = (bool) reader.ReadProperty().Value;
                node.delta = (double)reader.ReadProperty().Value;
                node.Delta = (double)reader.ReadProperty().Value;
                node.Input = (double)reader.ReadProperty().Value;
                node.Output = (double)reader.ReadProperty().Value;

                node.H = (double) reader.ReadProperty().Value;
                node.Hh = (double) reader.ReadProperty().Value;

                node.R = (double) reader.ReadProperty().Value;
                node.Rb = (double) reader.ReadProperty().Value;
                node.Rh = (double) reader.ReadProperty().Value;
                node.Rx = (double) reader.ReadProperty().Value;

                var reset = reader.ReadProperty().Value;
                if (reset != null)
                    node.ResetGate = Ject.FindType(reset.ToString()).CreateDefault<IFunction>();

                node.Z = (double) reader.ReadProperty().Value;
                node.Zb = (double) reader.ReadProperty().Value;
                node.Zh = (double) reader.ReadProperty().Value;
                node.Zx = (double) reader.ReadProperty().Value;

                var update = reader.ReadProperty().Value;
                if (update != null)
                    node.UpdateGate = Ject.FindType(update.ToString()).CreateDefault<IFunction>();

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

                writer.WriteProperty(nameof(RecurrentNeuron.H), node.H);
                writer.WriteProperty(nameof(RecurrentNeuron.Hh), node.Hh);

                writer.WriteProperty(nameof(RecurrentNeuron.R), node.R);
                writer.WriteProperty(nameof(RecurrentNeuron.Rb), node.Rb);
                writer.WriteProperty(nameof(RecurrentNeuron.Rh), node.Rh);
                writer.WriteProperty(nameof(RecurrentNeuron.Rx), node.Rx);

                writer.WriteProperty(nameof(RecurrentNeuron.ResetGate), node.ResetGate.GetType().FullName);

                writer.WriteProperty(nameof(RecurrentNeuron.Z), node.Z);
                writer.WriteProperty(nameof(RecurrentNeuron.Zb), node.Zb);
                writer.WriteProperty(nameof(RecurrentNeuron.Zh), node.Zh);
                writer.WriteProperty(nameof(RecurrentNeuron.Zx), node.Zx);

                writer.WriteProperty(nameof(RecurrentNeuron.UpdateGate), node.UpdateGate.GetType().FullName);
            }
        }
    }
}
