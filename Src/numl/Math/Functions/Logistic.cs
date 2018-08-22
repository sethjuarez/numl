// file:	Math\Functions\Logistic.cs
//
// summary:	Implements the logistic class

namespace numl.Math.Functions
{
    /// <summary>A logistic.</summary>
    public class Logistic : Function
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
        /// <param name="x">The value to process.</param>
        /// <returns>A double.</returns>
        public override double Compute(double x)
        {
            return 1d / (1d + exp(-x));
        }

        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The input to the function.</param>
        /// <param name="y">Precomputed logistic output.</param>
        /// <returns>Double.</returns>
        public override double Derivative(double x, double y)
        {
            return y * (1d - y);
        }
    }
}
