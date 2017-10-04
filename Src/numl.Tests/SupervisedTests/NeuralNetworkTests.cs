using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using numl.Math.Probability;
using numl.Model;
using numl.Supervised.NeuralNetwork;
using numl.Tests.Data;
using System;
using Xunit;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class NeuralNetworkTests : BaseSupervised
    {
        [Fact]
        public void InitializationTest()
        {
            // for testing consistency
            NeuralNetworkGenerator generator = new NeuralNetworkGenerator();
            generator.Descriptor = Descriptor.Create<Tennis>();

            int[] layers = new[] { 3, 2 };
            generator.Initialize<ReLU>(layers);
            Assert.Equal(generator.Layers.Length, layers.Length + 1);
        }

        [Fact]
        public void ForwardTest()
        {
            // for testing consistency
            NeuralNetworkGenerator generator = new NeuralNetworkGenerator();
            generator.Descriptor = Descriptor.Create<Tennis>();

            int[] layers = new[] { 3, 2 };
            generator.Initialize<ReLU>(layers);

            var (X, y) = generator.Descriptor.ToExamples(Tennis.GetData());
            generator.Forward(X, new ReLU());


        }
    }
}
