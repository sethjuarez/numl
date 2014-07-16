// file:	Math\Functions\Tanh.cs
//
// summary:	Implements the hyperbolic tangent class
using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.Functions
{
    /// <summary>A hyperbolic tangent.</summary>
    public class Tanh : Function
    {
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
