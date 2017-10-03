using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Utils;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class MultiClassLearnerTests
    {
        [Fact]
        public void Test_Digits_MultiClass_Classification()
        {
            var digits = Data.Digit.GetTrainingDigits();

            var descriptor = Model.Descriptor.Create<Data.Digit>();

            var model = MultiClassLearner.Learn(new Supervised.Regression.LogisticRegressionGenerator()
                                                    {
                                                        Descriptor = descriptor,
                                                        Lambda = 0.1,
                                                        PolynomialFeatures = 0,
                                                        LearningRate = 0.1
                                                    },
                                                digits, 1.0, 0.5);

            double traincount = 0;
            foreach (var train in digits)
            {
                var prediction = (int) model.Predict(train);
                traincount += (train.Label == prediction ? 1.0 : 0.0);
            }

            double trainaccuracy = traincount / (double) digits.Count();

            double testcount = 0;
            var tests = Data.Digit.GetTestDigits();
            foreach (var test in tests)
            {
                var prediction = (int)model.Predict(test);
                testcount += (test.Label == prediction ? 1.0 : 0.0);
            }

            double testaccuracy = testcount / (double)tests.Count();

            Console.WriteLine($"LR Model Score: { model.Score }");

            Assert.True(testaccuracy >= 0.9, $"{testaccuracy} >= {0.9}");
            Assert.True(trainaccuracy >= 0.95, $"{trainaccuracy} >= {0.95}");
        }
    }
}
