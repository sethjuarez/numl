using System;
using numl.Model;
using System.Linq;
using numl.Tests.Data;
using NUnit.Framework;
using numl.Supervised.NaiveBayes;
using System.Collections.Generic;
using numl.Math.Kernels;
using numl.Supervised.Perceptron;

namespace numl.Tests.SerializationTests
{
    [TestFixture, Category("Serialization")]
    public class KernelPerceptronSerializationTests : BaseSerialization
    {
        [Test]
        public void Basic_RBF_Kernel_Serialization_Test()
        {
            var data = new List<object>();
            for (var i = 0; i < 100; i++)
            {
                data.Add(new { Prop1 = i, Prop2 = i + 1, Prop3 = i + 2, Result = i % 2 == 0 });
            }
            var descriptor = Descriptor.Create(typeof(object))
                                        .With("Prop1").As(typeof(int))
                                        .With("Prop2").As(typeof(int))
                                        .With("Prop3").As(typeof(int))
                                        .Learn("Result").As(typeof(bool));

            var kernel = new RBFKernel(3);
            var generator = new KernelPerceptronGenerator(kernel) { Descriptor = descriptor };

            var model = generator.Generate(descriptor, data) as KernelPerceptronModel;
            
            Serialize(model);

            var lmodel = Deserialize<KernelPerceptronModel>();
            //Assert.AreEqual(model.Root, lmodel.Root);

            //learningModel.Model.Save("model.mdl");
            //// THIS FAILS ----
            //learningModel.Model.Load("model.mdl");
        }

        [Test]
        public void Iris_Naive_Bayes_Save_And_Load_Test()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();
            var generator = new NaiveBayesGenerator(2);
            var model = generator.Generate(description, data) as NaiveBayesModel;

            Serialize(model);

            var lmodel = Deserialize<NaiveBayesModel>();
            Assert.AreEqual(model.Root, lmodel.Root);
        }
    }
}
