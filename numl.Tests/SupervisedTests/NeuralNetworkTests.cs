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
    }
}
