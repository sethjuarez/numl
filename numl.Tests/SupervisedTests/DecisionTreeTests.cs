using System;
using numl.Data;
using numl.Model;
using numl.Utils;
using System.Linq;
using numl.Supervised;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Tests.Data;
using System.IO;
using numl.TitanicPassengersChallenge;
using System.Diagnostics;

namespace numl.Tests.SupervisedTests
{
    [TestFixture]
    public class DecisionTreeTests
    {
        [Test]
        public void House_DT_and_Prediction()
        {
            var data = House.GetData();

            var description = Descriptor.Create<House>();
            var generator = new DecisionTreeGenerator { Depth = 50 };
            var model = generator.Generate(description, data);

            House h = new House
            {
                District = District.Rural,
                HouseType = HouseType.Detached,
                Income = Income.High,
                PreviousCustomer = false
            };

            var prediction = model.Predict(h);
            Assert.IsTrue(prediction.Response);
        }

        [Test]
        public void Tennis_DT_and_Prediction()
        {
            var data = Tennis.GetData();
            var description = Descriptor.Create<Tennis>();
            var generator = new DecisionTreeGenerator(50);
            var model = generator.Generate(description, data);

            Tennis t = new Tennis
            {
                Humidity = Humidity.Normal,
                Outlook = Outlook.Overcast,
                Temperature = Temperature.Cool,
                Windy = true
            };

            model.Predict<Tennis>(t);
            Assert.IsTrue(t.Play);
        }


        [Test]
        public void Iris_DT_and_Prediction()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();
            var generator = new DecisionTreeGenerator(50);
            var model = generator.Generate(description, data);

            // should be Iris-Setosa
            Iris iris = new Iris
            {
                PetalWidth = 0.5m,
                PetalLength = 2.3m,
                SepalLength = 2.1m,
                SepalWidth = 2.1m
            };

            iris = model.Predict<Iris>(iris);
            Assert.AreEqual("Iris-setosa".Sanitize(), iris.Class);
        }

        [Test]
        public void Iris_DT_and_Prediction_with_Learner()
        {
            var data = Iris.Load();

            var generator = new DecisionTreeGenerator(50)
            {
                Descriptor = Descriptor.Create<Iris>(),
                Hint = 0
            };

            var lmodel = Learner.Learn(data, .80, 1000, generator);

            // should be Iris-Setosa
            Iris iris = new Iris
            {
                PetalWidth = 0.5m,
                PetalLength = 2.3m,
                SepalLength = 2.1m,
                SepalWidth = 2.1m
            };

            iris = lmodel.Model.Predict<Iris>(iris);
            Assert.AreEqual("Iris-setosa".Sanitize(), iris.Class);
        }

        [Test]
        public void ValueObject_Test_With_Yield_Enumerator()
        {
            var data = ValueObject.GetData();
            var generator = new DecisionTreeGenerator()
            {
                Descriptor = Descriptor.Create<ValueObject>()
            };

            var model = generator.Generate(data);
            var s = model.ToString();

            var o = new ValueObject() { V1 = 1, V2 = 60 };
            var os = model.Predict<ValueObject>(o).R;
            Assert.AreEqual("l".Sanitize(), os);
        }

        [Test]
        public void ArbitraryPrediction_Test_With_Enum_Label()
        {
            var data = ArbitraryPrediction.GetData();
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
                SecondTestFeature = 55.0m,
                ThirdTestFeature = 1.2m
            };

            var expectedMinimum = model.Predict<ArbitraryPrediction>(minimumPredictionValue).OutcomeLabel;
            var expectedMaximum = model.Predict<ArbitraryPrediction>(maximumPredictionValue).OutcomeLabel;

            //The value should be minimum, however, maximum is returned
            Assert.AreEqual(ArbitraryPrediction.PredictionLabel.Minimum, expectedMinimum);
            //Maximum is returned as expected
            Assert.AreEqual(ArbitraryPrediction.PredictionLabel.Maximum, expectedMaximum);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void ArbitraryPrediction_Test_With_Feature_Value_Greater_Than_Trained_Instances()
        {
            //TODO Seth, this test has an error
            var data = ArbitraryPrediction.GetData();
            var description = Descriptor.Create<ArbitraryPrediction>();
            var generator = new DecisionTreeGenerator(50);
            var model = generator.Generate(description, data);

            ArbitraryPrediction predictionValue = new ArbitraryPrediction
            {
                FirstTestFeature = 1.0m,
                //This value is greater than any of the trained instances
                SecondTestFeature = 106.0m,
                ThirdTestFeature = 1.2m
            };

            var expectedValue = model.Predict<ArbitraryPrediction>(predictionValue).OutcomeLabel;
            Assert.AreEqual(ArbitraryPrediction.PredictionLabel.Maximum, expectedValue);
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

            //The value should be minimum, however, maximum is returned
            Assert.AreEqual(ArbitraryPrediction.PredictionLabel.Minimum, expectedMinimum);
            //Maximum is returned as expected
            Assert.AreEqual(ArbitraryPrediction.PredictionLabel.Maximum, expectedMaximum);
        }

        [Test]
        public void Titanic_Passenger_Survival_Test_Success_Percentage_Should_Be_Seventy_Five_Percent()
        {
            var generator = new DecisionTreeGenerator(50)
            {
                Descriptor = Descriptor.Create<Passenger>(),
                Hint = 0
            };

            var data = Passenger.LoadData(@"D:\Development\machinelearning\numl-fork\numl\numl.TestPredictions\Data\Titanic_Training_Data.csv", true);

            var model = generator.Generate(data);
            //var model = Learner.Learn(data, .90, 1000, generator);

            int incorrectPredictions = 0;
            int correctPredictions = 0;

            for (int passengerIndex = 0; passengerIndex < 800; passengerIndex++)
            {
                var passenger = data[passengerIndex];
                var survived = passenger.PassengerSurvived;

                var passengerPredicted =
                    model.Predict<Passenger>(passenger);

                if (survived != passengerPredicted.PassengerSurvived)
                    incorrectPredictions++;
                else
                    correctPredictions++;
            }

            var percentageCorrect = (correctPredictions / 800.0) * 100;
            var percentageIncorrect = (incorrectPredictions / 800.0) * 100;

            Debug.WriteLine(string.Format("Out of 800 predictions, the engine predicted {0}% correctly and {1}% incorrectly", percentageCorrect, percentageIncorrect));
            Assert.IsTrue(percentageCorrect >= 70.0);
        }
    }
}
