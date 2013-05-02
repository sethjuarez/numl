using System;
using numl.Math.LinearAlgebra;

namespace numl.Math.Metrics
{
    public sealed class CosineSimilarity : ISimilarity
    {
        public double Compute(Vector x, Vector y)
        {
            return x.Dot(y) / (x.Norm() * y.Norm());
        }
    }
}
