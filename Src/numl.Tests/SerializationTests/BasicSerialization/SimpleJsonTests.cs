using System;
using System.Linq;
using Xunit;
using numl.Serialization;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Tests.SerializationTests.BasicSerialization
{
    [Trait("Category", "Serialization")]
    public class SimpleJsonTests : BaseSerialization
    {
        [Fact]
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
                Assert.Equal(v, v3);
            }
        }
        [Fact]
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
                Assert.Equal(m, m3);
            }
        }
    }
}
