using System;
using numl.Math.LinearAlgebra;

namespace numl.Math.Metrics
{
    public sealed class EuclidianSimilarity : ISimilarity
    {
        public double Compute(Vector x, Vector y)
        {
            return 1 / (1 + (x - y).Norm(2));
        }
    }
}
