using System;
using numl.Math.LinearAlgebra;


namespace numl.Math.Metrics
{
    public sealed class EuclidianDistance : IDistance
    {
        public double Compute(Vector x, Vector y)
        {
            return (x - y).Norm();
        }
    }
}
