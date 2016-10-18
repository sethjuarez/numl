using System;
using numl.Model;
using System.Linq;
using Xunit;
using numl.Math.LinearAlgebra;
using numl.Math.Functions.Cost;
using System.Collections.Generic;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class CofiRecommenderTests : BaseSupervised
    {
        #region Test Data

        /// <summary>
        /// movies x user features
        /// </summary>
        private Matrix X = new[,] {
               { 1.048686,  -0.400232,   1.194119,   0.371128,   0.407607 },
               { 0.780851,  -0.385626,   0.521198,   0.227355,   0.570109 },
               { 0.641509,  -0.547854,  -0.083796,  -0.598519,  -0.017694 },
               { 0.453618,  -0.800218,   0.680481,  -0.081743,   0.136601 },
               { 0.937538,   0.106090,   0.361953,   0.086646,   0.287505 },
               { 0.072619,  -0.508257,   0.052991,  -0.083351,   0.328765 },
               { 0.418129,  -0.560677,   0.481836,  -0.361659,   0.268461 }
            };

        /// <summary>
        /// movies x ratings
        /// </summary>
        private Matrix Y = new[,] {
               { 5,   4,   0,   0,   4,   4 },
               { 3,   0,   0,   0,   3,   0 },
               { 4,   0,   0,   0,   0,   0 },
               { 3,   0,   0,   0,   0,   0 },
               { 3,   0,   0,   0,   0,   0 },
               { 5,   0,   0,   0,   0,   0 },
               { 4,   0,   0,   0,   0,   2 }
            };


        /// <summary>
        /// ratings x movie features
        /// </summary>
        private Matrix Theta = new[,] {
               {  0.285444,  -1.684265,   0.262939,  -0.287317,   0.585725 },
               {  0.505013,  -0.454648,   0.317462,  -0.115087,   0.567704 },
               { -0.431917,  -0.478804,   0.846711,  -0.011731,  -0.138578 },
               {  0.728598,  -0.271894,   0.326844,  -0.250724,  -0.471017 },
               {  0.052119,  -1.380697,   0.637941,  -0.231980,   0.246337 },
               { -0.091194,  -0.600370,   0.697439,  -0.156222,   1.227546 }
            };

        #endregion

        private void CheckCofiGradient(double lambda)
        {
            Matrix X_s = Matrix.Rand(4, 3);
            Matrix T_s = Matrix.Rand(5, 3);

            Matrix Y_s = (X_s * T_s.T);
            Matrix R_s = Matrix.Zeros(Y_s.Rows, Y_s.Cols);

            Y_s[Matrix.Rand(Y_s.Rows, Y_s.Cols) > 0.5] = 0.0;
            R_s[Y_s == 0] = 1.0;

            Matrix X_ts = Matrix.NormRand(X_s.Rows, X_s.Cols);
            Matrix T_ts = Matrix.NormRand(T_s.Rows, T_s.Cols);

            ICostFunction costFunction = new CofiCostFunction() { R = R_s, X = X_ts, Y = Y_s.Unshape(), Lambda = 0, Regularizer = null, CollaborativeFeatures = 3 };
            costFunction.Initialize();

            Vector grad = costFunction.ComputeGradient(Vector.Combine(X_ts.Unshape(), T_ts.Unshape()));

            Vector numericalGrad = this.ComputeNumericalGradient(
                f => costFunction.ComputeCost(f),
                Vector.Combine(X_ts.Unshape(), T_ts.Unshape()));

            Assert.True(this.CheckNumericalGradient(numericalGrad, grad) < 0.0000000001);
        }

        [Fact]
        public void Test_Cofi_CostFunction()
        {
            Matrix rMat = Y.ToBinary(i => i > 0d);

            ICostFunction costFunction = new CofiCostFunction() { R = rMat, X = X, Y = Y.Unshape(), Lambda = 0, Regularizer = null, CollaborativeFeatures = X.Cols };
            costFunction.Initialize();
            double cost = costFunction.ComputeCost(Vector.Combine(X.Unshape(), Theta.Unshape()));
            Vector grad = costFunction.ComputeGradient(Vector.Combine(X.Unshape(), Theta.Unshape()));

            Almost.Equal(39.796d, System.Math.Round(cost, 3), 0.001);

            this.CheckCofiGradient(0);
        }

        [Fact]
        public void Test_Cofi_CostFunction_Regularized()
        {
            Matrix rMat = Y.ToBinary(i => i > 0d);

            ICostFunction costFunction = new CofiCostFunction() { R = rMat, X = X, Y = Y.Unshape(), Lambda = 1.5, Regularizer = null, CollaborativeFeatures = X.Cols };
            costFunction.Initialize();
            double cost = costFunction.ComputeCost(Vector.Combine(X.Unshape(), Theta.Unshape()));
            Vector grad = costFunction.ComputeGradient(Vector.Combine(X.Unshape(), Theta.Unshape()));

            Almost.Equal(55.172, System.Math.Round(cost, 3), 0.0011);

            this.CheckCofiGradient(1.5);
        }

        [Fact]
        public void Test_Cofi_Recommender()
        {
            var movies = new[] {
                new { ID = 1,   Name = "From Dawn til Dusk",  Ratings = new int[] { 4, 0, 3, 4, 4, 5, 4 } },
                new { ID = 2,   Name = "The Hoarder",         Ratings = new int[] { 0, 1, 5, 0, 0, 1, 0 } },
                new { ID = 3,   Name = "Cowboys and Cows",    Ratings = new int[] { 2, 0, 3, 4, 4, 5, 4 } },
                new { ID = 4,   Name = "Small Fry Town",      Ratings = new int[] { 0, 1, 2, 0, 1, 4, 0 } },
                new { ID = 5,   Name = "The White Knight",    Ratings = new int[] { 4, 0, 3, 0, 4, 5, 3 } },
                new { ID = 6,   Name = "Love Me Tender",      Ratings = new int[] { 0, 1, 1, 3, 0, 0, 0 } },
                new { ID = 7,   Name = "Total Groove",        Ratings = new int[] { 0, 1, 5, 3, 0, 1, 0 } },
                new { ID = 8,   Name = "Action Chase",        Ratings = new int[] { 0, 2, 3, 0, 0, 5, 4 } },
                new { ID = 9,   Name = "Underneath",          Ratings = new int[] { 0, 4, 0, 0, 0, 5, 0 } },
                new { ID = 10,  Name = "Time Reinvented",     Ratings = new int[] { 0, 0, 4, 3, 0, 3, 2 } },
            };

            // should predict (top 5): From Dawn til Dusk, The White Knight, Underneath, Cowboys and Cows, Action Chase

            var descriptor = Descriptor.New("MOVIES")
                                        .With("Ratings").AsEnumerable(7)
                                        .Learn("ID").As(typeof(int));

            var generator = new Recommendation.CofiRecommenderGenerator()
            {
                Ratings = new Math.Range(1, 5),
                CollaborativeFeatures = 7,
                Descriptor = descriptor,
                LearningRate = 0.1,
                Lambda = 1.0,
            };

            var model = Learner.Learn(movies, 1.0, 1, generator);
            // get predictions for the movies of the first user
            var predictions = ((Recommendation.CofiRecommenderModel)model.Model).Predict(0);

            Assert.Equal(1d, predictions[0]);

            // due to random initialisation one is favoured over the other at certain times
            Assert.True(5d == predictions[1] || 9d == predictions[1]);
            Assert.True(5d == predictions[2] || 9d == predictions[2]);
            
            Assert.Equal(3d, predictions[3]);
            Assert.Equal(8d, predictions[4]);
        }
    }
}
