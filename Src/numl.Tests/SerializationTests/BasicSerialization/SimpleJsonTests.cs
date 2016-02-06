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

            SerializeWith<VectorSerializer>(v);
            var vector = DeserializeWith<VectorSerializer>();

            Assert.AreEqual(v, vector);
        }
        [Test]
        public void MatrixSerializationTest()
        {
            Matrix m1 = new[,] {
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 },
                { System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2, System.Math.PI, System.Math.PI / 2.3, System.Math.PI * 1.2 }
            };

            SerializeWith<MatrixSerializer>(m1);
            var matrix = DeserializeWith<MatrixSerializer>();

            Assert.AreEqual(m1, matrix);
        }
    }
}
