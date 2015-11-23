using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using numl.Optimization.Functions;

namespace numl.Optimization.Methods
{
    /// <summary>
    /// A Stochastic Gradient Descent with Momentum method.
    /// </summary>
    public class FastGradientDescent : OptimizationMethod
    {
        /// <summary>
        /// Defines the Momentum to use.
        /// </summary>
        public double Momentum { get; set; }

        /// <summary>
        /// Initializes a new FastGradientDescent object with a default Momentum of 0.9
        /// </summary>
        public FastGradientDescent()
        {
            this.Momentum = 0.9;
        }

        /// <summary>
        /// Update and return the new Theta value.
        /// </summary>
        /// <param name="properties">Properties for the optimization routine.</param>
        /// <returns></returns>
        public override Vector UpdateTheta(OptimizerProperties properties)
        {
            Vector v = (this.Momentum * properties.Theta) - (properties.LearningRate * properties.Gradient);
            return properties.Theta + v;
        }
    }
}
