using System;
using numl.Model;
using System.Linq;
using numl.Tests.Data;
using Xunit;
using numl.Supervised.NaiveBayes;
using System.Collections.Generic;
using numl.Math.Kernels;
using numl.Supervised.Perceptron;

namespace numl.Tests.SerializationTests.ModelSerialization
{
    [Trait("Category", "Serialization")]
    public class KernelPerceptronSerializationTests : BaseSerialization
    {
        [Fact]
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
        }
    }
}
