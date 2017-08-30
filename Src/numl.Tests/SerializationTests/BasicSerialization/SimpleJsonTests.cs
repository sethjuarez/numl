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

            var file = GetPath();
            JsonWriter.Save(v, file);
            var v3 = JsonReader.ReadVector(file);
            Assert.Equal(v, v3);
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

            var file = GetPath();
            JsonWriter.Save(m, file);
            var m3 = JsonReader.ReadMatrix(file);
            Assert.Equal(m, m3);
        }
    }
}
