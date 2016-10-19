using System;
using System.Linq;
using Xunit;
using numl.Unsupervised;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Tests.UnsupervisedTests
{
    [Trait("Category", "Unsupervised")]
    public class PCATests
    {
        [Fact]
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

        [Fact]
        public void Test_Magic_3_PCA()
        {
            Matrix X = new[,]
            {
                { 8, 1, 6 },
                { 3, 5, 7 },
                { 4, 9, 2 }
            };

            Matrix Y = new[,]
            {
               { -4.8990e+000,  -1.4142e+000 },
               {  4.4409e-016,  2.8284e+000 },
               {  4.8990e+000,  -1.4142e+000 }
            };

            var pca = new PCA();

            pca.Generate(X);

            var Yt = pca.Reduce(2).T;

            for (int i = 0; i < Y.Rows; i++)
                for (int j = 0; j < Y.Cols; j++)
                    Assert.Equal(Y[i, j], Yt[i, j], 3);
        }
    }
}
