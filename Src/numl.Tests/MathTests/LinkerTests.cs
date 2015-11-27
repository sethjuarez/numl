using System;
using System.Linq;
using NUnit.Framework;
using numl.Math.Linkers;
using numl.Math.Metrics;
using System.Collections.Generic;

namespace numl.Tests.MathTests
{
    [TestFixture, Category("Math")]
    public class LinkerTests
    {
        [Test]
        public void Average_Linker_Test()
        {
            // TODO: Finish linker tests
            var a = new double[] { 1.0, 1.0 };
            var b = new double[] { 1.5, 1.5 };
            var c = new double[] { 5.0, 5.0 };
            var d = new double[] { 3.0, 4.0 };
            var e = new double[] { 4.0, 4.0 };
            var f = new double[] { 3.0, 3.5 };

            AverageLinker linker = new AverageLinker(new EuclidianDistance());

        }
    }
}
