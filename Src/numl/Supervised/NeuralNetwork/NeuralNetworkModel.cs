// file:	Supervised\NeuralNetwork\NeuralNetworkModel.cs
//
// summary:	Implements the neural network model class
using System;
using System.Linq;
using numl.Serialization;
using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>A data Model for the neural network.</summary>
    public class NeuralNetworkModel : Model, ISequenceModel
    {
        /// <summary>Gets or sets the network.</summary>
        /// <value>The network.</value>
        public Network Network { get; set; }

        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        public override double Predict(Vector x)
        {
            Vector output = this.PredictSequence(x);
            return (output.Length > 1 ? output.MaxIndex() : output.First());
        }

        /// <summary>
        /// Predicts the given x.
        /// </summary>
        /// <param name="x">Vector of features.</param>
        /// <returns>Vector.</returns>
        public virtual Vector PredictSequence(Vector x)
        {
            this.Preprocess(x);

            this.Network.Forward(x);

            Vector output = Network.Output();

            return output;
        }
    }
}
