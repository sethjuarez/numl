using numl.Serialization;
using numl.Serialization.Data;
using numl.Serialization.Supervised.NeuralNetwork;
using numl.Supervised.NeuralNetwork;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace numl.Tests.SerializationTests
{
    [TestFixture, Category("Serialization")]
    public class SerializationEngineTests : BaseSerialization
    {
        [Test]
        public void Basic_Registration_Test()
        {
            var type = typeof(Network);
            var serialzer = type.GetSerializer();
            Assert.AreEqual(typeof(NetworkSerializer), serialzer.GetType());
        }
    }
}
