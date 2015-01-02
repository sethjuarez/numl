using System;
using System.Linq;
using NUnit.Framework;
using numl.Math.Information;
using System.Collections.Generic;

namespace numl.Tests.MathTests
{
    [TestFixture, Category("Math")]
    public class InformationTests
    {
        [TestCase(new[] { 3d, 3, 2, 1, 1, 0, 3, 3, 3, 4 }, typeof(Entropy), 1.96096)]
        [TestCase(new[] { 2d, 3, 3, 2, 4, 7, 6, 3, 2, 7 }, typeof(Entropy), 2.17095)]
        [TestCase(new[] { 3d, 3, 2, 1, 1, 0, 3, 3, 3, 4 }, typeof(Gini), 0.68)]
        [TestCase(new[] { 2d, 3, 3, 2, 4, 7, 6, 3, 2, 7 }, typeof(Gini), 0.76)]
        [TestCase(new[] { 3d, 3, 2, 1, 1, 0, 3, 3, 3, 4 }, typeof(Error), 0.5)]
        [TestCase(new[] { 2d, 3, 3, 2, 4, 7, 6, 3, 2, 7 }, typeof(Error), 0.7)]
        public void Impurity_Calculation(double[] x, Type t, double truth)
        {
            Assert.AreEqual(typeof(Impurity), t.BaseType);

            var impurity = (Impurity)Activator.CreateInstance(t);
            var result = impurity.Calculate(x);

            truth = System.Math.Round(truth, 4);
            result = System.Math.Round(result, 4);

            Assert.AreEqual(truth, result);
        }
    }
}
