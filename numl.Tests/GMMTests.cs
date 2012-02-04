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
        private Matrix Rotation(double degrees)
        {
            var radians = degrees * (System.Math.PI / 180);
            var rot = Matrix.Zeros(2, 2);
            rot[0, 0] = System.Math.Cos(radians);
            rot[0, 1] = -System.Math.Sin(radians);
            rot[1, 0] = System.Math.Sin(radians);
            rot[1, 1] = System.Math.Cos(radians);

            return rot;
        }

        [Test]
        public void Test_Raw_GMM()
        {
            int n = 5;
            // two normally distributed 
            // clusters, should be good
            var g1 = Matrix.VStack(
                        Vector.NormRand(n, 8, 3),
                        Vector.NormRand(n, 1, 6)) * Rotation(15);

            var g2 = Matrix.VStack(
                        Vector.NormRand(n, -1, 7.5),
                        Vector.NormRand(n, .5, 2)) * Rotation(335);

            Matrix X = g1.Stack(g2);

            GMM gmm = new GMM();
            gmm.Generate(X, 2);
        }

        
    }
}
