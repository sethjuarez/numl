using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using numl.Utils;

namespace numl.Supervised.NeuralNetwork.Optimization
{
    /// <summary>
    /// RMSProp Network Trainer.
    /// </summary>
    public class RMSPropTrainer : INetworkTrainer
    {
        /// <summary>
        /// Dictionary of per-parameter means.
        /// </summary>
        protected readonly Dictionary<string, double> Mu = new Dictionary<string, double>();

        /// <summary>
        /// Applies an update using RMSProp to theta w.r.t the gradient for the specified node in the layer.
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
            
            Mu[label] = properties.Momentum * Mu.GetValueOrDefault(label, 0) + ((1.0 - properties.Momentum) * System.Math.Pow(gradient, 2.0));
            return theta - properties.LearningRate * gradient / (System.Math.Sqrt(Mu[label]) + properties.Epsilon);
        }
    }
}
