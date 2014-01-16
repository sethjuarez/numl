using numl.Data;
using numl.Model;
using numl.Supervised.NaiveBayes;
using numl.Tests.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Utils;

namespace numl.Tests.SerializationTests
{
    [TestFixture]
    public class NaiveBayesSerializationTests : BaseSerialization
    {
        [Test]
        public void Tennis_Naive_Bayes_Save_And_Load_Test()
        {
            var data = Tennis.GetData();
            var description = Descriptor.Create<Tennis>();
            var generator = new NaiveBayesGenerator(2);
            var model = generator.Generate(description, data);
            Serialize(model);


            var lmodel = Deserialize<NaiveBayesModel>();
        }

        [Test]
        public void Iris_Naive_Bayes_Save_And_Load_Test()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();
            var generator = new NaiveBayesGenerator(2);
            var model = generator.Generate(description, data);

            Serialize(model);

            var lmodel = Deserialize<NaiveBayesModel>();

        }
    }
}
