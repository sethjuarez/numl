// file:	Math\Functions\Ident.cs
//
// summary:	Implements the identifier class
using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.Functions
{
    /// <summary>An identifier / linear function.</summary>
    public class Ident : Function
    {
        /// <summary>
        /// Returns the minimum value from the function curve (-1.7976e^308).
        /// </summary>
        public override double Minimum { get { return double.MinValue; } }

        /// <summary>
        /// Returns the maximum value from the function curve (1.7976e^308).
        /// </summary>
        public override double Maximum { get { return double.MaxValue; } }

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
