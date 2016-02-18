using System;
using System.Linq;
using NUnit.Framework;
using numl.Serialization;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Tests.SerializationTests.BasicSerialization
{
    [TestFixture]
    public class SimpleJsonTests : BaseSerialization
    {
        [Test]
        public void VectorSerializationTest()
        {
            Vector v = new[] {
                System.Math.PI,
                System.Math.PI / 2.3,
                System.Math.PI * 1.2,
                System.Math.PI,
                System.Math.PI / 2.3,
                System.Math.PI * 1.2
            };

            using (var w = GetWriter()) w.WriteVector(v);

            using (var reader = GetReader())
            {
                Vector v3 = reader.ReadVector();
                Assert.AreEqual(v, v3);
            }
        }
        [Test]
        public void MatrixSerializationTest()
        {
            Matrix m = new[,] {
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 }
            };

            using (var w = GetWriter()) w.WriteMatrix(m);

            using (var reader = GetReader())
            {
                Matrix m3 = reader.ReadMatrix();
                Assert.AreEqual(m, m3);
            }
        }
    }
}
