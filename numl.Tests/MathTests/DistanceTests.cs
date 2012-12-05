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
        [TestCase(new[] { 1d, 5, 2, 3, 10 }, new[] { 4d, 15, 20, 5, 5 }, typeof(CosineDistance), 0.40629)]
        [TestCase(new[] { 1d, 2, 3 }, new[] { 2d, 4, 6 }, typeof(EuclidianDistance), 3.7416573867739413855837487323165)]
        [TestCase(new[] { 1d, 0, 0, 1, 1 }, new[] { 0d, 0, 1, 0, 1 }, typeof(HammingDistance), 3)]
        public void Distance_Test(double[] x, double[] y, Type t, double truth)
        {
            Assert.IsTrue(t.GetInterfaces().Contains(typeof(IDistance)));

            var distance = (IDistance)Activator.CreateInstance(t);
            var result = distance.Compute(new DenseVector(x), new DenseVector(y));

            truth = System.Math.Round(truth, 4);
            result = System.Math.Round(result, 4);

            Assert.AreEqual(truth, result);
        }
    }
}
