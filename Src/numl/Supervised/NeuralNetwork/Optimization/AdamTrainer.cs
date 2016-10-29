using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using numl.Utils;

namespace numl.Supervised.NeuralNetwork.Optimization
{
    /// <summary>
    /// Adam Network Trainer.
    /// </summary>
    public class AdamTrainer : INetworkTrainer
    {
        /// <summary>
        /// Dictionary of per-parameter M.
        /// </summary>
        protected readonly Dictionary<string, double> Mu = new Dictionary<string, double>();

        /// <summary>
        /// Dictionary of per-parameter V.
        /// </summary>
        protected readonly Dictionary<string, double> Tau = new Dictionary<string, double>();

        /// <summary>
        /// Gets or sets the beta parameter.
        /// </summary>
        public double Beta { get; set; } = 0.9;

        /// <summary>
        /// Gets or sets the gamma parameter.
        /// </summary>
        public double Gamma { get; set; } = 0.99;

        /// <summary>
        /// Applies an update using Adam to theta w.r.t the gradient for the specified node in the layer.
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

            Mu[label] = (this.Beta * this.Mu.GetValueOrDefault(label, 0.0)) + ((1.0 - this.Beta) * gradient);
            Tau[label] = (this.Gamma * this.Tau.GetValueOrDefault(label, 0.0)) + ((1.0 - this.Gamma) * (gradient * gradient));
            return theta - (properties.LearningRate * this.Mu[label] / (System.Math.Sqrt(Tau[label]) + properties.Epsilon));
        }
    }
}
