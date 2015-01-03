// file:	Math\Metrics\EuclidianSimilarity.cs
//
// summary:	Implements the euclidian similarity class
using System;
using numl.Math.LinearAlgebra;

namespace numl.Math.Metrics
{
    /// <summary>An euclidian similarity.</summary>
    public sealed class EuclidianSimilarity : ISimilarity
    {
        /// <summary>Computes.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        public double Compute(Vector x, Vector y)
        {
            return 1 / (1 + (x - y).Norm(2));
        }
    }
}
