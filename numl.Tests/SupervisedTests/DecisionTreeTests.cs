using System;
using numl.Model;
using numl.Utils;
using System.Linq;
using NUnit.Framework;
using numl.Tests.Data;
using System.Collections.Generic;
using numl.Supervised.DecisionTree;

namespace numl.Tests.SupervisedTests
{
    [TestFixture, Category("Supervised")]
    public class DecisionTreeTests : BaseSupervised
    {
        [Test]
        public void Tennis_Tests()
        {
            TennisPrediction(new DecisionTreeGenerator(50));
        }

        [Test]
        public void House_Tests()
        {
            HousePrediction(new DecisionTreeGenerator(50));
        }

        [Test]
        public void Iris_Tests()
        {
            IrisPrediction(new DecisionTreeGenerator(50));
        }

        [Test]
        public void Tennis_Learner_Tests()
        {
            TennisLearnerPrediction(new DecisionTreeGenerator(50) { Hint = 0 });
        }

        [Test]
        public void House_Learner_Tests()
        {
            HouseLearnerPrediction(new DecisionTreeGenerator(50) { Hint = 0 });
        }

        [Test]
        public void Iris_Learner_Tests()
        {
            IrisLearnerPrediction(new DecisionTreeGenerator(50) { Hint = 0 });
        }

        [Test]
        public void ValueObject_Test_With_Yield_Enumerator()
        {
            var o = new ValueObject() { V1 = 1, V2 = 60 };

            Prediction<ValueObject>(
                new DecisionTreeGenerator(),
                ValueObject.GetData(),
                o,
                vo => "l".Sanitize() == vo.R
            );
        }

        [Test]
        public void ArbitraryPrediction_Test_With_Enum_Label()
        {
            ArbitraryPrediction minimumPredictionValue = new ArbitraryPrediction
            {
                FirstTestFeature = 1.0m,
                SecondTestFeature = 10.0m,
                ThirdTestFeature = 1.2m
            };

            Prediction<ArbitraryPrediction>(
                new DecisionTreeGenerator(50),
                ArbitraryPrediction.GetData(),
                minimumPredictionValue,
                ap => ap.OutcomeLabel == ArbitraryPrediction.PredictionLabel.Minimum
            );

            ArbitraryPrediction maximumPredictionValue = new ArbitraryPrediction
            {
                FirstTestFeature = 1.0m,
                SecondTestFeature = 55.0m,
                ThirdTestFeature = 1.2m
            };

            Prediction<ArbitraryPrediction>(
                new DecisionTreeGenerator(50),
                ArbitraryPrediction.GetData(),
                maximumPredictionValue,
                ap => ap.OutcomeLabel == ArbitraryPrediction.PredictionLabel.Maximum
            );
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void ArbitraryPrediction_Test_With_Feature_Value_Greater_Than_Trained_Instances()
        {

            ArbitraryPrediction predictionValue = new ArbitraryPrediction
            {
                FirstTestFeature = 1.0m,
                //This value is greater than any of the trained instances
                SecondTestFeature = 106.0m,
                ThirdTestFeature = 1.2m
            };

            Prediction<ArbitraryPrediction>(
                new DecisionTreeGenerator(50),
                ArbitraryPrediction.GetData(),
                predictionValue,
                ap => ap.OutcomeLabel == ArbitraryPrediction.PredictionLabel.Maximum
            );

        }

        [Test]
        public void ArbitraryPrediction_Test_With_Named_Iterator()
        {
            var data = ArbitraryPrediction.GetDataUsingNamedIterator();
            var description = Descriptor.Create<ArbitraryPrediction>();
            var generator = new DecisionTreeGenerator(50);
            var model = generator.Generate(description, data);

            ArbitraryPrediction minimumPredictionValue = new ArbitraryPrediction
            {
                FirstTestFeature = 1.0m,
                SecondTestFeature = 10.0m,
                ThirdTestFeature = 1.2m
            };

            ArbitraryPrediction maximumPredictionValue = new ArbitraryPrediction
            {
                FirstTestFeature = 1.0m,
                SecondTestFeature = 57.0m,
                ThirdTestFeature = 1.2m
            };

            var expectedMinimum = model.Predict<ArbitraryPrediction>(minimumPredictionValue).OutcomeLabel;
            var expectedMaximum = model.Predict<ArbitraryPrediction>(maximumPredictionValue).OutcomeLabel;

            Assert.AreEqual(ArbitraryPrediction.PredictionLabel.Minimum, expectedMinimum);
            Assert.AreEqual(ArbitraryPrediction.PredictionLabel.Maximum, expectedMaximum);
        }
    }
}
