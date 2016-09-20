using numl.Serialization;
using numl.Serialization.Data;
using numl.Serialization.Supervised.NeuralNetwork;
using numl.Supervised.NeuralNetwork;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace numl.Tests.SerializationTests
{
    [Trait("Category", "Serialization")]
    public class SerializationEngineTests : BaseSerialization
    {
        [Fact]
        public void Basic_Registration_Test()
        {
            var type = typeof(Network);
            var serialzer = type.GetSerializer();
            Assert.Equal(typeof(NetworkSerializer), serialzer.GetType());
        }
    }
}
