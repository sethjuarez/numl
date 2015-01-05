using System;
using System.IO;
using NUnit.Framework;
using numl.Math.LinearAlgebra;
using System.Xml.Serialization;

namespace numl.Tests.MathTests
{
    /// <summary>
    /// Summary description for MatrixTests
    /// </summary>
    [TestFixture, Category("Math")]
    public class MatrixTests
    {
        private Matrix _test = new[,]
            {{ 1, 2, 3},
             { 4, 5, 6},
             { 7, 8, 9}};

        [Test]
        public void Test_Matrix_Vector_Enumeration_Row()
        {
            Vector[] a = new Vector[]
            {
                new[] { 1, 2, 3 },
                new[] { 4, 5, 6 },
                new[] { 7, 8, 9 },
            };

            for (int i = 0; i < 3; i++)
                Assert.IsTrue(a[i] == _test[i, VectorType.Row]);
        }

        [Test]
        public void Test_Matrix_Vector_Enumeration_Col()
        {
            Vector[] a = new Vector[]
            {
                new[] { 1, 4, 7 },
                new[] { 2, 5, 8 },
                new[] { 3, 6, 9 },
            };

            for (int i = 0; i < 3; i++)
                Assert.IsTrue(a[i] == _test[i, VectorType.Col]);
        }

        [Test]
        public void Test_Matrix_Vector_Enumeration_Row_Transpose()
        {
            Vector[] a = new Vector[]
            {
                new[] { 1, 4, 7 },
                new[] { 2, 5, 8 },
                new[] { 3, 6, 9 },
            };

            for (int i = 0; i < 3; i++)
                Assert.IsTrue(a[i] == _test.T[i, VectorType.Row]);
        }

        [Test]
        public void Test_Matrix_Vector_Enumeration_Col_Transpose()
        {
            Vector[] a = new Vector[]
            {
                new[] { 1, 2, 3 },
                new[] { 4, 5, 6 },
                new[] { 7, 8, 9 },
            };

            for (int i = 0; i < 3; i++)
                Assert.AreEqual(a[i], _test.T[i, VectorType.Col]);
        }

        [Test]
        public void Matrix_Serialize_Test()
        {
            string path = Directory.GetCurrentDirectory() + @"\matrix_serialize_test.xml";

            Matrix m1 = new[,] {
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 }
            };

            XmlSerializer serializer = new XmlSerializer(typeof(Matrix));

            // serialize
            // ensure we delete the file first or we may have extra data
            if (File.Exists(path))
            {
                // this could get an access violation but either way 
                // we don't want a pointer stuck in the app domain
                File.Delete(path);
            }

            using (var stream = File.OpenWrite(path))
            {
                serializer.Serialize(stream, m1);
            }

            // deserialize
            Matrix m2 = null;
            using (var stream = File.OpenRead(path))
            {
                m2 = (Matrix)serializer.Deserialize(stream);
            }

            Assert.AreEqual(m1, m2);
        }

