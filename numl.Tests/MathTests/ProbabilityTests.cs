using numl.Math.LinearAlgebra;
using numl.Math.Probability;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.MathTests
{
    [TestFixture, Category("Math")]
    public class ProbabilityTests
    {
        [TestCase(3, 10000)]
        [TestCase(3, 100000)]
        [TestCase(4, 10000)]
        [TestCase(4, 100000)]
        [TestCase(5, 10000)]
        [TestCase(5, 100000)]
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
                Assert.AreEqual(diff(means[i], dstrb.Mu[i]), 0, 0.1);
                // test covariance (should be 0, but with 10% tolerance)
                Assert.AreEqual(diff(means[i], cov[i]), 0, 0.1);
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
