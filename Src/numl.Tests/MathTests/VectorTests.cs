using System;
using System.IO;
using System.Linq;
using Xunit;
using numl.Math.LinearAlgebra;
using numl.Utils;
using System.Collections.Generic;
using numl.Math.Probability;
using numl.Tests.SerializationTests;

namespace numl.Tests.MathTests
{

    [Trait("Category", "Math")]
    public class VectorTests
    {
        [Fact]
        public void Vector_Serialize_Test()
        {
            string path = Path.Combine(BaseSerialization.GetPath(GetType()), "vector_serialize_test.json");

            // want to test "ugly" members in the vector
            Vector v1 = new[] { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 };

            // serialize
            // ensure we delete the file first 
            // or we may have extra data
            if (File.Exists(path)) File.Delete(path);
            v1.Save(path);

            // deserialize
            Vector v2 = Vector.Load(path);
            Assert.Equal(v1, v2);
        }

        [Fact]
        public void Vector_Random_Test()
        {
            Vector v1 = (Vector.Rand(5) * 10).Round();
        }

        [Fact]
        public void Vector_Equals_Test()
        {
            Vector v1 = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Vector v2 = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Assert.Equal(true, v1.Equals(v2));
            Assert.Equal(true, v1 == v2);
        }

        [Fact]
        public void Vector_Not_Equals_Test()
        {
            Vector v1 = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Vector v2 = new[] { 1, 2, 3, 4, 45, 6, 7, 8, 9 };
            Assert.Equal(false, v1.Equals(v2));
            Assert.Equal(false, v1 == v2);
            Assert.Equal(true, v1 != v2);
        }

        [Fact]
        public void Get_Column_Vector_From_Matrix()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Vector v1 = one[1, VectorType.Col];
            Vector v2 = new[] { 2, 5, 8 };
            Assert.True(v2 == v1);
        }

        [Fact]
        public void Get_Row_Vector_From_Matrix()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Vector v1 = one[1];
            Vector v2 = one[0, VectorType.Row];

            Vector v1Truth = new[] { 4, 5, 6 };
            Vector v2Truth = new[] { 1, 2, 3 };

