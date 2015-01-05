using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Supervised.Regression;
using numl.Math.LinearAlgebra;
using numl.Functions;

namespace numl.Tests.SupervisedTests
{
    [TestFixture, Category("Supervised")]
    public class LinearRegressionTests : BaseSupervised
    {
        [Test]
        public void Linear_Regression_Test_CostFunction()
        {
            Vector theta = new Vector(new double[] { 1, 1 });

            Matrix X = new[,] {
                        { 1, -15.9368 },
                        { 1, -29.1530 },
                        { 1,  36.1895 },
                        { 1,  37.4922 },
                        { 1, -48.0588 },
                        { 1,  -8.9415 },
                        { 1,  15.3078 },
                        { 1, -34.7063 },
                        { 1,   1.3892 },
                        { 1, -44.3838 },
                        { 1,   7.0135 },
                        { 1,  22.7627 }
            };

            Vector y = new Vector(new double[] {
                         2.1343,
                         1.1733,
                        34.3591,
                        36.8380,
                         2.8090,
                         2.1211,
                        14.7103,
                         2.6142,
                         3.7402,
                         3.7317,
                         7.6277,
                        22.7524
            });

            ICostFunction costFunction = new Functions.CostFunctions.LinearCostFunction();
            double cost = costFunction.ComputeCost(theta, X, y, 0, null);
            Vector grad = costFunction.ComputeGradient(theta, X, y, 0, null);

            Assert.AreEqual(303.95d, System.Math.Round(cost, 2));

            Assert.AreEqual(new double[] { -15.3, 598.2 }, grad.Select(s => System.Math.Round(s, 1)).ToArray());
        }

        [Test]
        public void Linear_Regression_Test_CostFunction_Regularized()
        {
            Vector theta = new Vector(new double[] { 1, 1 });

            Matrix X = new[,] {
                        { 1, -15.9368 },
                        { 1, -29.1530 },
                        { 1,  36.1895 },
                        { 1,  37.4922 },
                        { 1, -48.0588 },
                        { 1,  -8.9415 },
                        { 1,  15.3078 },
                        { 1, -34.7063 },
                        { 1,   1.3892 },
                        { 1, -44.3838 },
                        { 1,   7.0135 },
                        { 1,  22.7627 }
            };

            Vector y = new Vector(new double[] {
                         2.1343,
                         1.1733,
                        34.3591,
                        36.8380,
                         2.8090,
                         2.1211,
                        14.7103,
                         2.6142,
                         3.7402,
                         3.7317,
                         7.6277,
                        22.7524
            });

            ICostFunction costFunction = new Functions.CostFunctions.LinearCostFunction();
            IRegularizer regulariser = new Regularization();

            double cost = costFunction.ComputeCost(theta, X, y, 1, regulariser);
            Vector grad = costFunction.ComputeGradient(theta, X, y, 1, regulariser);

            Assert.AreEqual(303.99, System.Math.Round(cost, 2));

            Assert.AreEqual(new double[] { -15.3, 598.3 }, grad.Select(s => System.Math.Round(s, 1)).ToArray());
        }

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

            LinearRegressionGenerator generator = new LinearRegressionGenerator() { LearningRate = 0.01, MaxIterations = 400, Lambda = 0 };
            var model = generator.Generate(x.Copy(), y.Copy());
            var priceEqns = model.Predict(new Vector(new double[] { 1650, 3 }));

            double actualEqns = 278735d;

            Assert.AreEqual(actualEqns, System.Math.Round(priceEqns, 0));
        }

        [Test]
        public void Linear_Regression_Test_House_Predictions_Regularized()
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

            LinearRegressionGenerator generator = new LinearRegressionGenerator() { LearningRate = 0.01, MaxIterations = 400, Lambda = 1 };
            var model = generator.Generate(x.Copy(), y.Copy());
            var priceGrad = model.Predict(new Vector(new double[] { 1650, 3 }));

            double actualGrad = 280942d;

            Assert.AreEqual(actualGrad, System.Math.Round(priceGrad, 0));
        }
    }
}
