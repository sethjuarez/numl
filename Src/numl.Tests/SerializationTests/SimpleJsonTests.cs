using numl.Math.LinearAlgebra;
using numl.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.SerializationTests
{
    [TestFixture]
    public class SimpleJsonTests : BaseSerialization
    {
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

            var path = GetPath();
            var file = string.Format(path, "MatrixSerializationTest");
            MatrixSerializer m = new MatrixSerializer();
            using (var f = new StreamWriter(file, false))
                m.Write(f, m1);

            using (var f = new StreamReader(file))
            {
                var matrix = m.Deserialize(f);
                Assert.AreEqual(m1, matrix);
            }
        }
    }
}
