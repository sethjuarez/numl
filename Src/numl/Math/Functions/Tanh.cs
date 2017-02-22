// file:	Math\Functions\Tanh.cs
//
// summary:	Implements the hyperbolic tangent class

namespace numl.Math.Functions
{
    /// <summary>A hyperbolic tangent.</summary>
    public class Tanh : Function
    {
        /// <summary>
        /// Returns the minimum value from the function curve, equal to -1.0.
        /// </summary>
        public override double Minimum { get { return -1; } }

        /// <summary>
        /// Returns the maximum value from the function curve, equal to 1.0.
        /// </summary>
        public override double Maximum { get { return 1; } }

        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Compute(double x)
        {
            return (exp(x) - exp(-x)) / (exp(x) + exp(-x));
        }
        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Derivative(double x)
        {
            return 1 - pow(Compute(x), 2);
        }
    }
}
