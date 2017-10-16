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
        /// Returns the minimum value from the function curve, equal to 0.0.
        /// </summary>
        public override double Minimum { get { return 0; } }

        /// <summary>
        /// Returns the maximum value from the function curve, equal to infinity.
        /// </summary>
        public override double Maximum { get { return double.PositiveInfinity; } }

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
        /// <param name="cached">If True, uses the previously computed activation.</param>
        /// <returns>Double.</returns>
        public override double Derivative(double x, bool cached = false)
        {
            double c = (cached ? x : Compute(x));
            return 1.0 + (1.0 / (-exp(c)));
        }
    }
}
