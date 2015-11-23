using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Supervised.Regression;
using numl.Math.LinearAlgebra;
using numl.Optimization.Functions;
using numl.Utils;

namespace numl.Tests.SupervisedTests
{
    [TestFixture, Category("Supervised")]
    public class SVMTests : BaseSupervised
    {
        [Test]
        public void Test_SVM_Classification()
        {
            var students = Data.Student.GetData();

            var descriptor = Model.Descriptor.Create<Data.Student>();

            var model = MultiClassLearner.Learn(new SVMGenerator()
            {
                Descriptor = descriptor,
                KernelFunction = new Math.Kernels.LinearKernel()
            }, students, 0.9);

            
            Assert.GreaterOrEqual(model.Accuracy, 0.55d);
        }
    }
}
