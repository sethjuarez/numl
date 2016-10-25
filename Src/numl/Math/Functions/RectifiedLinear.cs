// file:	Math\Functions\RectifiedLinear.cs
//
// summary:	Implements the rectified linear class

namespace numl.Math.Functions
{
    /// <summary>A rectified linear function.</summary>
    public class RectifiedLinear : Function
    {
        /// <summary>
        /// Returns the minimum value from the function curve, equal to 0.0.
        /// </summary>
        public override double Minimum { get { return 0; } }

        /// <summary>
        /// Returns the maximum value from the function curve, equal to infinity.
        /// </summary>
        public override double Maximum { get { return double.MaxValue; } }

        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The value to process.</param>
        /// <returns>A double.</returns>
        public override double Compute(double x)
        {
            return (x > 0d ? x : 0d);
        }
        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The value to process.</param>
        /// <returns>A double.</returns>
        public override double Derivative(double x)
        {
            return (x > 0d ? 1 : 0);
        }
    }
}
