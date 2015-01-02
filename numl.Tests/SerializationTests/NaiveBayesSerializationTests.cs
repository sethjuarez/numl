using System;
using numl.Model;
using System.Linq;
using numl.Tests.Data;
using NUnit.Framework;
using numl.Supervised.NaiveBayes;
using System.Collections.Generic;

namespace numl.Tests.SerializationTests
{
    [TestFixture, Category("Serialization")]
    public class NaiveBayesSerializationTests : BaseSerialization
    {
        [Test]
        public void Tennis_Naive_Bayes_Save_And_Load_Test()
        {
            var data = Tennis.GetData();
            var description = Descriptor.Create<Tennis>();
            var generator = new NaiveBayesGenerator(2);
            var model = generator.Generate(description, data);

            Assert.IsInstanceOf<NaiveBayesModel>(model);

            Serialize(model);


            var lmodel = Deserialize<NaiveBayesModel>();
            Assert.IsInstanceOf<NaiveBayesModel>(lmodel);

            var mroot = ((NaiveBayesModel)model).Root;
            var lroot = ((NaiveBayesModel)lmodel).Root;


            Assert.IsTrue(mroot.Equals(lroot));
        }

        [Test]
        public void Iris_Naive_Bayes_Save_And_Load_Test()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();
            var generator = new NaiveBayesGenerator(2);
            var model = generator.Generate(description, data);

            Assert.IsInstanceOf<NaiveBayesModel>(model);

            Serialize(model);

            var lmodel = Deserialize<NaiveBayesModel>();
            Assert.IsInstanceOf<NaiveBayesModel>(lmodel);

            var mroot = ((NaiveBayesModel)model).Root;
            var lroot = ((NaiveBayesModel)lmodel).Root;
            Assert.IsTrue(mroot.Equals(lroot));
        }
    }
}
