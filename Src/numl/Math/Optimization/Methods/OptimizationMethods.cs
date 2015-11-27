using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Math.Optimization.Methods
{
    /// <summary>
    /// Lists available builtin optimizers
    /// </summary>
    public enum OptimizationMethods
    {
        /// <summary>
        /// An external optimization method
        /// </summary>
        External,
        /// <summary>
        /// Standard stochastic gradient descent method
        /// </summary>
        StochasticGradientDescent,
        /// <summary>
        /// Stochastic gradient descent method with momentum
        /// </summary>
        FastGradientDescent,
        /// <summary>
        /// Nesterov accelerated gradient descent
        /// </summary>
        NAGDescent,
    }
}
