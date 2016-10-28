// file:	Math\Metrics\PearsonCorrelation.cs
//
// summary:	Implements the pearson correlation class
using System;
using numl.Math.LinearAlgebra;

namespace numl.Math.Metrics
{
    /// <summary>A pearson correlation.</summary>
    public sealed class PearsonCorrelation : ISimilarity
    {
        /// <summary>Computes.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        public double Compute(Vector x, Vector y)
        {
            if (x.Length != y.Length)
                throw new InvalidOperationException("Cannot compute similarity between two unequally sized Vectors!");

            var xSum = x.Sum();
            var ySum = y.Sum();
            
            var xElem = System.Math.Pow(x.Norm(), 2) - ((xSum * xSum) / x.Length);
            var yElem = System.Math.Pow(y.Norm(), 2) - ((ySum * ySum) / y.Length);

            return (x.Dot(y) - ((xSum * ySum) / x.Length)) / System.Math.Sqrt(xElem * yElem);
        }
    }
}
