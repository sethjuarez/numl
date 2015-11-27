// file:	Math\Functions\RectifiedLinear.cs
//
// summary:	Implements the rectified linear class
using System;
using System.Linq;
using System.Collections.Generic;

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
        /// Initializes a new Clipped Rectified Linear function with default maximum of 5.
        /// </summary>
        public ClippedRectifiedLinear()
        {
            this.MaxValue = 5d;
        }
        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Compute(double x)
        {
            return (x > 0d ? (x > this.MaxValue ? this.MaxValue : x) : 0d);
        }
        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public override double Derivative(double x)
        {
            return 1 - Compute(x);
        }
    }
}
