// file:	Supervised\NeuralNetwork\NeuralNetworkModel.cs
//
// summary:	Implements the neural network model class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using System.Xml;
using numl.Utils;
using numl.Preprocessing;
using numl.Math.Functions;
using Newtonsoft.Json;
using numl.Serialization;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>A data Model for the neural network.</summary>
    public class NeuralNetworkModel : Model
    {
        /// <summary>Gets or sets the network.</summary>
        /// <value>The network.</value>
        [JsonConverter(typeof(NetworkConverter))]
        public Network Network { get; set; }

        /// <summary>
        /// Gets or sets the output layer function (i.e. Softmax).
        /// </summary>
        public IFunction OutputFunction { get; set; }
        // TODO: should likely think about this in the case of multiple outputs
        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        public override double Predict(Vector y)
        {
            Network.Forward(y);

            Vector output = Network.Out.Select(n => n.Output).ToVector();

            return (this.OutputFunction != null ? this.OutputFunction.Compute(output).Max() : output.Max());
        }
    }
}
