using numl.Math.LinearAlgebra;
using numl.Math.Probability;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.MathTests
{
    [Trait("Category", "Math")]
    public class ProbabilityTests
    {
        [Theory]
        [InlineData(3, 10000)]
        [InlineData(3, 100000)]
        [InlineData(4, 10000)]
        [InlineData(4, 100000)]
        [InlineData(5, 10000)]
        [InlineData(5, 100000)]
        public void Test_Normal_Estimation(int d, int n)
        {
            // generate mu and sigma
            Vector means = Vector.Zeros(d);
            // assuming diagonal covariance matrix
            // for generation purposes equal to the
            // sqrt of the mean (easy to test)
            Vector sigma = Vector.Zeros(d);
            for (int i = 0; i < d; i++)
            {
                means[i] = Sampling.GetUniform() * 10;
                sigma[i] = sqrt(means[i]);
            }


            Matrix data = Matrix.Zeros(n, d);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < d; j++)
                    data[i, j] = Sampling.GetNormal(means[j], sigma[j]);

            NormalDistribution dstrb = new NormalDistribution();
            dstrb.Estimate(data);

            var cov = dstrb.Sigma.Diag();
            for (int i = 0; i < d; i++)
            {
                // test mean (should be 0, but with 10% tolerance)
                Almost.Equal(diff(means[i], dstrb.Mu[i]), 0, 0.1);
                // test covariance (should be 0, but with 10% tolerance)
                Almost.Equal(diff(means[i], cov[i]), 0, 0.1);
            }
        }

        [Fact]
        public void Test_Random_Bounds()
        {
            Random rnd = new Random();
            for (int x = 0; x < 10000; x++)
            {
                double min = rnd.Next(0, x);
                double max = rnd.Next((int)min + 1, x + 1);

                double d = Sampling.GetUniform(min, max);

                Assert.True(d >= min, $"[iter {x}]: {min} <-- {d} --> {max}");
                Assert.True(d <= max, $"[iter {x}]: {min} <-- {d} --> {max}");
            }
        }

        // used to calculate pct difference
        public static double diff(double x1, double x2)
        {
            return (x1 - x2) / x2;
        }

        public static double sqrt(double x)
        {
            return System.Math.Sqrt(x);
        }
    }
}
