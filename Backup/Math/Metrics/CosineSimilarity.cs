// file:	Math\Metrics\CosineSimilarity.cs
//
// summary:	Implements the cosine similarity class
using System;
using numl.Math.LinearAlgebra;

namespace numl.Math.Metrics
{
    /// <summary>A cosine similarity.</summary>
    public sealed class CosineSimilarity : ISimilarity
    {
        /// <summary>Computes.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        public double Compute(Vector x, Vector y)
        {
            return x.Dot(y) / (x.Norm() * y.Norm());
        }
    }
}
