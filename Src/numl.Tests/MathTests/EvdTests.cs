using Xunit;
using numl.Math.LinearAlgebra;

namespace numl.Tests.MathTests
{
    [Trait("Category", "Math")]
    public class EvdTests
    {
        [Theory]
        [InlineData(1, false)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        [InlineData(3, true)]
        [InlineData(5, false)]
        [InlineData(5, true)]
        [InlineData(10, false)]
        [InlineData(10, true)]
        [InlineData(20, false)]
        [InlineData(20, true)]
        [InlineData(30, false)]
        [InlineData(30, true)]
        [InlineData(50, false)]
        [InlineData(50, true)]
        [InlineData(100, false)]
        [InlineData(100, true)]
        public void CanFactorizeRandomSymmetricMatrix(int order, bool diagonal)
        {
            // create random matrix
            var rand = Matrix.Rand(order, 1.13);
            // create a symmetric positive definite matrix
            var A = rand.T * rand;

            if (diagonal)
            {
                var diag = A[0, 0];
                for (var i = 1; i < order; i++)
                    A[i, i] = diag;
            }

            var evd = new Evd(A);
            // compute eigenvalues/eigenvectors
            evd.Compute();
            var eigenvectors = evd.Eigenvectors;
            var eigenvalues = evd.Eigenvalues;

            foreach (var e in eigenvalues)
                Assert.False(double.IsNaN(e));

            Assert.Equal(order, eigenvectors.Rows);
            Assert.Equal(order, eigenvectors.Cols);
            Assert.Equal(order, eigenvalues.Length);

            // make sure that A = V*λ*VT 
            var computed = eigenvectors * eigenvalues.Diag() * eigenvectors.T;
            Assert.True(A.Equals(computed, 1e-10));
        }
    }
}
