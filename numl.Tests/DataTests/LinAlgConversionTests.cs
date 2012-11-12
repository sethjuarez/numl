using System;
using numl.Math;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace numl.Tests.DataTests
{
    [TestFixture]
    public class LinAlgConversionTests
    {
        [Test]
        public void Test_Dense_Vector_Conversion()
        {
            IEnumerable<double> x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Vector v = MathConversion.ToVector(x);

            Assert.IsInstanceOf(typeof(DenseVector), v);
            Vector truth = 
                new DenseVector(new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            Assert.AreEqual(truth, x);
        }

        [Test]
        public void Test_Sparse_Vector_Conversion()
        {
            IEnumerable<double> x = new double[] { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 };
            Vector v = MathConversion.ToVector(x);

            Assert.IsInstanceOf(typeof(SparseVector), v);
            Vector truth =
                new DenseVector(new double[] { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 });

            Assert.AreEqual(truth, x);
        }

        [Test]
        public void Test_Dense_Matrix_Conversion()
        {
            IEnumerable<IEnumerable<double>> x =
                new double[][] 
                { 
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                };

            Matrix v = MathConversion.ToMatrix(x);

            Assert.IsInstanceOf(typeof(DenseMatrix), v);

            Matrix truth =
                new DenseMatrix(
                    new double[,] { 
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    }
                );

            Assert.AreEqual(truth, v);
        }

        [Test]
        public void Test_Sparse_Matrix_Conversion()
        {
            IEnumerable<IEnumerable<double>> x =
                new double[][] 
                { 
                    new double[] { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                    new double[] { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                    new double[] { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                    new double[] { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                    new double[] { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                };

            Matrix v = MathConversion.ToMatrix(x);

            Assert.IsInstanceOf(typeof(SparseMatrix), v);

            Matrix truth =
                new SparseMatrix(
                    new double[,] { 
                        { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                        { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                        { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                        { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                        { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                    }
                );

            Assert.AreEqual(truth, v);
        }
    }
}
