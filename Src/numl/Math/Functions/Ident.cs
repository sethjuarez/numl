// file:	Math\Functions\Ident.cs
//
// summary:	Implements the identifier class
namespace numl.Math.Functions
{
    /// <summary>An identifier / linear function.</summary>
    public class Ident : Function
    {
        /// <summary>
        /// Returns the minimum value from the function curve.
        /// </summary>
        public override double Minimum { get { return double.NegativeInfinity; } }

        /// <summary>
        /// Returns the maximum value from the function curve.
        /// </summary>
        public override double Maximum { get { return double.PositiveInfinity; } }

        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Compute(double x)
        {
            return x;
        }
        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Derivative(double x)
        {
            return 1;
        }
    }
}