            Assert.True(v1Truth == v1);
            Assert.True(v2Truth == v2);
        }

        [Fact]
        public void Assign_Column_Vector_To_Matrix()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Vector toAssign = new[] { 1, 1, 1 };
            one[1, VectorType.Col] = toAssign;

            Assert.True(toAssign == one[1, VectorType.Col]);
        }

        [Fact]
        public void Assign_Row_Vector_To_Matrix()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Vector toAssign = new[] { 1, 1, 1 };
            one[1, VectorType.Row] = toAssign;
            one[0] = toAssign;

            Assert.Equal(toAssign, one[1, VectorType.Row]);
            Assert.Equal(toAssign, one[0]);
        }

        [Fact]
        public void Matrix_Dot_Vector()
        {
            Matrix x = new[,]
                {{1, 2, 3, 4},
                 {4, 5, 6, 7},
                 {7, 8, 9, 10}};

            Vector v = new[] { 1, 2, 3, 4 };

            Vector sol = new[] { 30, 60, 90 };
            Vector ans = Matrix.Dot(x, v);
            //Matrix m = x * v;
            Assert.Equal(sol, ans);
        }

        [Fact]
        public void Vector_Dot_Matrix()
        {
            Matrix x = new[,]
                {{1, 2, 3, 4},
                 {4, 5, 6, 7},
                 {7, 8, 9, 10}};

            Vector v = new[] { 1, 2, 3 };

            Vector sol = new[] { 30, 36, 42, 48 };
            Vector ans = Matrix.Dot(v, x);
            //Matrix m = v * x;
            Assert.Equal(sol, ans);
        }

        [Fact]
        public void Vector_Combine()
        {
            Vector v1 = new[] { 1, 2, 3 };
            Vector v2 = new[] { 4, 5, 6 };
            Vector v3 = new[] { 7, 8, 9 };

            Vector sl = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Vector an = Vector.Combine(v1, v2, v3);

            Assert.Equal(sl, an);
        }

        [Fact]
        public void Vector_Expand_By_N()
        {
            Vector v1 = new[] { 1, 2, 3 };


            Vector sl = new[] { 1, 2, 3, 0, 0, 0 };
            Vector an = v1.Expand(3);

            Assert.Equal(sl, an);
        }

        [Fact]
        public void Vector_Expand_By_Vector()
        {
            Vector v1 = new[] { 1, 2, 3 };
            Vector v2 = new[] { 4, 5, 6 };
            Vector v3 = new[] { 7, 8, 9 };


            Vector sl = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Vector an = v1
                        .Expand(v2)
                        .Expand(v3);

            Assert.Equal(sl, an);
        }

        [Fact]
        public void Vector_Top()
        {
            Vector sl = new[] { 11, 23, 12, 56, 34, 76, 89, 23, 45, 34, 22, 12, 34, 54, 66 };
            var ans = new[] { 5, 6, 14 };
            var cmp = sl.Top(3).ToArray();

            Assert.Equal(ans.Length, cmp.Length);
            for (int i = 0; i < ans.Length; i++)
                Assert.Equal(ans[i], cmp[i]);
        }

        [Fact]
        public void Vector_Reshape_To_Matrix_1()
        {
            Vector v = new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Matrix ans = new double[,] 
            {
                { 1, 2, 3, 4, 5 },
                { 6, 7, 8, 9, 10 }
            };

            Matrix m = v.Reshape(5, VectorType.Col, VectorType.Col);

            Assert.Equal(v[0], ans[0, 0]);
            Assert.Equal(v[4], ans[0, 4]);
            Assert.Equal(v[5], ans[1, 0]);
            Assert.Equal(v[9], ans[1, 4]);
        }

        [Fact]
        public void Vector_Reshape_To_Matrix_2()
        {
            Vector v = new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Matrix ans = new double[,]
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
                { 7, 8 },
                { 9, 10 }
            };

            Matrix m = v.Reshape(2, VectorType.Col, VectorType.Col);

            Assert.Equal(v[0], ans[0, 0]);
            Assert.Equal(v[4], ans[2, 0]);
            Assert.Equal(v[5], ans[2, 1]);
            Assert.Equal(v[9], ans[4, 1]);
        }

        [Fact]
        public void Vector_Reshape_To_Matrix_3()
        {
            Vector v = new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Matrix ans = new double[,]
            {
                { 1, 6 },
                { 2, 7 },
                { 3, 8 },
                { 4, 9 },
                { 5, 10 }
            };

            Matrix m = v.Reshape(2, VectorType.Col, VectorType.Row);

            Assert.Equal(v[0], ans[0, 0]);
            Assert.Equal(v[4], ans[4, 0]);
            Assert.Equal(v[5], ans[0, 1]);
            Assert.Equal(v[9], ans[4, 1]);
        }

        [Fact]
        public void Vector_GetRandom_Element_Test()
        {
            Vector v = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Vector h = new Vector(v.Length);
            for (int i = 0; i < 10000; i++)
                h[(int)v.GetRandom()] += 1;

            h = h / 1000;

            for (int i = 0; i < h.Length; i++)
                Almost.Equal(1d, h[i], .1);
        }

        [Fact]
        public void Vector_And_Scalar_Subtraction_Test()
        {
            Vector v = new Vector(new double[] { 1, 2, 3 });
            double c = 2;
            Vector expectedDifference = new Vector(new double[] { -1, 0, 1 });
            Vector difference = v - c;

            Assert.Equal(difference, expectedDifference);
        }

        [Fact]
        public void Vector_And_Scalar_Swapped_Subtraction_Test()
        {
            Vector v = new Vector(new double[] { 1, 2, 3 });
            double c = 2;
            Vector difference = v - c;
            Vector swappedDifference = c - v;
            Assert.Equal(difference, -swappedDifference);
        }
    }
}

