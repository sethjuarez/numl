using MathNet.Numerics.LinearAlgebra.Double;
using numl.Math.Metrics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.MathTests
{
    [TestFixture]
    public class MetricTests
    {
        [TestCase(new[] { 1d, 5, 2, 3, 10 }, new[] { 4d, 15, 20, 5, 5 }, 0.40629)]
        public void Cosine_Distance(double[] x, double[] y, double truth)
        {
            CosineDistance distance = new CosineDistance();
            var result = distance.Compute(new DenseVector(x), new DenseVector(y));
            Assert.AreEqual(System.Math.Round(truth, 4), System.Math.Round(result, 4));
        }

        [TestCase(new[] { 1d, 2, 3 }, new[] { 2d, 4, 6 }, 3.7416573867739413855837487323165)]
        public void Euclidian_Distance(double[] x, double[] y, double truth)
        {
            EuclidianDistance distance = new EuclidianDistance();
            var result = distance.Compute(new DenseVector(x), new DenseVector(y));
            Assert.AreEqual(System.Math.Round(truth, 4), System.Math.Round(result, 4));
        }

        [TestCase(new[] { 1d, 0, 0, 1, 1 }, new[] { 0d, 0, 1, 0, 1 }, 3)]
        public void Hamming_Distance(double[] x, double[] y, double truth)
        {
            HammingDistance distance = new HammingDistance();
            var result = distance.Compute(new DenseVector(x), new DenseVector(y));
            Assert.AreEqual(System.Math.Round(truth, 4), System.Math.Round(result, 4));
        }
    }
}
