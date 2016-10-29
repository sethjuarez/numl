using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using numl;
using numl.Model;
using numl.Math.LinearAlgebra;
using numl.Supervised.NeuralNetwork;
using numl.Supervised.NeuralNetwork.Recurrent;

using Xunit;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class RecurrentNetworkTests
    {
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
                UpdateGate = new Math.Functions.Logistic(),
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
                ActivationFunction = new Math.Functions.Ident()
            };

            input.Input = 10.0;

            var gru = new Supervised.NeuralNetwork.Recurrent.RecurrentNeuron()
            {
                ActivationFunction = new Math.Functions.Tanh(),
                UpdateGate = new Math.Functions.Logistic(),
                ResetGate = new Math.Functions.Logistic(),
                H = 0.0543,
                Rb = 1.5,
                Zb = -1.5,
                Rh = -0.00111453,
                Rx = 0.00112138,
                Zh = 0.00899571,
                Zx = 0.00999628,
                Hh = 0.00423760
            };

            Supervised.NeuralNetwork.Edge.Create(input, gru, 1.0);

            double output = gru.Evaluate();

            Almost.Equal(0.24144243, output, 0.00001, "1: Hidden state");
            Almost.Equal(0.81923206, gru.R, 0.00001, "1: Reset value");
            Almost.Equal(0.19788773, gru.Z, 0.00001, "1: Update value");

            input.Input = 20.0;

            double output2 = gru.Evaluate();

            Almost.Equal(0.40416687, output2, 0.00001, "Second pass");
            Almost.Equal(0.82085611, gru.R, 0.00001, "2: Reset value");
            Almost.Equal(0.21451824, gru.Z, 0.00001, "2: Update value");
        }

        [Fact]
        public void Recurrent_Sequence_Test()
        {

            Matrix X = new Matrix(new double[,]
            {
                { 0 },
                { 1 },
                { 1 },
                { 1 },
                { 0 }
            });

            Vector Y = new double[]
            {
                1,
                1,
                1,
                0,
                0
            };

            var generator = new GatedRecurrentGenerator() { SequenceLength = X.Rows, NormalizeFeatures = false };
            var model = (GatedRecurrentModel) generator.Generate(X, Y);

            Vector result = Vector.Zeros(Y.Length);

            for (int x = 0; x < result.Length; x++)
                result[x] = model.Predict(X[x]);

            //TODO: Uncomment when gradient bug fixed.
            //Assert.Equal(Y, result);
        }
    }
}
