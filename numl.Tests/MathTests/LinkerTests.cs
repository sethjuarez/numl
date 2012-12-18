using numl.Math.Linkers;
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
    public class LinkerTests
    {
        [Test]
        public void Average_Linker_Test()
        {
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
