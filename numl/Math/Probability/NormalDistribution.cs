// file:	Math\Probability\NormalDistribution.cs
//
// summary:	Implements the normal distribution class
using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Math.Probability
{
    /// <summary>A normal distribution.</summary>
    public class NormalDistribution
    {
        /// <summary>Gets or sets the mu.</summary>
        /// <value>The mu.</value>
        public Vector Mu { get; set; }
        /// <summary>Gets or sets the sigma.</summary>
        /// <value>The sigma.</value>
        public Matrix Sigma { get; set; }
        /// <summary>Estimates.</summary>
        /// <param name="X">The Matrix to process.</param>
        /// <param name="type">(Optional) the type.</param>
        public void Estimate(Matrix X, VectorType type = VectorType.Row)
        {
            int n = type == VectorType.Row ? X.Rows : X.Cols;
            int s = type == VectorType.Row ? X.Cols : X.Rows;
            Mu = X.Sum(type) / n;
            Sigma = Matrix.Zeros(s);
            
            for (int i = 0; i < n; i++)
            {
                var x = X[i, type] - Mu;
                Sigma += x.Outer(x);
            }

            Sigma *= (1d / (n - 1d));
        }
        /// <summary>Computes the given x coordinate.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>A double.</returns>
        public double Compute(Vector x)
        {
            return 0;
        }
    }
}
