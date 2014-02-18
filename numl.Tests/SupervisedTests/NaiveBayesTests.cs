using System;
using numl.Model;
using System.Linq;
using numl.Supervised;
using numl.Tests.Data;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Supervised.NaiveBayes;
using numl.Data;
using numl.Utils;
using numl.Math.LinearAlgebra;

namespace numl.Tests.SupervisedTests
{
    [TestFixture]
    public class NaiveBayesTests
    {
        [Test]
        public void Main_Naive_Bayes_Test()
        {
            var data = Tennis.GetData();
            var description = Descriptor.Create<Tennis>();
            var generator = new NaiveBayesGenerator(2);
            var model = generator.Generate(description, data);

            Tennis t = new Tennis
            {
                Humidity = Humidity.Normal,
                Outlook = Outlook.Overcast,
                Temperature = Temperature.Cool,
                Windy = true
            };

            model.Predict<Tennis>(t);
            Assert.IsTrue(t.Play);
        }

        [Test]
        public void Iris_DT_and_Prediction()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();            
            var generator = new NaiveBayesGenerator(2);
            var model = generator.Generate(description, data);

            // should be Iris-Setosa
            Iris iris = new Iris
            {
                SepalLength = 2.1m,
                SepalWidth = 2.2m,
                PetalWidth = 0.5m,
                PetalLength = 2.3m,
            };

            iris = model.Predict<Iris>(iris);
            Assert.AreEqual("Iris-setosa".Sanitize(), iris.Class);
        }

        [Test]
        public void Iris_FluentDescriptor_DT_and_Prediction()
        {
            var data = Iris.Load();
            var description = Descriptor.For<Iris>()
                .Learn(x => x.Class)
                .With(x => x.PetalLength)
                .With(x => x.PetalWidth)
                .With(x => x.SepalLength)
                .With(x => x.SepalWidth);
            var generator = new NaiveBayesGenerator(2);
            var model = generator.Generate(description, data);

            // should be Iris-Setosa
            Iris iris = new Iris
            {
                SepalLength = 2.1m,
                SepalWidth = 2.2m,
                PetalWidth = 0.5m,
                PetalLength = 2.3m,
            };

            iris = model.Predict<Iris>(iris);
            Assert.AreEqual("Iris-setosa".Sanitize(), iris.Class);
        }
    }
}
