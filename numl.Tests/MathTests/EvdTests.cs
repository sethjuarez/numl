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
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(30)]
        //[TestCase(50)]
        //[TestCase(100)]
        public void CanFactorizeRandomSymmetricMatrix(int order)
        {
            var rand = Matrix.Rand(order, order, 1);
            var A = rand.T * rand;
            var evd = new Evd(A);
            evd.compute();
            var eigenvectors = evd.Eigenvectors;
            var eigenvalues = evd.Eigenvalues;


            Assert.AreEqual(order, eigenvectors.Rows);
            Assert.AreEqual(order, eigenvectors.Cols);
            Assert.AreEqual(order, eigenvalues.Length);

            // make sure that A = V*λ*VT 
            var computed = eigenvectors * eigenvalues.Diag() * eigenvectors.T;

            for (var i = 0; i < computed.Rows; i++)
                for (var j = 0; j < computed.Cols; j++)
                    Assert.AreEqual(A[i, j], computed[i, j], 1.0e-10);
        }

    }
}
