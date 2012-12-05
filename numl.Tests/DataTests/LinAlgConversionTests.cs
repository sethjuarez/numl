/*
 Copyright (c) 2012 Seth Juarez

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
*/

using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;
using numl.Utils;
using numl.Math;

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

            Matrix v = MathConversion.ToMatrix(x);

            Assert.IsInstanceOf(typeof(DenseMatrix), v);

            Matrix truth =
                new DenseMatrix(
                    new double[,] { 
                        { 1, 2, 3, 4, 5, 6, 7, 0, 0, 0 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                        { 1, 2, 3, 4, 5, 6, 0, 0, 0, 0 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 0, 0 },
                        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    }
                );

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

            var tuple = MathConversion.ToExamples(x);

            Matrix m =
                new DenseMatrix(
                    new double[,] { 
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                    }
                );

            Vector v =
                new DenseVector(new double[] { -1, 1, -1, 1, 1 });

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

            var tuple = MathConversion.ToExamples(x);


            Matrix m =
                new DenseMatrix(
                    new double[,] { 
                        { 1, 2, 3, 4, 5, 6, 7, 0, 0 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                        { 1, 2, 3, 4, 5, 6, 0, 0, 0 },
                        { 1, 2, 3, 4, 5, 6, 7, 8, 0 },
                        { 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                    }
                );

            Vector v =
                new DenseVector(new double[] { 0, 10, 0, 0, 0 });


            Assert.AreEqual(m, tuple.Item1);
            Assert.AreEqual(v, tuple.Item2);
        }
    }
}
