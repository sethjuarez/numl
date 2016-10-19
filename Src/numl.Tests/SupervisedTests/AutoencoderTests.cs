using numl.Math.LinearAlgebra;
using numl.Model;
using numl.Supervised.NeuralNetwork;
using numl.Supervised;
using numl.Supervised.NeuralNetwork.Encoders;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class AutoencoderTests
    {
        [Fact]
        public void Autoencoder_Dense_Test()
        {

            // create a distributed range with zero mean
            var distribution = new[]
            {
                new { Test = true, A = 1.01, B = 2.60, C = 3.1, D = 4.10, E = 5.21, F = 6.31 },
                new { Test = true, A = 1.02, B = 2.65, C = 3.2, D = 4.11, E = 5.22, F = 6.32 },
                new { Test = true, A = 1.03, B = 2.70, C = 3.3, D = 4.12, E = 5.23, F = 6.33 },
                new { Test = true, A = 1.04, B = 2.75, C = 3.4, D = 4.13, E = 5.24, F = 6.34 },
                new { Test = true, A = 1.05, B = 2.80, C = 3.5, D = 4.14, E = 5.25, F = 6.35 },
                new { Test = true, A = 1.06, B = 2.85, C = 3.6, D = 4.15, E = 5.26, F = 6.36 },
                new { Test = true, A = 1.07, B = 2.90, C = 3.7, D = 4.16, E = 5.27, F = 6.37 },
                new { Test = true, A = 1.08, B = 2.95, C = 3.8, D = 4.17, E = 5.28, F = 6.38 },
                new { Test = true, A = 1.09, B = 2.99, C = 3.9, D = 4.18, E = 5.29, F = 6.39 },
                new { Test = true, A = 1.10, B = 2.99, C = 3.9, D = 4.19, E = 5.30, F = 6.40 },
                new { Test = true, A = 1.09, B = 2.99, C = 3.9, D = 4.18, E = 5.29, F = 6.39 },
                new { Test = true, A = 1.08, B = 2.95, C = 3.8, D = 4.17, E = 5.28, F = 6.38 },
                new { Test = true, A = 1.07, B = 2.90, C = 3.7, D = 4.16, E = 5.27, F = 6.37 },
                new { Test = true, A = 1.06, B = 2.85, C = 3.6, D = 4.15, E = 5.26, F = 6.36 },
                new { Test = true, A = 1.05, B = 2.80, C = 3.5, D = 4.14, E = 5.25, F = 6.35 },
                new { Test = true, A = 1.04, B = 2.75, C = 3.4, D = 4.13, E = 5.24, F = 6.34 },
                new { Test = true, A = 1.03, B = 2.70, C = 3.3, D = 4.12, E = 5.23, F = 6.33 },
                new { Test = true, A = 1.02, B = 2.65, C = 3.2, D = 4.11, E = 5.22, F = 6.32 },
                new { Test = true, A = 1.01, B = 2.60, C = 3.1, D = 4.10, E = 5.21, F = 6.31 },
            };

            var test = new Matrix(new double[,]
            {
                { 3.05, 2.80, 3.5, 4.14, 7.25, 6.35 },
                { 1.90, 2.99, 3.9, 8.19, 5.30, 6.40 },
                { 1.07, 2.90, 4.7, 4.16, 5.27, 9.37 }
            });

            var d = Descriptor.New("DIST")
                              .With("A").As(typeof(double))
                              .With("B").As(typeof(double))
                              .With("C").As(typeof(double))
                              .With("D").As(typeof(double))
                              .With("E").As(typeof(double))
                              .With("F").As(typeof(double))
                              .Learn("Test").As(typeof(bool)); // label not used 

            var train = d.ToExamples(distribution);

            var generator = new AutoencoderGenerator { Descriptor = d, LearningRate = 0.1, OutputFunction = new Math.Functions.Ident(), Sparsity = 0.2, SparsityWeight = 3.0, Density = 12 };
            var encoder = generator.Generate(train.Item1, train.Item1);

            for (int i = 0; i < test.Rows; i++)
            {
                var score1 = Score.ScorePredictions(encoder.PredictSequence(test[i, VectorType.Row]), test[i, VectorType.Row]);
                Assert.True(score1.MeanAbsError <= 1.00);
            }

        }

        [Fact]
        public void Autoencoder_Sparse_Test()
        {

            // create a distributed range with zero mean
            var distribution = new[]
            {
                new { Test = true, A = 1.01, B = 2.60, C = 3.1, D = 4.10, E = 5.21, F = 6.31 },
                new { Test = true, A = 1.02, B = 2.65, C = 3.2, D = 4.11, E = 5.22, F = 6.32 },
                new { Test = true, A = 1.03, B = 2.70, C = 3.3, D = 4.12, E = 5.23, F = 6.33 },
                new { Test = true, A = 1.04, B = 2.75, C = 3.4, D = 4.13, E = 5.24, F = 6.34 },
                new { Test = true, A = 1.05, B = 2.80, C = 3.5, D = 4.14, E = 5.25, F = 6.35 },
                new { Test = true, A = 1.06, B = 2.85, C = 3.6, D = 4.15, E = 5.26, F = 6.36 },
                new { Test = true, A = 1.07, B = 2.90, C = 3.7, D = 4.16, E = 5.27, F = 6.37 },
                new { Test = true, A = 1.08, B = 2.95, C = 3.8, D = 4.17, E = 5.28, F = 6.38 },
                new { Test = true, A = 1.09, B = 2.99, C = 3.9, D = 4.18, E = 5.29, F = 6.39 },
                new { Test = true, A = 1.10, B = 2.99, C = 3.9, D = 4.19, E = 5.30, F = 6.40 },
                new { Test = true, A = 1.09, B = 2.99, C = 3.9, D = 4.18, E = 5.29, F = 6.39 },
                new { Test = true, A = 1.08, B = 2.95, C = 3.8, D = 4.17, E = 5.28, F = 6.38 },
                new { Test = true, A = 1.07, B = 2.90, C = 3.7, D = 4.16, E = 5.27, F = 6.37 },
                new { Test = true, A = 1.06, B = 2.85, C = 3.6, D = 4.15, E = 5.26, F = 6.36 },
                new { Test = true, A = 1.05, B = 2.80, C = 3.5, D = 4.14, E = 5.25, F = 6.35 },
                new { Test = true, A = 1.04, B = 2.75, C = 3.4, D = 4.13, E = 5.24, F = 6.34 },
                new { Test = true, A = 1.03, B = 2.70, C = 3.3, D = 4.12, E = 5.23, F = 6.33 },
                new { Test = true, A = 1.02, B = 2.65, C = 3.2, D = 4.11, E = 5.22, F = 6.32 },
                new { Test = true, A = 1.01, B = 2.60, C = 3.1, D = 4.10, E = 5.21, F = 6.31 },
            };

            var test = new Matrix(new double[,]
            {
                { 3.05, 2.80, 3.5, 4.14, 7.25, 6.35 },
                { 1.90, 2.99, 3.9, 8.19, 5.30, 6.40 },
                { 1.07, 2.90, 4.7, 4.16, 5.27, 9.37 }
            });

            var d = Descriptor.New("DIST")
                              .With("A").As(typeof(double))
                              .With("B").As(typeof(double))
                              .With("C").As(typeof(double))
                              .With("D").As(typeof(double))
                              .With("E").As(typeof(double))
                              .With("F").As(typeof(double))
                              .Learn("Test").As(typeof(bool)); // label not used 

            var generator = new AutoencoderGenerator { Descriptor = d, LearningRate = 0.1, OutputFunction = new Math.Functions.Ident(), Sparsity = 0.2, SparsityWeight = 1.0 };
            var encoder = (AutoencoderModel) generator.Generate(distribution);

            Vector avg = d.ToExamples(distribution).Item1.Mean(VectorType.Row);

            for (int i = 0; i < test.Rows; i++)
            {
                var score = Score.ScorePredictions(encoder.PredictSequence(test[i, VectorType.Row]), test[i, VectorType.Row]);
                Assert.True(score.MeanAbsError <= 1.0);
            }
        }
    }
}
