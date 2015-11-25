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
                KernelFunction = new LinearKernel()
            }, students, 0.9);


            Assert.GreaterOrEqual(model.Accuracy, 0.55d);
        }
    }
}
