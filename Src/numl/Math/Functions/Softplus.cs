using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Math.Functions
{
    /// <summary>
    /// Softplus function
    /// </summary>
    public class Softplus : Function
    {
        /// <summary>
        /// Computes the softplus function on the given input.
        /// </summary>
        /// <param name="x">The double to process.</param>
        /// <returns>Double.</returns>
        public override double Compute(double x)
        {
            return System.Math.Log(1d + exp(x));
        }

        /// <summary>
        /// Derivatives the given x coordinate.
        /// </summary>
        /// <param name="x">The double to process.</param>
        /// <returns>Double.</returns>
        public override double Derivative(double x)
        {
            return 1d / (1d + exp(-x));
        }
    }
}
