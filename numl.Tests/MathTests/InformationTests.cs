using numl.Math.Information;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.MathTests
{
    [TestFixture]
    public class InformationTests
    {
        [TestCase(new[] { 6d, 3, 3, 2, 1 }, typeof(Entropy), 2.1055872616983)]
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
