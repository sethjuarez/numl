// file:	Math\Functions\Function.cs
//
// summary:	Implements the function class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Functions
{
    /// <summary>A function.</summary>
    public abstract class Function : IFunction
    {
        /// <summary>Exps.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A double.</returns>
        internal double exp(double x) { return System.Math.Exp(x); }

        /// <summary>Pows.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <param name="a">The double to process.</param>
        /// <returns>A double.</returns>
        internal double pow(double x, double a) { return System.Math.Pow(x, a); }

        /// <summary>
        /// Returns the minimum value from the function curve.
        /// </summary>
        public abstract double Minimum { get; }

        /// <summary>
        /// Returns the maximum value from the function curve.
        /// </summary>
        public abstract double Maximum { get; }

        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public abstract double Compute(double x);

        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public abstract double Derivative(double x);

        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public Vector Compute(Vector x)
        {
            return x.Calc(d => Compute(d));
        }
        /// <summary>Derivatives the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A Vector.</returns>
        public Vector Derivative(Vector x)
        {
            return x.Calc(d => Derivative(d));
        }

        /// <summary>
        /// Sums the given x coordinate.
        /// </summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>Double.</returns>
        public virtual double Minimize(Vector x)
        {
            return x.Calc(d => Compute(d)).Sum();
        }
    }
}
