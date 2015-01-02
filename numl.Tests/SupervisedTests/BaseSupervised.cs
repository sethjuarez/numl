using System;
using numl.Model;
using numl.Utils;
using System.Linq;
using numl.Supervised;
using numl.Tests.Data;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Math.Probability;

namespace numl.Tests.SupervisedTests
{
    [TestFixture]
    public class BaseSupervised
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            // just in case a generator uses
            // randomization, need to have a
            // good starting seed
            Sampling.SetSeedFromSystemTime();
        }

        public static IModel Prediction<T>(IGenerator generator, IEnumerable<T> data, T item, Func<T, bool> test)
            where T : class
        {
            var description = Descriptor.Create<T>();
            var model = generator.Generate(description, data);
            var prediction = model.Predict(item);
            Assert.IsTrue(test(prediction));
            return model;
        }

        public void HousePrediction(IGenerator generator)
        {
            // item to verify
            House h = new House
            {
                District = District.Rural,
                HouseType = HouseType.Detached,
                Income = Income.High,
                PreviousCustomer = false
            };

            Prediction<House>(
                generator,              // generator
                House.GetData(),        // Training data
                h,                      // test object
                p => p.Response         // should be true
            );
        }

        public void TennisPrediction(IGenerator generator)
        {
            Tennis t = new Tennis
            {
                Humidity = Humidity.Normal,
                Outlook = Outlook.Overcast,
                Temperature = Temperature.Cool,
                Windy = true
            };

            Prediction<Tennis>(
                generator,              // generator
                Tennis.GetData(),       // training data
                t,                      // test object
                p => p.Play             // should be true
            );
        }

        public void IrisPrediction(IGenerator generator)
        {
            // should be Iris-Setosa
            Iris iris = new Iris
            {
                PetalWidth = 0.5m,
                PetalLength = 2.3m,
                SepalLength = 2.1m,
                SepalWidth = 2.1m
            };


            Prediction<Iris>(
                generator,              // DT
                Iris.Load(),                                // training data
                iris,                                       // test object
                i => "Iris-setosa".Sanitize() == i.Class    // should be true
            );
        }

        public static void LearnerPrediction<T>(IGenerator generator, IEnumerable<T> data, T item, Func<T, bool> test)
            where T : class
        {
            var description = Descriptor.Create<T>();
            generator.Descriptor = description;
            var lmodel = Learner.Learn(data, .80, 1000, generator);
            var prediction = lmodel.Model.Predict(item);
            Assert.IsTrue(test(prediction));
        }

        public void HouseLearnerPrediction(IGenerator generator)
        {
            // item to verify
            House h = new House
            {
                District = District.Rural,
                HouseType = HouseType.Detached,
                Income = Income.High,
                PreviousCustomer = false
            };

            LearnerPrediction<House>(
                generator,              // generator
                House.GetData(),        // Training data
                h,                      // test object
                p => p.Response         // should be true
            );
        }

        public void TennisLearnerPrediction(IGenerator generator)
        {
            Tennis t = new Tennis
            {
                Humidity = Humidity.Normal,
                Outlook = Outlook.Overcast,
                Temperature = Temperature.Cool,
                Windy = true
            };

            LearnerPrediction<Tennis>(
                generator,              // generator
                Tennis.GetData(),       // training data
                t,                      // test object
                p => p.Play             // should be true
            );
        }

        public void IrisLearnerPrediction(IGenerator generator)
        {
            // should be Iris-Setosa
            Iris iris = new Iris
            {
                PetalWidth = 0.5m,
                PetalLength = 2.3m,
                SepalLength = 2.1m,
                SepalWidth = 2.1m
            };


            LearnerPrediction<Iris>(
               generator,
                Iris.Load(),
                iris,
                i => "Iris-setosa".Sanitize() == i.Class    // should be true
            );
        }
    }
}