        [Test]
        public void Matrix_Equal_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix two = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Assert.AreEqual(true, one.Equals(two));
            Assert.AreEqual(true, one == two);
        }

        [Test]
        public void Matrix_Not_Equal_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 10}};

            Matrix two = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Assert.AreEqual(false, one.Equals(two));
            Assert.AreEqual(false, one == two);
            Assert.AreEqual(true, one != two);
        }

        [Test]
        public void Matrix_Identity_Test()
        {
            Matrix eye1 = new[,]
                {{1, 0, 0},
                 {0, 1, 0},
                 {0, 0, 1}};

            Matrix eye2 = new[,]
                {{1, 0, 0, 0},
                 {0, 1, 0, 0},
                 {0, 0, 1, 0}};

            Matrix eye3 = new[,]
                {{1, 0, 0},
                 {0, 1, 0},
                 {0, 0, 1},
                 {0, 0, 0}};

            Assert.AreEqual(eye1, Matrix.Identity(3));
            Assert.AreEqual(eye2, Matrix.Identity(3, 4));
            Assert.AreEqual(eye3, Matrix.Identity(4, 3));
        }

        [Test]
        public void Matrix_Trace_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 10}};

            Matrix two = new[,]
                {{1, 2, 3, 9},
                 {4, 5, 6, 12},
                 {7, 8, 10, 13}};

            Matrix three = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 10},
                 {11,12,13}};

            Assert.AreEqual(16, one.Trace());
            Assert.AreEqual(16, two.Trace());
            Assert.AreEqual(16, three.Trace());
        }

        [Test]
        public void Matrix_Zeros_Test()
        {
            Matrix one = new[,]
                {{0, 0, 0},
                 {0, 0, 0},
                 {0, 0, 0}};

            Matrix two = new[,]
                {{0, 0, 0, 0},
                 {0, 0, 0, 0},
                 {0, 0, 0, 0}};

            Matrix three = new[,]
                {{0, 0, 0},
                 {0, 0, 0},
                 {0, 0, 0},
                 {0, 0, 0}};

            Assert.AreEqual(one, Matrix.Zeros(3));
            Assert.AreEqual(two, Matrix.Zeros(3, 4));
            Assert.AreEqual(three, Matrix.Zeros(4, 3));
        }

        [Test]
        public void Matrix_Transpose_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix oneT = new[,]
                {{1, 4, 7},
                 {2, 5, 8},
                 {3, 6, 9}};

            Matrix two = new[,]
                {{1, 2, 3, 9},
                 {4, 5, 6, 12},
                 {7, 8, 10, 13}};

            Matrix twoT = new[,]
                {{1, 4, 7},
                 {2, 5, 8},
                 {3, 6, 10},
                 {9,12, 13}};

            Assert.AreEqual(oneT, one.Transpose());
            Assert.AreEqual(oneT, one.T);
            Assert.AreEqual(one, oneT.Transpose());
            Assert.AreEqual(one, oneT.T);
            Assert.AreEqual(twoT, two.Transpose());
            Assert.AreEqual(twoT, two.T);
            Assert.AreEqual(two, twoT.Transpose());
            Assert.AreEqual(two, twoT.T);
        }

        [Test]
        public void Matrix_Assign_Value_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            one[1, 1] = 14.5;
            Assert.AreEqual(14.5, one[1, 1]);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Matrix_Assign_Value_Bad_Index_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            one[5, 5] = 14.5;
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Matrix_Read_Value_Bad_Index_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            var d = one[5, 5];
        }

        [Test]
        public void Matrix_Add_Aligned_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix two = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix sum = new[,]
                {{2, 4, 6},
                 {8, 10, 12},
                 {14, 16, 18}};

            Matrix a = new[,]
                {{1, 2, 3, 1},
                 {4, 5, 6, 2},
                 {7, 8, 9, 3}};

            Matrix b = new[,]
                {{1, 2, 3, 5},
                 {4, 5, 6, 6},
                 {7, 8, 9, 7}};

            Matrix c = new[,]
                {{2, 4, 6, 6},
                 {8, 10, 12, 8},
                 {14, 16, 18, 10}};

            Matrix d = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9},
                 {1, 2, 3}};

            Matrix e = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9},
                 {3, 2, 1}};

            Matrix f = new[,]
                {{2, 4, 6},
                 {8, 10, 12},
                 {14, 16, 18},
                 {4, 4, 4}};

            Assert.AreEqual(sum, one + two);
            Assert.AreEqual(c, a + b);
            Assert.AreEqual(f, d + e);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Matrix_Add_Non_Aligned_Test_1()
        {
            Matrix a = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix b = new[,]
                {{2, 4, 6, 6},
                 {8, 10, 12, 8},
                 {14, 16, 18, 10}};

            var x = a + b;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Matrix_Add_Non_Aligned_Test_2()
        {
            Matrix a = new[,]
                {{2, 4, 6},
                 {8, 10, 12},
                 {14, 16, 18},
                 {4, 4, 4}};

            Matrix b = new[,]
                {{2, 4, 6, 6},
                 {8, 10, 12, 8},
                 {14, 16, 18, 10}};

            var x = a + b;
        }

        [Test]
        public void Matrix_Subtract_Aligned_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix two = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix sum = new[,]
                {{0, 0, 0},
                 {0, 0, 0},
                 {0, 0, 0}};

            Matrix a = new[,]
                {{1, 2, 3, 1},
                 {4, 5, 6, 2},
                 {7, 8, 9, 3}};

            Matrix b = new[,]
                {{1, 2, 3, 5},
                 {4, 5, 6, 6},
                 {7, 8, 9, 7}};

            Matrix c = new[,]
                {{0, 0, 0, -4},
                 {0, 0, 0, -4},
                 {0, 0, 0, -4}};

            Matrix d = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9},
                 {1, 2, 3}};

            Matrix e = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9},
                 {3, 2, 1}};

            Matrix f = new[,]
                {{0, 0, 0},
                 {0, 0, 0},
                 {0, 0, 0},
                 {-2, 0, 2}};

            Assert.AreEqual(sum, one - two);
            Assert.AreEqual(c, a - b);
            Assert.AreEqual(f, d - e);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Matrix_Subtract_Non_Aligned_Test_1()
        {
            Matrix a = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix b = new[,]
                {{2, 4, 6, 6},
                 {8, 10, 12, 8},
                 {14, 16, 18, 10}};

            var x = a - b;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Matrix_Subtract_Non_Aligned_Test_2()
        {
            Matrix a = new[,]
                {{2, 4, 6},
                 {8, 10, 12},
                 {14, 16, 18},
                 {4, 4, 4}};

            Matrix b = new[,]
                {{2, 4, 6, 6},
                 {8, 10, 12, 8},
                 {14, 16, 18, 10}};

            var x = a - b;
        }

        [Test]
        public void Matrix_Multiply_Vector_Aligned_Test()
        {
            Matrix one = new[,]
                {{ 2,  4,  6},
                 { 8, 10, 12},
                 {14, 16, 18}};
            Vector v = new[] { 0.5, 0.5, 0.5 };

            Matrix s1 = (new Vector(new double[] { 6, 15, 24 }))
                            .ToMatrix(VectorType.Col);
            Matrix s2 = (new Vector(new double[] { 12, 15, 18 }))
                            .ToMatrix(VectorType.Row);

            Assert.AreEqual(s1, one * v);
            Assert.AreEqual(s2, v * one);
        }

        [Test]
        public void Matrix_Sum_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3, 4},
                 {4, 5, 6, 5},
                 {7, 8, 9, 6}};

            Vector row = new[] { 12, 15, 18, 15 };
            Vector col = new[] { 10, 20, 30 };

            Assert.AreEqual(row, one.Sum(VectorType.Row));
            Assert.AreEqual(col, one.Sum(VectorType.Col));
            Assert.AreEqual(60, one.Sum());
        }

        [Test]
        public void Matrix_Multiply_Aligned_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix sol = new[,]
                {{30, 36, 42},
                 {66, 81, 96},
                 {102, 126, 150}};

            Matrix a = new[,]
                {{2, 4, 6},
                 {8, 10, 12},
                 {14, 16, 18},
                 {4, 4, 4}};

            Matrix b = new[,]
                {{2, 4, 6, 6},
                 {8, 10, 12, 8},
                 {14, 16, 18, 10}};

            Matrix c = new[,]
                {{120, 144, 168, 104},
                 {264, 324, 384, 248},
                 {408, 504, 600, 392},
                 { 96, 120, 144,  96}};


            Assert.AreEqual(sol, one * one);
            Assert.AreEqual(c, a * b);
        }

        [Test]
        public void Matrix_Multiply_Scalar_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix sol = new[,]
                {{ 2,  4,  6},
                 { 8, 10, 12},
                 {14, 16, 18}};

            Assert.AreEqual(sol, 2 * one);
            Assert.AreEqual(sol, one * 2);
        }

        [Test]
        public void Matrix_Sub_Matrix_Test()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix sol1 = new[,]
                {{ 2, 3 },
                 { 5, 6 },
                 { 8, 9 }};

            Matrix sol2 = new[,]
                {{ 5, 6 },
                 { 8, 9 }};

            Assert.AreEqual(sol1, one.GetMatrix(1, 2, 0, 2));
            Assert.AreEqual(sol2, one.GetMatrix(1, 2, 1, 2));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Matrix_Multiply_Non_Aligned_Test()
        {
            Matrix a = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Matrix b = new[,]
                {{2, 4, 6},
                 {8, 10, 12},
                 {14, 16, 18},
                 {4, 4, 4}};

            var x = a * b;
        }

        [Test]
        public void Matrix_Inverse_Test()
        {
            Matrix a = new[,]
                {{4, 3},
                 {3, 2}};

            Matrix aInv = new[,]
                {{-2,  3},
                 { 3, -4}};

            Matrix b = new[,]
                {{ 1,  2,  3},
                 { 0,  4,  5},
                 { 1,  0,  6}};

            Matrix bInv = new Matrix(new double[,]
                {{ 12.0/11,  -6.0/11,  -1.0/11},
                 {  5.0/22,   3.0/22,  -5.0/22},
                 { -2.0/11,   1.0/11,   2.0/11}});

            Assert.AreEqual(aInv, a ^ -1);
            Assert.AreEqual(bInv.ToString(), (b ^ -1).ToString());
        }


        [Test]
        public void Matrix_Cholesky_Test()
        {
            Matrix m = new[,]
                {{ 25, 15, -5},
                 { 15, 18,  0},
                 { -5,  0, 11}};

            Matrix sol = new[,]
                {{  5, 0, 0},
                 {  3, 3, 0},
                 { -1, 1, 3}};

            Assert.AreEqual(sol, Matrix.Cholesky(m));
            Assert.AreEqual(sol, m.Cholesky());
        }

        [Test]
        public void Matrix_LLS_Test()
        {
            Matrix A = new[,]
                {{ 3, -6},
                 { 4, -8},
                 { 0,  1}};

            Vector b = new[] { -1, 7, 2 };
            Vector sol = new[] { 5, 2 };

            // LLS implementation
            Vector x = A / b;
            Assert.AreEqual(sol, x);

            // this should fire regular inverse
            // need to be a bit smarter about using straight
            // inverse here...
            // Cholesky if positive diagonal or
            // LU
            Vector y = (A.T * A) / (A.T * b).ToVector();
            Assert.AreEqual(sol, y);
        }

        [Test]
        public void Matrix_QR_Test()
        {
            Matrix A = new[,] {{ 4,  1,  2},
                               { 1,  4,  0},
                               { 2,  0,  4}};

            var t = Matrix.QR(A);
            var Q = t.Item1;
            var R = t.Item2;

            // close enough...
            var diff = A.Norm() - (Q * R).Norm();
            Assert.AreEqual(0, diff);
        }

        [Test]
        public void Matrix_Doolittle_Pivot_Test()
        {
            Matrix A = new[,] {{ 1, 4, 2, 3 },
                               { 1, 2, 1, 0 },
                               { 2, 6, 3, 1 },
                               { 0, 0, 1, 4 }};

            Matrix B = new[,] {{ 2,  3,  1,  2 },
                                {-1,  2,  7,  5 },
                                {-4, -3,  4,  2 },
                                { 3,  1,  6,  3 }};

            Matrix C = new[,] {{ 1, 4, 2, 3 },
                               { 1, 2, 1, 0 },
                               { 2, 6, 3, 1 },
                               { 0, 0, 1, 4 }};

            Matrix P = new[,] {{ 0, 0, 0, 1 },
                               { 0, 1, 0, 0 },
                               { 1, 0, 0, 0 },
                               { 0, 0, 1, 0 }};


            var I = Matrix.Pivot(B);
            //Assert.AreEqual(P, I);
        }

        [Test]
        public void Matrix_LU_Test()
        {
            Matrix A = new[,] {{ 1, 2, 0 },
                               { 3, 6, -1},
                               { 1, 2, 1 }};

            Matrix B = new[,] {{ 7,  3, -1,  2},
                               { 3,  8,  1, -4},
                               {-1,  1,  4, -1},
                               { 2, -4, -1,  6}};

            Matrix C = new[,] {{ 1, 4, 2, 3 },
                               { 1, 2, 1, 0 },
                               { 2, 6, 3, 1 },
                               { 0, 0, 1, 4 }};

            Matrix D = new[,] {{ 2,  3,  1,  2 },
                               {-1,  2,  7,  5 },
                               {-4, -3,  4,  2 },
                               { 3,  1,  6,  3 }};

            var t = Matrix.LU(D);
            var P = t.Item1;
            var L = t.Item2;
            var U = t.Item3;

            var T = Matrix.Stack(P, L).Stack(U);
        }

        [Test]
        public void Matrix_Extract_Test()
        {
            Matrix A = new[,]
                {{  1,  2,  4 },
                 {  1,  6,  2 },
                 {  3,  1,  1 }};


            Matrix sol = new[,]
                {{ 6, 2},
                 { 1, 1}};

            Assert.AreEqual(sol, A.Extract(1, 1, 2, 2));

            Matrix B = new[,]
                {{  1,  2,  4,  9 },
                 {  1,  6,  2,  7 },
                 {  3,  1,  1,  4 },
                 {  4,  2,  3,  2 }};

            Matrix bSol = new[,]
                {{  2,  7 },
                 {  1,  4 },
                 {  3,  2 }};

            Assert.AreEqual(bSol, B.Extract(2, 1, 2, 3));
        }

        [Test]
        public void Matrix_Covariance_Test()
        {
            // from: http://www.itl.nist.gov/div898/handbook/pmc/section5/pmc541.htm

            Matrix x = new[,]
               {{ 4.0, 2.0, .60 },
                { 4.2, 2.1, .59 },
                { 3.9, 2.0, .58 },
                { 4.3, 2.1, .62 },
                { 4.1, 2.2, .63 }};

            Matrix covTruth = new[,]
               {{ 0.02500, 0.00750, 0.00175 },
                { 0.00750, 0.00700, 0.00135 },
                { 0.00175, 0.00135, 0.00043 }};

            var cov = x.Covariance().Round(5);
            Assert.AreEqual(covTruth, cov);
        }

        [Test]
        public void Matrix_Determinant_Test()
        {

            Matrix x = new[,]
               {{ -2, 2, 3 },
                { -1, 1, 3 },
                { 2, 0, 1 }};

            Assert.AreEqual(6, x.Det());

            Matrix m = new[,] 
                {{ 1, 2,  2, 1 },
                 { 1, 2,  4, 2 },
                 { 2, 7,  5, 2 },
                 {-1, 4, -6, 3 }};

            // -42 + residual
            var det = m.Det();

            Matrix q = new[,] 
                 {{ 3,   2,  -1,   4, },
                  { 2,   1,   5,   7, },
                  { 0,   5,   2,  -6, },
                  { -1,   2,   1,   0, }};

            // -418
            var qd = q.Det();
        }

        [Test]
        public void Matrix_ToArray_Test()
        {
            Matrix m = new[,] 
                {{ 1d, 2d,  2d, 1d },
                 { 1d, 2d,  4d, 2d },
                 { 2d, 7d,  5d, 2d },
                 {-1d, 4d, -6d, 3d }};

            var n = m.ToArray();

            Assert.AreEqual(m, new Matrix(n));

            m[1, 2] = m[1, 2] + m[1, 2];

            Assert.AreNotEqual(m[1, 2], n[1][2]);
        }

        [Test]
        public void Matrix_Insertion_Row_Test()
        {
            Matrix m = new[,] 
                {{ 1d, 2d,  2d, 1d },
                 { 1d, 2d,  4d, 2d },
                 { 2d, 7d,  5d, 2d },
                 {-1d, 4d, -6d, 3d }};

            Matrix n = new[,] 
                {{ 1d, 2d,  2d, 1d },
                 { 1d, 2d,  4d, 2d },
                 { 1d, 1d, 1d, 1d },
                 { 2d, 7d,  5d, 2d },
                 {-1d, 4d, -6d, 3d },
                 { 1d, 1d, 1d, 1d }};

            m = m.Insert(Vector.Ones(4), 2, VectorType.Row);
            m = m.Insert(Vector.Ones(4), 5, VectorType.Row);

            Assert.AreEqual(m, n);
        }

        [Test]
        public void Matrix_Insertion_Col_Test()
        {
            Matrix m = new[,] 
                {{ 1d, 2d,  2d, 1d },
                 { 1d, 2d,  4d, 2d },
                 { 2d, 7d,  5d, 2d },
                 {-1d, 4d, -6d, 3d }};

            Matrix n = new[,] 
                {{ 0d, 1d, 2d, 0d, 2d, 1d, 1d },
                 { 0d, 1d, 2d, 0d, 4d, 2d, 1d },
                 { 0d, 2d, 7d, 0d, 5d, 2d, 1d },
                 { 0d,-1d, 4d, 0d,-6d, 3d, 1d }};

            m = m.Insert(Vector.Zeros(4), 0, VectorType.Col);
            m = m.Insert(Vector.Zeros(4), 3, VectorType.Col);
            m = m.Insert(Vector.Ones(4), m.Cols-1, VectorType.Col);

            Assert.AreEqual(n, m);
        }
    }
}
