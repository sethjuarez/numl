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
    public class PCATests
    {
        [Test]
        public void Test_Numerical_PCA()
        {
            Matrix m = new[,]
                {{ 2.5, 2.4 },
                 { 0.5, 0.7 },
                 { 2.2, 2.9 },
                 { 1.9, 2.2 },
                 { 3.1, 3.0 },
                 { 2.3, 2.7 },
                 { 2.0, 1.6 },
                 { 1.0, 1.1 },
                 { 1.5, 1.6 },
                 { 1.1, 0.9 }};

            PCA pca = new PCA();
            pca.Generate(m);
        }
    }
}
