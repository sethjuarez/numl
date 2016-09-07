using System;
using System.Linq;
using Xunit;
using System.Reflection;
using numl.Math.Information;
using System.Collections.Generic;

namespace numl.Tests.MathTests
{
    [Trait("Category", "Math")]
    public class InformationTests
    {
        [Theory]
        [InlineData(new[] { 3d, 3, 2, 1, 1, 0, 3, 3, 3, 4 }, typeof(Entropy), 1.96096)]
        [InlineData(new[] { 2d, 3, 3, 2, 4, 7, 6, 3, 2, 7 }, typeof(Entropy), 2.17095)]
        [InlineData(new[] { 3d, 3, 2, 1, 1, 0, 3, 3, 3, 4 }, typeof(Gini), 0.68)]
        [InlineData(new[] { 2d, 3, 3, 2, 4, 7, 6, 3, 2, 7 }, typeof(Gini), 0.76)]
        [InlineData(new[] { 3d, 3, 2, 1, 1, 0, 3, 3, 3, 4 }, typeof(Error), 0.5)]
        [InlineData(new[] { 2d, 3, 3, 2, 4, 7, 6, 3, 2, 7 }, typeof(Error), 0.7)]
        public void Impurity_Calculation(double[] x, Type t, double truth)
        {
            Assert.Equal(typeof(Impurity), t.GetTypeInfo().BaseType);

            var impurity = (Impurity)Activator.CreateInstance(t);
            var result = impurity.Calculate(x);

            truth = System.Math.Round(truth, 4);
            result = System.Math.Round(result, 4);

            Assert.Equal(truth, result);
        }
    }
}
