using numl.Math.LinearAlgebra;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.MathTests
{
    [TestFixture]
    public class EvdTests
    {

        [Test]
        public void Simple_3_by_3()
        {
            var s = 1 / System.Math.Sqrt(2);
            var d = 1 / 3d;
            Matrix A = new[,] {
                {  d,  -d,  0 },
                { -d, d+1, -s },
                {  0,  -s,  1 }
            };


        }

    }
}
