using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Utils;
using numl.Math;
using numl.Math.LinearAlgebra;

namespace numl.Tests.DataTests
{
    [TestFixture, Category("Data")]
    public class LinAlgConversionTests
    {
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

            Matrix v = x.ToMatrix();

            Matrix truth =
                    new double[,] { 
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    };

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

            Matrix v = x.ToMatrix();


            Matrix truth =
                    new double[,] { 
                        { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                        { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                        { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                        { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                        { 0, 2, 0, 0, 5, 6, 0, 8, 0, 0 },
                    };

            Assert.AreEqual(truth, v);
        }

        [Test]
        public void Test_Jagged_Matrix_Conversion()
        {
            IEnumerable<IEnumerable<double>> x =
                new double[][] 
                { 
                    new double[] { 1, 2, 3, 4, 5, 6, 7 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    new double[] { 1, 2, 3, 4, 5, 6 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8},
                    new double[] { 1 },
                };

            Matrix v = x.ToMatrix();

            Matrix truth =
                    new double[,] { 
                        { 1, 2, 3, 4, 5, 6, 7, 0, 0, 0 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                        { 1, 2, 3, 4, 5, 6, 0, 0, 0, 0 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 0, 0 },
                        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    };

            Assert.AreEqual(truth, v);
        }

        [Test]
        public void Test_Example_Conversion()
        {
            IEnumerable<IEnumerable<double>> x =
                new double[][] 
                { 
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, -1 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, -1 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 },
                };

            var tuple = x.ToExamples();

            Matrix m =
                    new double[,] { 
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                    };

            Vector v = new double[] { -1, 1, -1, 1, 1 };

            Assert.AreEqual(m, tuple.Item1);
            Assert.AreEqual(v, tuple.Item2);
        }

        [Test]
        public void Test_Jagged_Example_Conversion()
        {
            IEnumerable<IEnumerable<double>> x =
                new double[][] 
                { 
                    new double[] { 1, 2, 3, 4, 5, 6, 7 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    new double[] { 1, 2, 3, 4, 5, 6 },
                    new double[] { 1, 2, 3, 4, 5, 6, 7, 8 },
                    new double[] { 1 },
                };

            var tuple = x.ToExamples();


            Matrix m =
                    new double[,] { 
                        { 1, 2, 3, 4, 5, 6, 7, 0, 0 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                        { 1, 2, 3, 4, 5, 6, 0, 0, 0 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 0 },
                        { 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                    };

            Vector v = new double[] { 0, 10, 0, 0, 0 };


            Assert.AreEqual(m, tuple.Item1);
            Assert.AreEqual(v, tuple.Item2);
        }
    }
}
