using System;
using System.Linq;
using NUnit.Framework;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Tests.MathTests
{
    [TestFixture, Category("Math")]
    public class EvdTests
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(30)]
        [TestCase(50)]
        [TestCase(100)]
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
