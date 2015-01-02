using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using numl.Math.LinearAlgebra;
using System.Xml.Serialization;

namespace numl.Tests.MathTests
{
    /// <summary>
    /// Summary description for VectorTests
    /// </summary>
    [TestFixture, Category("Math")]
    public class VectorTests
    {
        [Test]
        public void Vector_Serialize_Test()
        {
            string path = Directory.GetCurrentDirectory() + @"\vector_serialize_test.xml";

            // want to test "ugly" members in the vector
            Vector v1 = new[] { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 };

            XmlSerializer serializer = new XmlSerializer(typeof(Vector));

            // serialize
            // ensure we delete the file first 
            // or we may have extra data
            if (File.Exists(path))
            {
                // this could get an access violation but either way we 
                // don't want a pointer stuck in the app domain
                File.Delete(path);
            }
            using (var stream = File.OpenWrite(path))
            {
                serializer.Serialize(stream, v1);
            }

            // deserialize
            Vector v2 = null;
            using (var stream = File.OpenRead(path))
            {
                v2 = (Vector)serializer.Deserialize(stream);
            }

            Assert.AreEqual(v1, v2);
        }

        [Test]
        public void Vector_Random_Test()
        {
            Vector v1 = (Vector.Rand(5) * 10).Round();
        }

        [Test]
        public void Vector_Equals_Test()
        {
            Vector v1 = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Vector v2 = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Assert.AreEqual(true, v1.Equals(v2));
            Assert.AreEqual(true, v1 == v2);
        }

        [Test]
        public void Vector_Not_Equals_Test()
        {
            Vector v1 = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Vector v2 = new[] { 1, 2, 3, 4, 45, 6, 7, 8, 9 };
            Assert.AreEqual(false, v1.Equals(v2));
            Assert.AreEqual(false, v1 == v2);
            Assert.AreEqual(true, v1 != v2);
        }

        [Test]
        public void Get_Column_Vector_From_Matrix()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Vector v1 = one[1, VectorType.Col];
            Vector v2 = new[] { 2, 5, 8 };
            Assert.IsTrue(v2 == v1);
        }

        [Test]
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

            Assert.IsTrue(v1Truth == v1);
            Assert.IsTrue(v2Truth == v2);
        }

        [Test]
        public void Assign_Column_Vector_To_Matrix()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Vector toAssign = new[] { 1, 1, 1 };
            one[1, VectorType.Col] = toAssign;

            Assert.IsTrue(toAssign == one[1, VectorType.Col]);
        }

        [Test]
        public void Assign_Row_Vector_To_Matrix()
        {
            Matrix one = new[,]
                {{1, 2, 3},
                 {4, 5, 6},
                 {7, 8, 9}};

            Vector toAssign = new[] { 1, 1, 1 };
            one[1, VectorType.Row] = toAssign;
            one[0] = toAssign;

            Assert.AreEqual(toAssign, one[1, VectorType.Row]);
            Assert.AreEqual(toAssign, one[0]);
        }

        [Test]
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
            Assert.AreEqual(sol, ans);
        }

        [Test]
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
            Assert.AreEqual(sol, ans);
        }

        [Test]
        public void Vector_Combine()
        {
            Vector v1 = new[] { 1, 2, 3 };
            Vector v2 = new[] { 4, 5, 6 };
            Vector v3 = new[] { 7, 8, 9 };

            Vector sl = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Vector an = Vector.Combine(v1, v2, v3);

            Assert.AreEqual(sl, an);
        }

        [Test]
        public void Vector_Expand_By_N()
        {
            Vector v1 = new[] { 1, 2, 3 };


            Vector sl = new[] { 1, 2, 3, 0, 0, 0 };
            Vector an = v1.Expand(3);

            Assert.AreEqual(sl, an);
        }

        [Test]
        public void Vector_Expand_By_Vector()
        {
            Vector v1 = new[] { 1, 2, 3 };
            Vector v2 = new[] { 4, 5, 6 };
            Vector v3 = new[] { 7, 8, 9 };


            Vector sl = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Vector an = v1
                        .Expand(v2)
                        .Expand(v3);

            Assert.AreEqual(sl, an);
        }

        [Test]
        public void Vector_Top()
        {
            Vector sl = new[] { 11, 23, 12, 56, 34, 76, 89, 23, 45, 34, 22, 12, 34, 54, 66 };
            var ans = new[] { 5, 6, 14 };
            var cmp = sl.Top(3).ToArray();

            Assert.AreEqual(ans.Length, cmp.Length);
            for (int i = 0; i < ans.Length; i++)
                Assert.AreEqual(ans[i], cmp[i]);
        }
    }
}
