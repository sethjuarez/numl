using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using numl.Math;
using numl.Unsupervised;

namespace numl.Tests
{
    [TestFixture]
    public class GMMTests
    {
        [Test]
        public void Test_Raw_GMM()
        {
            int n = 5;
            // two normally distributed 
            // clusters, should be good
            var g1 = Matrix.VStack(
                        Vector.NormRand(n, 8, 3),
                        Vector.NormRand(n, 1, 6));

            var g2 = Matrix.VStack(
                        Vector.NormRand(n, -1, 7.5),
                        Vector.NormRand(n, .5, 2));

            Matrix X = g1.Stack(g2);

            GMM gmm = new GMM();
            gmm.Generate(X, 2);
        }

        
    }
}
