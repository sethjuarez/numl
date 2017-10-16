// file:	Math\Functions\Logistic.cs
//
// summary:	Implements the logistic class

namespace numl.Math.Functions
{
    /// <summary>A steep logistic function.</summary>
    public class SteepLogistic : Function
    {
        /// <summary>
        /// Returns the minimum value from the function curve, equal to 0.0.
        /// </summary>
        public override double Minimum { get { return 0; } }

        /// <summary>
        /// Returns the maximum value from the function curve, equal to 1.0.
        /// </summary>
        public override double Maximum { get { return 1; } }

        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Compute(double x)
        {
            return 1d / (1d + exp(-System.Math.PI * x));
        }
        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <param name="cached">If True, uses the previously computed activation.</param>
        /// <returns>A Vector.</returns>
        public override double Derivative(double x, bool cached = false)
        {
            var c = (cached ? x : Compute(x));
            return System.Math.PI * c * (1d - c);
        }
    }
}
