// file:	Math\Functions\Logistic.cs
//
// summary:	Implements the logistic class

namespace numl.Math.Functions
{
    /// <summary>A Swish activation.</summary>
    public class Swish : Function
    {
        /// <summary>
        /// Returns the minimum value from the function curve, equal to 0.0.
        /// </summary>
        public override double Minimum { get { return 0; } }

        /// <summary>
        /// Returns the maximum value from the function curve, equal to positive infinity.
        /// </summary>
        public override double Maximum { get { return double.PositiveInfinity; } }

        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Compute(double x)
        {
            return x * Sigmoid(x);
        }

        private double Sigmoid(double x)
        {
            return (1d / (1d + exp(-x)));
        }

        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The input to the function.</param>
        /// <param name="y">Precomputed swish output.</param>
        /// <returns>Double.</returns>
        public override double Derivative(double x, double y)
        {
            return y + Sigmoid(x) * (1d - y);
        }
    }
}
