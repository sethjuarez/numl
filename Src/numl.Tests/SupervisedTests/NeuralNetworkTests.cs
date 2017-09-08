using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using numl.Math.Probability;
using numl.Model;
using numl.Supervised.NeuralNetwork;
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
            Sampling.SetSeed(1);
            var v = Vector.Rand(100);


            NeuralNetworkGenerator generator = new NeuralNetworkGenerator();
            generator.Descriptor = Descriptor.New("Initial")
                                             .With("P1").As(typeof(double))
                                             .With("P2").As(typeof(double))
                                             .With("P3").As(typeof(double))
                                             .With("P4").As(typeof(double))
                                             .With("P5").As(typeof(double))
                                             .Learn("L").As(typeof(bool));
            int[] layers = new[] { 4 };
            generator.Initialize<ReLU>(layers);
            Assert.Equal(generator.Layers.Length, layers.Length + 1);

        }
    }
}
