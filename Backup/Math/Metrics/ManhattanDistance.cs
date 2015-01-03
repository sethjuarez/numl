// file:	Math\Metrics\ManhattanDistance.cs
//
// summary:	Implements the manhattan distance class
using System;
using numl.Math.LinearAlgebra;

namespace numl.Math.Metrics
{
    /// <summary>A manhattan distance.</summary>
    public sealed class ManhattanDistance : IDistance
    {
        /// <summary>Computes.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        public double Compute(Vector x, Vector y)
        {
            return (x - y).Norm(1);
        }
    }
}
