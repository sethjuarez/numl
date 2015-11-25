using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Optimization.Methods.GradientDescent
{
    /// <summary>
    /// A Stochastic Gradient Descent method.
    /// </summary>
    public class StochasticGradientDescent : OptimizationMethod
    {
        /// <summary>
        /// Update and return the new Theta value.
        /// </summary>
        /// <param name="properties">Properties for the optimization routine.</param>
        /// <returns></returns>
        public override Vector UpdateTheta(OptimizerProperties properties)
        {
            if (properties.Cost > properties.CostHistory.Last())
            {
                properties.LearningRate *= 0.9;
                return properties.Theta;
            }
            else
            {
                return properties.Theta - properties.LearningRate * properties.Gradient;
            }
        }
    }
}
