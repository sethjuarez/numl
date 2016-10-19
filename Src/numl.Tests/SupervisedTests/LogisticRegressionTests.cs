using System;
using numl.Model;
using System.Linq;
using Xunit;
using numl.Math.LinearAlgebra;
using numl.Math.Functions.Cost;
using System.Collections.Generic;
using numl.Supervised.Regression;
using numl.Math.Functions.Regularization;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class LogisticRegressionTests : BaseSupervised
    {
        [Fact]
        public void Logistic_Regression_Test_Generator()
        {
            Matrix m = new[,] {
                {  0.0512670,   0.6995600 },
                { -0.0927420,   0.6849400 },
                { -0.2137100,   0.6922500 },
                { -0.3750000,   0.5021900 },
                { -0.5132500,   0.4656400 },
                { -0.5247700,   0.2098000 },
                { -0.3980400,   0.0343570 },
                { -0.3058800,  -0.1922500 },
                {  0.0167050,  -0.4042400 },
                {  0.1319100,  -0.5138900 },
                { -0.6111800,  -0.0679820 },
                { -0.6630200,  -0.2141800 },
                { -0.5996500,  -0.4188600 },
                { -0.7263800,  -0.0826020 },
                { -0.8300700,   0.3121300 },
                { -0.7206200,   0.5387400 },
                { -0.5938900,   0.4948800 },
                { -0.4844500,   0.9992700 },
                { -0.0063364,   0.9992700 },
                {  0.6326500,  -0.0306120 },
            };

            Vector y = new Vector(new double[] {
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            });

            Vector test = new Vector(new double[] { 0.1319100, -0.513890 });

            var generator = new LogisticRegressionGenerator() { Lambda = 1, LearningRate = 0.1, PolynomialFeatures = 6, MaxIterations = 400, NormalizeFeatures = true };

            var model2 = generator.Generate(m, y);
            double p = model2.Predict(test);

            Assert.Equal(1d, p);
        }

        [Fact]
        public void Logistic_Regression_Test_CostFunction_1()
        {
            Matrix X = new[,]
            {{ 1, 1, 1 },
             { 1, 1, 1 },
             { 1, 1, 1 },
             { 8, 1, 6 },
             { 3, 5 ,7 },
             { 4, 9, 2 }};

            Vector y = new Vector(new double[] { 1, 0, 1, 0, 1, 0 });
            Vector theta = new Vector(new double[] { 0, 1, 0 });

            ICostFunction logisticCostFunction = new LogisticCostFunction()
            {
                X = X,
                Y = y,
                Lambda = 3,
                Regularizer = new L2Regularizer()
            };

            double cost = logisticCostFunction.ComputeCost(theta.Copy());

            theta = logisticCostFunction.ComputeGradient(theta.Copy());

            Assert.Equal(2.2933d, System.Math.Round(cost, 4));

            Assert.Equal(1.6702d, System.Math.Round(theta[0], 4));
            Assert.Equal(2.1483d, System.Math.Round(theta[1], 4));
            Assert.Equal(1.0887d, System.Math.Round(theta[2], 4));
        }

        [Fact]
        public void Logistic_Regression_Test_CostFunction_2_WithoutRegularization()
        {
            Matrix X = new[,] {
             { 8, 1, 6 },
             { 3, 5 ,7 },
             { 4, 9, 2 }};

            Vector y = new Vector(new double[] { 1, 1, 0 });
            Vector theta = new Vector(new double[] { 0, 1, 0 });

            ICostFunction logisticCostFunction = new LogisticCostFunction()
            {
                X = X,
                Y = y,
                Lambda = 0,
            };

            double cost = logisticCostFunction.ComputeCost(theta.Copy());

            theta = logisticCostFunction.ComputeGradient(theta.Copy());

            Assert.Equal(3.1067d, System.Math.Round(cost, 4));

            Assert.Equal(0.6093d, System.Math.Round(theta[0], 4));
            Assert.Equal(2.8988d, System.Math.Round(theta[1], 4));
            Assert.Equal(0.1131d, System.Math.Round(theta[2], 4));
        }

        [Fact]
        public void Logistic_Regression_Test_CostFunction_2_WithRegularization()
        {
            Matrix X = new[,] {
             { 8, 1, 6 },
             { 3, 5 ,7 },
             { 4, 9, 2 }};

            Vector y = new Vector(new double[] { 1, 1, 0 });
            Vector theta = new Vector(new double[] { 0, 1, 0 });

            ICostFunction logisticCostFunction = new LogisticCostFunction()
            {
                X = X,
                Y = y,
                Lambda = 3,
                Regularizer = new L2Regularizer()
            };

            double cost = logisticCostFunction.ComputeCost(theta.Copy());

            theta = logisticCostFunction.ComputeGradient(theta.Copy());

            Assert.Equal(3.6067d, System.Math.Round(cost, 4));

            Assert.Equal(0.6093d, System.Math.Round(theta[0], 4));
            Assert.Equal(3.8988d, System.Math.Round(theta[1], 4));
            Assert.Equal(0.1131d, System.Math.Round(theta[2], 4));
        }

        [Fact]
        public void Logistic_Regression_XOR_Learner()
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

            var generator = new LogisticRegressionGenerator() { Descriptor = d, PolynomialFeatures = 3, LearningRate = 0.01, Lambda = 0 };
            var model = Learner.Learn(xor, 0.9, 5, generator).Model;

            Matrix x = new[,]
                {{ -1, -1 },  // false, false -> -
                 { -1,  1 },  // false, true  -> +
                 {  1, -1 },  // true, false  -> +
                 {  1,  1 }}; // true, true   -> -

            Vector y_actual = new Vector(new[] { 0d, 1d, 1d, 0d });

            Vector y = new Vector(x.Rows);

            for (int i = 0; i < x.Rows; i++)
                y[i] = model.Predict(x[i, VectorType.Row]);

            var scored = Supervised.Score.ScorePredictions(y, y_actual);

            Assert.True(scored.Accuracy >= 0.75);

            Console.WriteLine($"LR Model Score: { scored }");
        }
    }
}
