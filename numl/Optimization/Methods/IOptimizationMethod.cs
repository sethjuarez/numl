using numl.Math.LinearAlgebra;
using numl.Optimization.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Optimization
{
    /// <summary>
    /// Implements an optimization method.
    /// </summary>
    public interface IOptimizationMethod
    {
        /// <summary>
        /// Returns a boolean value indicating to keep updating.
        /// <para>Used for optimization routines with early stopping.</para>
        /// </summary>
        /// <returns>Boolean</returns>
        bool Update(OptimizerProperties properties);

        /// <summary>
        /// Update and return the Cost.
        /// </summary>
        /// <param name="costFunction">The cost function to optimize.</param>
        /// <param name="properties">Properties for the optimization routine.</param>
        /// <returns>Double</returns>
        double UpdateCost(ICostFunction costFunction, OptimizerProperties properties);

        /// <summary>
        /// Update and return the Gradient.
        /// </summary>
        /// <param name="costFunction">The cost function to optimize.</param>
        /// <param name="properties">Properties for the optimization routine.</param>
        /// <returns>Vector</returns>
        Vector UpdateGradient(ICostFunction costFunction, OptimizerProperties properties);

        /// <summary>
        /// Update and return the new Theta value.
        /// </summary>
        /// <param name="properties">Properties for the optimization routine.</param>
        /// <returns></returns>
        Vector UpdateTheta(OptimizerProperties properties);
    }
}
