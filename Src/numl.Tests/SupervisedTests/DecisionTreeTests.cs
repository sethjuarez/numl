using System;
using numl.Model;
using numl.Utils;
using System.Linq;
using Xunit;
using numl.Tests.Data;
using System.Collections.Generic;
using numl.Supervised.DecisionTree;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class DecisionTreeTests : BaseSupervised
    {
        [Fact]
        public void Tennis_Tests()
        {
            TennisPrediction(new DecisionTreeGenerator(50));
        }

        [Fact]
        public void House_Tests()
        {
            HousePrediction(new DecisionTreeGenerator(50));
        }

        [Fact]
        public void Iris_Tests()
        {
            IrisPrediction(new DecisionTreeGenerator(50));
        }

        [Fact]
        public void Tennis_Learner_Tests()
        {
            TennisLearnerPrediction(new DecisionTreeGenerator(50) { Hint = 0 });
        }

        [Fact]
        public void House_Learner_Tests()
        {
            HouseLearnerPrediction(new DecisionTreeGenerator(50) { Hint = 0 });
        }

        [Fact]
        public void Iris_Learner_Tests()
        {
            IrisLearnerPrediction(new DecisionTreeGenerator(50) { Hint = 0 });
        }

        [Fact]
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

		[Fact]
		public void Decision_Tree_Study_Category_Test()
		{
			var categoryA = Guid.NewGuid();
			var categoryB = Guid.NewGuid();

			var data = new[] {
				new { Study = 2.0, Category = categoryA, Passed = false},
				new { Study = 3.0, Category = categoryA, Passed = false},
				new { Study = 1.0, Category = categoryB, Passed = false},
				new { Study = 4.0, Category = categoryA, Passed = false},
				new { Study = 6.0, Category = categoryA, Passed = true},
				new { Study = 8.0, Category = categoryB, Passed = true},
				new { Study = 12.0, Category = categoryA, Passed = true},
				new { Study = 3.0, Category = categoryB, Passed = true}
			};

			var descriptor = Descriptor.New("Student")
				.With("Study").As(typeof(double))
				.With("Category").AsGuid()
				.Learn("Passed").As(typeof(bool));

			DecisionTreeGenerator generator = new DecisionTreeGenerator()
			{
				Descriptor = descriptor,
				NormalizeFeatures = true
			};

			var model = Learner.Learn(data, 0.8, 10, generator).Model;

			var test = model.PredictValue(new { Study = 7.0, Category = categoryA, Passed = false });

			Assert.Equal(true, test);
		}

		[Fact]
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

        [Fact]
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

            Assert.Equal(ArbitraryPrediction.PredictionLabel.Minimum, expectedMinimum);
            Assert.Equal(ArbitraryPrediction.PredictionLabel.Maximum, expectedMaximum);
        }
    }
}
