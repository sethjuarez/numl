using numl.Supervised.NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace numl.Supervised.NeuralNetwork
{
    /// <summary>
    /// Network Training Properties class.
    /// </summary>
    public class NetworkTrainingProperties
    {
        private readonly Dictionary<string, object> _Parameters;

        /// <summary>
        /// Gets or sets a reference to the network being optimized.
        /// </summary>
        public Network Network { get; set; }

        /// <summary>
        /// Gets or sets the number of training examples.
        /// </summary>
        public int Examples { get; set; }

        /// <summary>
        /// Gets or sets the number of features.
        /// </summary>
        public int Features { get; set; }

        /// <summary>
        /// Gets or sets the learning rate (alpha).
        /// </summary>
        public double LearningRate { get; set; }

        /// <summary>
        /// Gets or sets the Momentum parameter.
        /// </summary>
        public double Momentum { get; set; }

        /// <summary>
        /// Gets or sets the lambda (weight decay) parameter.
        /// </summary>
        public double Lambda { get; set; }

        /// <summary>
        /// Gets or sets an optional Epsilon parameter.
        /// </summary>
        public double Epsilon { get; set; }

        /// <summary>
        /// Gets or sets the current iteration.
        /// </summary>
        public int Iteration { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
        public int MaxIterations { get; private set; }

        /// <summary>
        /// Gets or sets the specified parameter for use in training.
        /// </summary>
        public object this[string name]
        {
            get
            {
                return this._Parameters[name];
            }
            set
            {
                this._Parameters[name] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkTrainingProperties"/> class.
        /// <param name="parameters">Optional custom properties used in training.</param>
        /// </summary>
        public NetworkTrainingProperties(Dictionary<string, object> parameters = null)
        {
            this.LearningRate = 0.1;
            this.Momentum = 0.9;
            this.Epsilon = 0.9;

            this._Parameters = parameters;
        }

        /// <summary>
        /// Creates a new Network Properties object for use in training a neural network model.
        /// </summary>
        /// <param name="network">Network being optimized.</param>
        /// <param name="examples">Number of training examples.</param>
        /// <param name="features">Number of features in the training examples.</param>
        /// <param name="learningRate">Learning rate.</param>
        /// <param name="lambda">Lambda (weight decay).</param>
        /// <param name="maxIterations">Maximum number of iterations.</param>
        /// <param name="parameters">Optional parameters object used when training networks.
        ///     <para>Usage: parameters = new { Obj1, Obj2, Obj3... }</para>
        /// </param>
        /// <returns></returns>
        public static NetworkTrainingProperties Create(Network network, int examples, int features, double learningRate, 
            double lambda, int maxIterations, object parameters = null)
        {
            Dictionary<string, object> props = new Dictionary<string, object>();
            if (parameters != null)
            {
                foreach (var prop in parameters.GetType().GetProperties())
                {
                    props[prop.Name] = prop.GetValue(parameters);
                }
            }

            return new NetworkTrainingProperties(props)
            {
                Network = network,
                Examples = examples,
                Features = features,
                Lambda = lambda,
                LearningRate = learningRate
            };
        }
    }
}
