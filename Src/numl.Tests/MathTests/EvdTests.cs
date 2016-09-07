using System;
using System.Linq;
using Xunit;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Tests.MathTests
{
    [Trait("Category", "Math")]
    public class EvdTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        [InlineData(50)]
        [InlineData(100)]
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

            Assert.Equal(order, eigenvectors.Rows);
            Assert.Equal(order, eigenvectors.Cols);
            Assert.Equal(order, eigenvalues.Length);

            // make sure that A = V*λ*VT 
            var computed = eigenvectors * eigenvalues.Diag() * eigenvectors.T;
            Assert.True(A.Equals(computed, 1e-10));
        }
    }
}
