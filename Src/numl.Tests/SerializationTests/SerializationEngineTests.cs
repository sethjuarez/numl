using Xunit;
using System;
using System.Linq;
using numl.Serialization;
using numl.Supervised.NeuralNetwork;
using numl.Supervised.NeuralNetwork.Serializer;


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
