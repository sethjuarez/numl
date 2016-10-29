using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using numl.Utils;

namespace numl.Supervised.NeuralNetwork.Optimization
{
    /// <summary>
    /// AdaGrad Network Trainer.
    /// </summary>
    public class AdaGradTrainer : INetworkTrainer
    {
        /// <summary>
        /// Dictionary of per-parameter squares.
        /// </summary>
        protected readonly Dictionary<string, double> Mu = new Dictionary<string, double>();

        /// <summary>
        /// Applies an update using AdaGrad to theta w.r.t the gradient for the specified node in the layer.
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

            this.Mu[label] = this.Mu.GetValueOrDefault(label, 0) + System.Math.Pow(gradient, 2.0);
            return theta - properties.LearningRate * gradient / (System.Math.Sqrt(this.Mu[label]) + properties.Epsilon);
        }
    }
}
