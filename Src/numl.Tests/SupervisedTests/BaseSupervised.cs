using System;
using System.Collections.Generic;
using System.Linq;
using numl.Math.LinearAlgebra;
using numl.Model;
using numl.Supervised;
using numl.Tests.Data;
using numl.Utils;
using Xunit;
using numl.Math.Probability;

namespace numl.Tests.SupervisedTests
{
    public class BaseSupervised
    {

        /// <summary>
        /// Compares the two gradients and returns the relative difference.
        /// </summary>
        /// <param name="numericalGrad"></param>
        /// <param name="grad"></param>
        /// <returns></returns>
        protected double CheckNumericalGradient(Vector numericalGrad, Vector grad)
        {
            return (numericalGrad - grad).Norm() / (numericalGrad + grad).Norm();
        }

        /// <summary>
        /// Computes the numerical gradient on the supplied theta.
        /// </summary>
        /// <param name="fnGradientSelector">Function to call for computing the gradient with a new theta value.</param>
        /// <param name="theta">Initial theta value.</param>
        /// <returns></returns>
        protected double ComputeNumericalGradient(Func<double, double> fnGradientSelector, double theta)
        {
            double e = 1e-4;
            double loss1 = fnGradientSelector(theta - e);
            double loss2 = fnGradientSelector(theta + e);

            return (loss2 - loss1) / (2 * e);
        }

        /// <summary>
        /// Compute the numerical gradient of a random test problem using the supplied cost function reference.
        /// </summary>
        /// <param name="fnCostSelector">A reference to a cost function accepting a Vector.</param>
        /// <param name="theta">Theta.</param>
        /// <returns></returns>
        protected Vector ComputeNumericalGradient(Func<Vector, double> fnCostSelector, Vector theta)
        {
            Vector numericalGrad = Vector.Zeros(theta.Length);
            Vector perturb = Vector.Zeros(theta.Length);

            double e = 1e-4;

            for (int p = 0; p < numericalGrad.Length; p++)
            {
                perturb[p] = e;

                double loss1 = fnCostSelector(theta - perturb);
                double loss2 = fnCostSelector(theta + perturb);

                numericalGrad[p] = (loss2 - loss1) / (2 * e);
                perturb[p] = 0;
            }

            return numericalGrad;
        }

        public static void HouseLearnerPrediction(IGenerator generator)
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
            var lmodel = Learner.Learn(data, .80, 10, generator);
            var prediction = lmodel.Model.Predict(item);
            Assert.True(test(prediction));
        }

        public static IModel Prediction<T>(IGenerator generator, IEnumerable<T> data, T item, Func<T, bool> test)
            where T : class
        {
            generator.Descriptor = Descriptor.Create<T>();
            // I can't assume it will always do a good job
            // especially because of overfitting
            // var model = generator.Generate(description, data);
            var model = Learner.Learn(data, .7, 10, generator);
            Console.WriteLine($"{model}");
            var prediction = model.Model.Predict(item);
            Assert.True(test(prediction));
            return model.Model;
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
    }
}
