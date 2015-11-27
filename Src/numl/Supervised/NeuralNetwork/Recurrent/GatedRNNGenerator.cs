using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;

namespace numl.Supervised.NeuralNetwork.Recurrent
{
    /// <summary>
    /// A Gated Recurrent Unit neural network generator.
    /// </summary>
    public class GatedRNNGenerator : Generator
    {
        /// <summary>
        /// Generates a GRU neural network from the training set.
        /// </summary>
        /// <param name="x">The Matrix of example data.</param>
        /// <param name="y">The vector of example labels.</param>
        /// <returns></returns>
        public override IModel Generate(Matrix x, Vector y)
        {
            throw new NotImplementedException();
        }
    }
}
