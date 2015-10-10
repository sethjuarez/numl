// file:	Supervised\NeuralNetwork\NeuralNetworkModel.cs
//
// summary:	Implements the neural network model class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using Newtonsoft.Json;
using numl.Serialization;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>A data Model for the neural network.</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class NeuralNetworkModel : Model
    {
        /// <summary>Gets or sets the network.</summary>
        /// <value>The network.</value>
        [JsonProperty]
        [JsonConverter(typeof(NetworkConverter))]
        public Network Network { get; set; }
        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        public override double Predict(Vector y)
        {
            Network.Forward(y);
            return Network.Out.Select(n => n.Output).Max();
        }

    }
}
