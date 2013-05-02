using System;
using numl.Math.LinearAlgebra;

namespace numl.Math.Metrics
{
    public sealed class PearsonCorrelation : ISimilarity
    {
        public double Compute(Vector x, Vector y)
        {
            if (x.Length != y.Length)
                throw new InvalidOperationException("Cannot compute similarity between two unequally sized Vectors!");

            var xSum = x.Sum();
            var ySum = y.Sum();
            
            var xElem = (x^2).Sum() - ((xSum * xSum) / x.Length);
            var yElem = (y^2).Sum() - ((ySum * ySum) / y.Length);

            return (x.Dot(y) - ((xSum * ySum) / x.Length)) / System.Math.Sqrt(xElem * yElem);
        }
    }
}
