using System;
using numl.Model;
using System.Linq;
using numl.Supervised;
using numl.Tests.Data;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Supervised.NaiveBayes;

namespace numl.Tests.SupervisedTests
{
    [TestFixture]
    public class NaiveBayesTests
    {
        [Test]
        public void Main_Naive_Bayes_Test()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();
            var generator = new NaiveBayesGenerator();
            var model = generator.Generate(description, data);

            // should be Iris-Setosa
            Iris iris = new Iris
            {
                PetalWidth = 0.5m,
                PetalLength = 2.3m,
                SepalLength = 2.1m,
                SepalWidth = 2.1m
            };

            //iris = model.Predict<Iris>(iris);
            //Assert.AreEqual("Iris-setosa".Sanitize(), iris.Class);
        }
    }
}
