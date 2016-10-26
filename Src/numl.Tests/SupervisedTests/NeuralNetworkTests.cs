using System;
using numl.Model;
using System.Linq;
using Xunit;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Supervised.NeuralNetwork;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class NeuralNetworkTests : BaseSupervised
    {
        [Fact]
        public void Tennis_Tests()
        {
            TennisPrediction(new NeuralNetworkGenerator());
        }

        [Fact]
        public void House_Tests()
        {
            HousePrediction(new NeuralNetworkGenerator());
        }

        [Fact]
        public void Iris_Tests()
        {
            IrisPrediction(new NeuralNetworkGenerator());
        }

        [Fact]
        public void XOR_Test_Learner()
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
            var model = Learner.Learn(xor, .75, 10, generator).Model;
                        
            Matrix x = new[,]
                {{ -1, -1 },  // false, false -> -
                 { -1,  1 },  // false, true  -> +
                 {  1, -1 },  // true, false  -> +
                 {  1,  1 }}; // true, true   -> -

            Vector actual = new int[] { -1, 1, 1, -1 };

            Vector y = new[] { 0, 0, 0, 0 };

            for (int i = 0; i < x.Rows; i++)
                y[i] = model.Predict(x[i, VectorType.Row]);

            var score = numl.Supervised.Score.ScorePredictions(y, actual);
            Console.WriteLine(string.Format("Neural Network XOR Test (score) =>\n{0}", score.RMSE));
        }

        [Fact]
        public void Neural_Network_Study_Test()
        {
            var data = new[] {
                new { Study = 2.0, Beer = 3.0, Passed = false},
                new { Study = 3.0, Beer = 4.0, Passed = false},
                new { Study = 1.0, Beer = 6.0, Passed = false},
                new { Study = 4.0, Beer = 5.0, Passed = false},
                new { Study = 6.0, Beer = 2.0, Passed = true},
                new { Study = 8.0, Beer = 3.0, Passed = true},
                new { Study = 12.0, Beer = 1.0, Passed = true},
                new { Study = 3.0, Beer = 2.0, Passed = true}
            };

            var descriptor = Descriptor.New("Student")
                .With("Study").As(typeof(double))
                .With("Beer").As(typeof(double))
                .Learn("Passed").As(typeof(bool));

            NeuralNetworkGenerator generator = new NeuralNetworkGenerator()
            {
                Descriptor = descriptor,
                NormalizeFeatures = true,
                LearningRate = 0.1
            };

            var model = Learner.Learn(data, 0.8, 10, generator).Model;

            var test = model.PredictValue(new { Study = 7.0, Beer = 1.0, Passed = false });

            Assert.Equal(true, test);
        }
    }
}
