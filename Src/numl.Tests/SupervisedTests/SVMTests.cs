using System;
using numl.Model;
using System.Linq;
using NUnit.Framework;
using numl.Math.Kernels;
using System.Collections.Generic;

namespace numl.Tests.SupervisedTests
{
    [TestFixture, Category("Supervised")]
    public class SVMTests : BaseSupervised
    {
        [Test]
        public void Test_SVM_Classification()
        {
            var students = Data.Student.GetData();

            var descriptor = Descriptor.Create<Data.Student>();

            var model = MultiClassLearner.Learn(new numl.Supervised.SVM.SVMGenerator()
            {
                Descriptor = descriptor,
                KernelFunction = new LogisticKernel(),
                C = 10,
                Bias = 1,
                MaxIterations = 100
            }, students, 0.9);

            // TODO: Need some work to make this better
            // Assert.GreaterOrEqual(model.Accuracy, 0.65d);
            Console.WriteLine($"SVM Model Accuracy: { model.Accuracy }");
        }
    }
}
