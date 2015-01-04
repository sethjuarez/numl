using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Supervised.Regression;
using numl.Math.LinearAlgebra;

namespace numl.Tests.SupervisedTests
{
    [TestFixture, Category("Supervised")]
    public class LinearRegressionTests : BaseSupervised
    {
        [Test]
        public void Linear_Regression_Test_House_Predictions_Normal()
        {
            // test house prices based on ft-sq and no# bedrooms
            Matrix x = new [,]
                {
                    {2104, 3},
                    {1600, 3},
                    {2400, 3},
                    {1416, 2},
                    {3000, 4},
                    {1985, 4},
                    {1534, 3},
                    {1427, 3},
                    {1380, 3},
                    {1494, 3}
                };

            Vector y = new[] 
                { 
                    399900,
                    329900,
                    369000,
                    232000,
                    539900,
                    299900,
                    314900,
                    198999,
                    212000,
                    242500
                };

            LinearRegressionGenerator generator = new LinearRegressionGenerator() { LearningRate = 0.01, MaxIterations = 400 };
            var model = generator.Generate(x.Copy(), y.Copy());
            var priceEqns = model.Predict(new Vector(new double[] { 1650, 3 }));

            double epsilon = 0.01;

            var actualEqns = 280653d;

            Assert.GreaterOrEqual(actualEqns + (actualEqns * epsilon), priceEqns);
            Assert.LessOrEqual(actualEqns - (actualEqns * epsilon), priceEqns);
        }

        [Test]
        public void Linear_Regression_Test_House_Predictions_Gradient()
        {
            // test house prices based on ft-sq and no# bedrooms
            Matrix x = new [,]
                {
                    {2104, 3},
                    {1600, 3},
                    {2400, 3},
                    {1416, 2},
                    {3000, 4},
                    {1985, 4},
                    {1534, 3},
                    {1427, 3},
                    {1380, 3},
                    {1494, 3}
                };

            Vector y = new[] 
                { 
                    399900,
                    329900,
                    369000,
                    232000,
                    539900,
                    299900,
                    314900,
                    198999,
                    212000,
                    242500
                };

            LinearRegressionGenerator generator = new LinearRegressionGenerator() { LearningRate = 0.01, MaxIterations = 400, UseGradientDescent = true };
            var model = generator.Generate(x.Copy(), y.Copy());
            var priceGrad = model.Predict(new Vector(new double[] { 1650, 3 }));
            
            double epsilon = 0.01;

            var actualGrad = 277440d;

            Assert.GreaterOrEqual(actualGrad + (actualGrad * epsilon), priceGrad);
            Assert.LessOrEqual(actualGrad - (actualGrad * epsilon), priceGrad);
        }
    }
}
