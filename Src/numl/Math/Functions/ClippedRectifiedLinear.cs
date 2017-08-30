// file:	Math\Functions\RectifiedLinear.cs
//
// summary:	Implements the rectified linear class

namespace numl.Math.Functions
{
    /// <summary>A clipped rectified linear function.</summary>
    public class ClippedRectifiedLinear : Function
    {
        /// <summary>
        /// Gets or sets the maximum ouput value (default is 5).
        /// </summary>
        public double MaxValue { get; set; }

        /// <summary>
        /// Returns the minimum value from the function curve.
        /// </summary>
        public override double Minimum { get { return 0; } }

        /// <summary>
        /// Returns the maximum value from the function curve.  Default is <see cref="MaxValue"/>.
        /// </summary>
        public override double Maximum { get { return MaxValue; } }

        /// <summary>
        /// Initializes a new Clipped Rectified Linear function with default maximum of 5.
        /// </summary>
        public ClippedRectifiedLinear()
        {
            MaxValue = 5d;
        }
        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Compute(double x)
        {
            return (x > 0d ? (x > MaxValue ? MaxValue : x) : 0d);
        }
        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Derivative(double x)
        {
            return (x > 0d && x < MaxValue ? 1 : 0);
        }
    }
}
