using System;
using numl.Model;
using System.Linq;
using Xunit;
using numl.Math.LinearAlgebra;
using numl.Math.Functions.Cost;
using System.Collections.Generic;
using numl.Supervised.Regression;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class LinearRegressionTests : BaseSupervised
    {
        [Fact]
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

            ICostFunction costFunction = new LinearCostFunction()
            {
                X = X,
                Y = y,
                Lambda = 0
            };
            double cost = costFunction.ComputeCost(theta);
            Vector grad = costFunction.ComputeGradient(theta);

            Assert.Equal(303.95d, System.Math.Round(cost, 2));

            Assert.Equal(new double[] { -15.3, 598.2 }, grad.Select(s => System.Math.Round(s, 1)).ToArray());
        }

        [Fact]
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

            ICostFunction costFunction = new LinearCostFunction()
            {
                X = X,
                Y = y,
                Lambda = 1,
                Regularizer = new numl.Math.Functions.Regularization.L2Regularizer()
            };

            double cost = costFunction.ComputeCost(theta);
            Vector grad = costFunction.ComputeGradient(theta);

            Assert.Equal(303.99, System.Math.Round(cost, 2));

            Assert.Equal(new double[] { -15.3, 598.3 }, grad.Select(s => System.Math.Round(s, 1)).ToArray());
        }

        [Fact]
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

            LinearRegressionGenerator generator = new LinearRegressionGenerator() { LearningRate = 0.01, MaxIterations = 400, Lambda = 0, NormalizeFeatures = true };
            var model = generator.Generate(x.Copy(), y.Copy());
            var priceEqns = model.Predict(new Vector(new double[] { 1650, 3 }));

            // CK 150929: increased due to improvements in optimisation
            double actualEqns = 295107.0d;

            Almost.Equal(actualEqns, System.Math.Round(priceEqns, 0), 5000);
        }

        [Fact]
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

            LinearRegressionGenerator generator = new LinearRegressionGenerator() { LearningRate = 0.01, MaxIterations = 400, Lambda = 1, NormalizeFeatures = true };
            var model = generator.Generate(x.Copy(), y.Copy());
            var priceGrad = model.Predict(new Vector(new double[] { 1650, 3 }));

            double actualGrad = 296500.0d;

            Almost.Equal(actualGrad, System.Math.Round(priceGrad, 0), 5000);
        }

        [Fact]
        public void Linear_Regression_Learner_Test()
        {
            var datum = new[]
            {
                new { X = 1.0, Y = 1.0 },
                new { X = 2.0, Y = 2.0 },
                new { X = 3.0, Y = 1.3 },
                new { X = 4.0, Y = 3.75 },
                new { X = 5.0, Y = 5.25 }
            };

            var d = Descriptor.New("DATUM")
                              .With("X").As(typeof(double))
                              .Learn("Y").As(typeof(double));

            var generator = new LinearRegressionGenerator() { Descriptor = d };
            var model = Learner.Learn(datum, 0.9, 5, generator);

            Assert.True(0.8 >= model.Accuracy);
        }
    }
}
