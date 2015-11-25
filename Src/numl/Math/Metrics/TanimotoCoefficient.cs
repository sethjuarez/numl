// file:	Math\Metrics\TanimotoCoefficient.cs
//
// summary:	Implements the tanimoto coefficient class
using System;
using numl.Math.LinearAlgebra;

namespace numl.Math.Metrics
{
    /// <summary>A tanimoto coefficient.</summary>
    public sealed class TanimotoCoefficient : ISimilarity
    {
        /// <summary>Computes.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        public double Compute(Vector x, Vector y)
        {
            double dot = x.Dot(y);
            return dot / (System.Math.Pow(x.Norm(), 2) + System.Math.Pow(y.Norm(), 2) - dot);
        }
    }
}
