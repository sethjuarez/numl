using numl.Math.LinearAlgebra;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Supervised.Regression;
using numl.Functions;

namespace numl.Tests.SupervisedTests
{
    [TestFixture, Category("Supervised")]
    public class LogisticRegressionTests : BaseSupervised
    {
        [Test]
        public void Logistic_Regression_Test_Prediction()
        {
            Matrix m = new[,] 
            {{ 8, 1, 6 },
             { 3, 5 ,7 },
             { 4, 9, 2 }};

            var model = new LogisticRegressionModel() { 
                LogisticFunction = new Math.Functions.Logistic(), 
                Theta = new Vector(new double[] { 1, 2, 1, -9 }) 
            };

            var p = model.Predict(new Vector(new double[] { 8, 1, 6 }));

            Assert.AreEqual(1d, p);
        }

        [Test]
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

            Vector test = new Vector(new double[] { 0.1319100, -0.513890
                 });

            var generator = new LogisticRegressionGenerator() { Lambda = 1, LearningRate = 0.01, PolynomialFeatures = 6, MaxIterations = 400 };

            var model2 = generator.Generate(m, y);
            double p = model2.Predict(test);

            Assert.AreEqual(1d, p);
        }

        [Test]
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

            ICostFunction logisticCostFunction = new Functions.CostFunctions.LogisticCostFunction();
            IRegularizer regularizer = new Functions.Regularization();

            double cost = logisticCostFunction.ComputeCost(theta.Copy(), X, y, 3, regularizer);
            
            theta = logisticCostFunction.ComputeGradient(theta.Copy(), X, y, 3, regularizer);

            Assert.AreEqual(2.2933d, System.Math.Round(cost, 4));

            Assert.AreEqual(1.6702d, System.Math.Round(theta[0], 4));
            Assert.AreEqual(2.1483d, System.Math.Round(theta[1], 4));
            Assert.AreEqual(1.0887d, System.Math.Round(theta[2], 4));
        }

        [Test]
        public void Logistic_Regression_Test_CostFunction_2_WithoutRegularization()
        {
            Matrix X = new[,] {
             { 8, 1, 6 },
             { 3, 5 ,7 },
             { 4, 9, 2 }};

            Vector y = new Vector(new double[] { 1, 1, 0 });
            Vector theta = new Vector(new double[] { 0, 1, 0 });

            ICostFunction logisticCostFunction = new Functions.CostFunctions.LogisticCostFunction();

            double cost = logisticCostFunction.ComputeCost(theta.Copy(), X, y, 0, null);
            
            theta = logisticCostFunction.ComputeGradient(theta.Copy(), X, y, 0, null);

            Assert.AreEqual(3.1067d, System.Math.Round(cost, 4));

            Assert.AreEqual(0.6093d, System.Math.Round(theta[0], 4));
            Assert.AreEqual(2.8988d, System.Math.Round(theta[1], 4));
            Assert.AreEqual(0.1131d, System.Math.Round(theta[2], 4));
        }

        [Test]
        public void Logistic_Regression_Test_CostFunction_2_WithRegularization()
        {
            Matrix X = new[,] {
             { 8, 1, 6 },
             { 3, 5 ,7 },
             { 4, 9, 2 }};

            Vector y = new Vector(new double[] { 1, 1, 0 });
            Vector theta = new Vector(new double[] { 0, 1, 0 });

            ICostFunction logisticCostFunction = new Functions.CostFunctions.LogisticCostFunction();
            IRegularizer regularizer = new Functions.Regularization();

            double cost = logisticCostFunction.ComputeCost(theta.Copy(), X, y, 3, regularizer);
            
            theta = logisticCostFunction.ComputeGradient(theta.Copy(), X, y, 3, regularizer);

            Assert.AreEqual(3.6067d, System.Math.Round(cost, 4));

            Assert.AreEqual(0.6093d, System.Math.Round(theta[0], 4));
            Assert.AreEqual(3.8988d, System.Math.Round(theta[1], 4));
            Assert.AreEqual(0.1131d, System.Math.Round(theta[2], 4));
        }
    }
}
