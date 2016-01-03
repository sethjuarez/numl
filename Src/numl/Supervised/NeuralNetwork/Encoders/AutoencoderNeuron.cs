using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Supervised.NeuralNetwork.Encoders
{
    /// <summary>
    /// An autoencoding node.
    /// </summary>
    public class AutoencoderNeuron : Node
    {
        /// <summary>
        /// Gets or sets the sparsity constraint (KL divergence property) used in the weight update function.
        /// </summary>
        public double Sparsity { get; set; }

        /// <summary>Calculates and returns the Node's <see cref="Output"/> value.</summary>
        /// <remarks>Input is equal to the weights multiplied by the source <see cref="Node"/>'s Output.</remarks>
        /// <returns>A double.</returns>
        public override double Evaluate()
        {
            return base.Evaluate();
        }

        /// <summary>Calculates and returns the error derivative (<see cref="Delta"/>) of this node.</summary>
        /// <param name="t">The double to process.</param>
        /// <returns>A double.</returns>
        public override double Error(double t)
        {
            return base.Error(t);
        }

        /// <summary>Propagates a weight update event upstream through the network using the supplied learning rate.</summary>
        /// <param name="properties">Network training properties.</param>
        public override void Update(NetworkTrainingProperties properties)
        {
            base.Update(properties);
        }
    }
}
