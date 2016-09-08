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
        public void RNN_Unit_Test_1()
        {
            var input = new Supervised.NeuralNetwork.Neuron()
            {
                ActivationFunction = new Math.Functions.Ident(),
                Input = 1.0
            };

            // hh = 0.00845734

            var gru = new Supervised.NeuralNetwork.Recurrent.RecurrentNeuron()
            {
                ActivationFunction = new Math.Functions.Tanh(),
                MemoryGate = new Math.Functions.Logistic(),
                ResetGate = new Math.Functions.Logistic(),
                H = 0,
                Rb = 0.0,
                Zb = 0.0,
                Rh = 0.00822019,
                Rx = -0.00808389,
                Zh = 0.00486728,
                Zx = -0.0040537
            };

            // Z should equal approx. = 0.49898658
            // R should equal approx. = 0.49797904

            // htP should equal approx. = 0.00406561 / 
            // H should equal approx. = 0.00202869

            Supervised.NeuralNetwork.Edge.Create(input, gru, 0.00845734);

            double output = gru.Evaluate();

            Almost.Equal(0.00422846, output, 0.002, "First pass");

            gru.Output = 1.5;

            double output2 = gru.Evaluate();

            Almost.Equal(0.00739980, output2, 0.002, "Second pass");
        }

        [Fact]
        public void RNN_Unit_Test_2()
        {
            var input = new Supervised.NeuralNetwork.Neuron()
            {
                ActivationFunction = new Math.Functions.Ident(),
                Output = 10.0
            };

            var gru = new Supervised.NeuralNetwork.Recurrent.RecurrentNeuron()
            {
                ActivationFunction = new Math.Functions.Tanh(),
                MemoryGate = new Math.Functions.Logistic(),
                ResetGate = new Math.Functions.Logistic(),
                H = 0.0543,
                Rb = 1.5,
                Zb = -1.5,
                Rh = -0.00111453,
                Rx = 0.00112138,
                Zh = 0.00899571,
                Zx = 0.00999628,
            };

            Supervised.NeuralNetwork.Edge.Create(input, gru, 1.0);

            double output = gru.Evaluate();

            //Assert.Equal(0.24144242, output, 0.002, "First pass");
            Almost.Equal(0.18775503, output, 0.002, "First pass");

            input.Output = 20.0;

            double output2 = gru.Evaluate();

            //Assert.Equal(0.40416686, output2, 0.002, "Second pass");
            Almost.Equal(0.30399969, output2, 0.002, "Second pass");
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
