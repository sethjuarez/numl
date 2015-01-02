using System;
using numl.Model;
using System.Linq;
using NUnit.Framework;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Supervised.NeuralNetwork;

namespace numl.Tests.SupervisedTests
{
    [TestFixture, Category("Supervised")]
    public class NeuralNetworkTests : BaseSupervised
    {
        [Test]
        public void Tennis_Tests()
        {
            TennisPrediction(new NeuralNetworkGenerator());
        }

        [Test]
        public void House_Tests()
        {
            HousePrediction(new NeuralNetworkGenerator());
        }

        [Test]
        public void Iris_Tests()
        {
            IrisPrediction(new NeuralNetworkGenerator());
        }

        [Test]
        public void xor_test_learner()
        {
            var xor = new[]
            {
                new { a = false, b = false, c = false },
                new { a = false, b = true, c = true },
                new { a = true, b = false, c = true },
                new { a = true, b = true, c = false },
                new { a = false, b = false, c = false },
                new { a = false, b = true, c = true },
                new { a = true, b = false, c = true },
                new { a = true, b = true, c = false },
                new { a = false, b = false, c = false },
                new { a = false, b = true, c = true },
                new { a = true, b = false, c = true },
                new { a = true, b = true, c = false },
                new { a = false, b = false, c = false },
                new { a = false, b = true, c = true },
                new { a = true, b = false, c = true },
                new { a = true, b = true, c = false },
                new { a = false, b = false, c = false },
                new { a = false, b = true, c = true },
                new { a = true, b = false, c = true },
                new { a = true, b = true, c = false },
            };

            var d = Descriptor.New("XOR")
                              .With("a").As(typeof(bool))
                              .With("b").As(typeof(bool))
                              .Learn("c").As(typeof(bool));

            var generator = new NeuralNetworkGenerator { Descriptor = d };
            var model = Learner.Learn(xor, .75, 1000, generator).Model;
                        
            Matrix x = new[,]
                {{ -1, -1 },  // false, false -> -
                 { -1,  1 },  // false, true  -> +
                 {  1, -1 },  // true, false  -> +
                 {  1,  1 }}; // true, true   -> -

            Vector y = new[] { 0, 0, 0, 0 };

            for (int i = 0; i < x.Rows; i++)
                y[i] = model.Predict(x[i, VectorType.Row]);

        }
    }
}
