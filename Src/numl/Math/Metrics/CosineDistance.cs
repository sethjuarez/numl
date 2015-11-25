// file:	Math\Metrics\CosineDistance.cs
//
// summary:	Implements the cosine distance class
using System;
using numl.Math.LinearAlgebra;

namespace numl.Math.Metrics
{
    /// <summary>A cosine distance.</summary>
    public sealed class CosineDistance : IDistance
    {
        /// <summary>Computes.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        public double Compute(Vector x, Vector y)
        {   
            return 1d - (x.Dot(y) / (x.Norm() * y.Norm()));
        }
    }
}
