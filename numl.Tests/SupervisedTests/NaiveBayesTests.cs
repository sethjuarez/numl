using System;
using numl.Model;
using System.Linq;
using numl.Supervised;
using numl.Tests.Data;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Supervised.NaiveBayes;
using numl.Data;

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
        }
    }
}
