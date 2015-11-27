// file:	Math\Functions\RectifiedLinear.cs
//
// summary:	Implements the rectified linear class
using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.Functions
{
    /// <summary>A rectified linear function.</summary>
    public class RectifiedLinear : Function
    {
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
            return 1 - Compute(x);
        }
    }
}
