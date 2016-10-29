using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using numl.Utils;

namespace numl.Supervised.NeuralNetwork.Optimization
{
    /// <summary>
    /// Fast Gradient Descent Network Trainer.
    /// </summary>
    public class FastGradientDescentTrainer : INetworkTrainer
    {
        /// <summary>
        /// Dictionary of per-parameter velocities.
        /// </summary>
        private readonly Dictionary<string, double> Omega = new Dictionary<string, double>();

        /// <summary>
        /// Applies an update using accelerated gradient descent to theta w.r.t the gradient for the specified node in the layer.
        /// </summary>
        /// <param name="sourceId">Source node identifier.</param>
        /// <param name="targetId">Target node identifier.</param>
        /// <param name="paramName">Name of the theta parameter being optimized.</param>
        /// <param name="theta">Current theta or weight value.</param>
        /// <param name="gradient">Current gradient value.</param>
        /// <param name="properties">Networking training properties instance.</param>
        /// <returns>double.</returns>
        public double Update(int sourceId, int targetId, string paramName, double theta, double gradient, NetworkTrainingProperties properties)
        {
            string label = $"{sourceId}:{targetId}:{paramName}";

            this.Omega[label] = properties.Momentum * this.Omega.GetValueOrDefault(label, 0) - properties.LearningRate * gradient;
            return theta - this.Omega[label];
        }
    }
}
