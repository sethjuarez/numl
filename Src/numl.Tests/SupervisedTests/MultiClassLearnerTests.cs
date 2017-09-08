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
                                                digits, 0.8, 0.5);

            double count = 0;
            var tests = Data.Digit.GetTestDigits();
            foreach (var test in tests)
            {
                var prediction = (int)model.Predict(test);
                count += (test.Label == prediction ? 1.0 : 0.0);
            }

            double accuracy = count / (double)tests.Count();

            Console.WriteLine($"LR Model Score: { model.Score }");

            Console.WriteLine($"Digit Test:\nOverall Accuracy => { accuracy }");

            Assert.True(accuracy >= model.Accuracy, $"{accuracy} >= {model.Accuracy}");
        }
    }
}
