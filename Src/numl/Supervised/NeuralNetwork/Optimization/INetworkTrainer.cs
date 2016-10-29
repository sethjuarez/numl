using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace numl.Supervised.NeuralNetwork.Optimization
{
    /// <summary>
    /// INetworkTrainer interface.
    /// </summary>
    public interface INetworkTrainer
    {
        /// <summary>
        /// Applies an update to theta w.r.t the gradient for the specified node in the layer.
        /// </summary>
        /// <param name="sourceId">Source node identifier.</param>
        /// <param name="targetId">Target node identifier.</param>
        /// <param name="paramName">Name of the theta parameter being optimized.</param>
        /// <param name="theta">Current theta or weight value.</param>
        /// <param name="gradient">Current gradient value.</param>
        /// <param name="properties">Networking training properties instance.</param>
        /// <returns>double.</returns>
        double Update(int sourceId, int targetId, string paramName, double theta, double gradient, NetworkTrainingProperties properties);
    }
}
