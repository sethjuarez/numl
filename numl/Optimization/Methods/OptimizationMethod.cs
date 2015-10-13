using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using numl.Optimization.Functions;

namespace numl.Optimization.Methods
{
    /// <summary>
    /// Implements an optimization method when overridden in a derived class.
    /// </summary>
    public abstract class OptimizationMethod : IOptimizationMethod
    {
        /// <summary>
        /// Returns a boolean value indicating whether to keep optimizing.
        /// <para>Used for optimization routines with early stopping.</para>
        /// </summary>
        /// <returns>Boolean</returns>
        public virtual bool Update(OptimizerProperties properties)
        {
            return (properties.Iteration <= properties.MaxIterations || 
                (properties.Iteration > 1 ? 
                    properties.Cost < (properties.CostHistory[properties.Iteration - 1] - properties.MinimizationConstant)
                    : true));
        }

        /// <summary>
        /// Update and return the Cost.
        /// </summary>
        /// <param name="costFunction">The cost function to optimize.</param>
        /// <param name="properties">Properties for the optimization routine.</param>
        /// <returns>Double</returns>
        public virtual double UpdateCost(ICostFunction costFunction, OptimizerProperties properties)
        {
            return costFunction.ComputeCost(properties.Theta);
        }

        /// <summary>
        /// Update and return the Gradient.
        /// </summary>
        /// <param name="costFunction">The cost function to optimize.</param>
        /// <param name="properties">Properties for the optimization routine.</param>
        /// <returns>Vector</returns>
        public virtual Vector UpdateGradient(ICostFunction costFunction, OptimizerProperties properties)
        {
            return costFunction.ComputeGradient(properties.Theta);
        }

        /// <summary>
        /// Update and return the new Theta value.
        /// </summary>
        /// <param name="properties">Properties for the optimization routine.</param>
        /// <returns></returns>
        public abstract Vector UpdateTheta(OptimizerProperties properties);
    }
}
