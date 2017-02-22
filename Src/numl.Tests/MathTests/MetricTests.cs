using System;
using System.Linq;
using Xunit;
using numl.Math.Metrics;
using System.Reflection;
using numl.Math.LinearAlgebra;

namespace numl.Tests.MathTests
{
    [Trait("Category", "Math")]
    public class MetricTests
    {
        [Theory]
        [InlineData(new[] { 1d, 5, 2, 3, 10 }, new[] { 4d, 15, 20, 5, 5 }, typeof(CosineDistance), 0.40629)]
        [InlineData(new[] { 1d, 2, 3 }, new[] { 2d, 4, 6 }, typeof(EuclidianDistance), 3.7416573867739413855837487323165)]
        [InlineData(new[] { 1d, 0, 0, 1, 1 }, new[] { 0d, 0, 1, 0, 1 }, typeof(HammingDistance), 3)]
        [InlineData(new[] { 1d, 5, 3, 10 }, new[] { 4d, 15, 20, 5 }, typeof(ManhattanDistance), 35)]
        public void Distance_Test(double[] x, double[] y, Type t, double truth)
        {
            Assert.True(t.GetInterfaces().Contains(typeof(IDistance)));

            var distance = (IDistance)Activator.CreateInstance(t);
            var result = distance.Compute(new Vector(x), new Vector(y));

            truth = System.Math.Round(truth, 4);
            result = System.Math.Round(result, 4);

            Assert.Equal(truth, result);
        }

        [Theory]
        [InlineData(new[] { 1d, 5, 2, 3, 10 }, new[] { 4d, 15, 10, 5, 5 }, typeof(EuclidianSimilarity), 0.06573467)]
        [InlineData(new[] { 1d, 2, 3 }, new[] { 2d, 4, 6 }, typeof(CosineSimilarity), 1)]
        [InlineData(new[] { 1d, 2, 6 }, new[] { 3d, 7, 20 }, typeof(PearsonCorrelation), 0.999321)]
        [InlineData(new[] { 1d, 5, 3 }, new[] { 4d, 7, 20 }, typeof(TanimotoCoefficient), 0.2468827)]
        public void Similarity_Test(double[] x, double[] y, Type t, double truth)
        {
            Assert.True(t.GetInterfaces().Contains(typeof(ISimilarity)));

            var similarity = (ISimilarity)Activator.CreateInstance(t);
            var result = similarity.Compute(new Vector(x), new Vector(y));

            truth = System.Math.Round(truth, 4);
            result = System.Math.Round(result, 4);

            Assert.Equal(truth, result);
        }

    }
}
