// file:	Math\Metrics\HammingDistance.cs
//
// summary:	Implements the hamming distance class
using System;
using numl.Math.LinearAlgebra;

namespace numl.Math.Metrics
{
    /// <summary>A hamming distance.</summary>
    public sealed class HammingDistance : IDistance
    {
        /// <summary>Computes.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        public double Compute(Vector x, Vector y)
        {
            if (x.Length != y.Length)
                throw new InvalidOperationException("Cannot compute distance between two unequally sized Vectors!");

            double sum = 0;
            for (int i = 0; i < x.Length; i++)
                if (x[i] != y[i])
                    sum++;
            return sum;
        }
    }
}
