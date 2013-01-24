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
        //[TestCase(1)]
        //[TestCase(2)]
        //[TestCase(3)]
        //[TestCase(5)]
        //[TestCase(10)]
        //[TestCase(20)]
        //[TestCase(30)]
        //[TestCase(50)]
        //[TestCase(53)]
        //[TestCase(100)]
        //[TestCase(200)]
        [TestCase(310)]
        //[TestCase(500)]
        //[TestCase(1000)]
        public void CanFactorizeRandomSymmetricMatrix(int order)
        {
            // create random matrix
            var rand = Matrix.Rand(order, 1.13);
            // create a symmetric positive definite matrix
            var A = rand.T * rand;
            var evd = new Evd(A);
            // compute eigenvalues/eigenvectors
            evd.compute();
            var eigenvectors = evd.Eigenvectors;
            var eigenvalues = evd.Eigenvalues;

            Assert.AreEqual(order, eigenvectors.Rows);
            Assert.AreEqual(order, eigenvectors.Cols);
            Assert.AreEqual(order, eigenvalues.Length);

            // make sure that A = V*λ*VT 
            var computed = eigenvectors * eigenvalues.Diag() * eigenvectors.T;
            Assert.IsTrue(A.Equals(computed, 1e-10));
        }
    }
}
