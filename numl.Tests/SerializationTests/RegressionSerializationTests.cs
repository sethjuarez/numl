using numl.Math.LinearAlgebra;
using numl.Model;
using numl.Supervised.Regression;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.SerializationTests
{
    [TestFixture, Category("Serialization")]
    public class RegressionSerializationTests : BaseSerialization
    {
        //[Test]
        public void Save_And_Load_LogisticRegression()
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

            var generator = new LogisticRegressionGenerator() { Lambda = 1, LearningRate = 0.01, PolynomialFeatures = 6, MaxIterations = 400 };

            var model = generator.Generate(m, y) as LogisticRegressionModel;

            Serialize(model);

            var lmodel = Deserialize<LogisticRegressionModel>();
            Assert.AreEqual(model.Theta, lmodel.Theta);
            Assert.AreEqual(model.PolynomialFeatures, lmodel.PolynomialFeatures);
            Assert.AreEqual(model.LogisticFunction, lmodel.LogisticFunction);
        }

        //[Test]
        public void Save_And_Load_LinearRegression()
        {
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
            var model = generator.Generate(x.Copy(), y.Copy()) as LinearRegressionModel;

            Serialize(model);

            var lmodel = Deserialize<LinearRegressionModel>();
            Assert.AreEqual(model.Theta, lmodel.Theta);
        }
    }
}
