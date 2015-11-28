using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.SupervisedTests
{
    [TestFixture, Category("Supervised")]
    public class MultiClassLearnerTests
    {
        [Test]
        public void Test_Digits_MultiClass_Classification()
        {
            var digits = Data.Digit.GetTrainingDigits();

            var descriptor = Model.Descriptor.Create<Data.Digit>();

            var model = MultiClassLearner.Learn(new Supervised.Regression.LogisticRegressionGenerator()
                                                    {
                                                        Descriptor = descriptor,
                                                        Lambda = 0.1,
                                                        PolynomialFeatures = 0
                                                    },
                                                digits, 0.8, 0.5);

            int count = 0;
            var tests = Data.Digit.GetTestDigits();
            foreach (var test in tests)
            {
                var prediction = (int)model.Predict(test);
                count += (test.Label == prediction ? 1 : 0);
            }

            Assert.GreaterOrEqual(count / tests.Count(), model.Accuracy);
        }
    }
}
