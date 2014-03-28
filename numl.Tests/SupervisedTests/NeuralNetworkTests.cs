using System;
using numl.Data;
using numl.Model;
using numl.Utils;
using System.Linq;
using numl.Supervised;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Tests.Data;
using System.IO;
using numl.Supervised.DecisionTree;
using numl.Supervised.NeuralNetwork;
using numl.Math.LinearAlgebra;
using numl.Math.Probability;

namespace numl.Tests.SupervisedTests
{
    [TestFixture]
    public class NeuralNetworkTests
    {
        [Test]
        public void Iris_DT_and_Prediction()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();
            var generator = new NeuralNetworkGenerator();
            var model = generator.Generate(description, data);

            // should be Iris-Setosa
            Iris iris = new Iris
            {
                PetalWidth = 0.5m,
                PetalLength = 2.3m,
                SepalLength = 2.1m,
                SepalWidth = 2.1m
            };

            iris = model.Predict<Iris>(iris);
            Assert.AreEqual("Iris-setosa".Sanitize(), iris.Class);
        }

        [Test]
        public void xor_test_learner()
        {
            Sampling.SetSeedFromSystemTime();
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
            var model = Learner.Learn(xor, .75, 1000, generator).Model;
                        
            Matrix x = new[,]
                {{ -1, -1 }, // false, false -> +
                 { -1,  1 }, // false, true  -> -
                 {  1, -1 }, // true, false  -> -
                 {  1,  1 }};// false, false -> +

            Vector y = new[] { 0, 0, 0, 0 };

            for (int i = 0; i < x.Rows; i++)
                y[i] = model.Predict(x[i, VectorType.Row]);

        }
    }
}
