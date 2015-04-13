using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using numl.Math.Functions;
using numl.Utils;

namespace numl.Functions
{
    /// <summary>
    /// Gradient Descent
    /// </summary>
    public static class GradientDescent
    {
        private const double MaxRetryPercentage = 0.02; 
        /// <summary>
        /// Performs gradient descent to optomise theta parameters.
        /// </summary>
        /// <param name="theta">Initial Theta (Zeros)</param>
        /// <param name="x">Training set</param>
        /// <param name="y">Training labels</param>
        /// <param name="maxIterations">Maximum number of iterations to run gradient descent</param>
        /// <param name="learningRateAlpha">The learning rate (Alpha)</param>
        /// <param name="costFunction">Cost function to use for gradient descent</param>
        /// <param name="lambda">The regularization constant to apply</param>
        /// <param name="regularizer">The regularization function to apply</param>
        /// <returns></returns>
        public static Tuple<double, Vector> Run(Vector theta, Matrix x, Vector y, int maxIterations, double alpha, 
            ICostFunction costFunction, double lambda, IRegularizer regularizer)
        {
            Vector bestTheta = theta.Copy();
            double bestCost = double.PositiveInfinity;

            double currentCost = 0;
            Vector currentGradient = theta.Copy();
            Vector currentTheta = bestTheta.Copy();

            double tempAlpha = alpha;
            int resetCount = 0;
            double[] costHistory = new double[maxIterations];

            for (int i = 0; i < maxIterations; i++)
            {
                costHistory[i] = currentCost;

                currentTheta = currentTheta - tempAlpha * currentGradient;
                currentCost = costFunction.ComputeCost(currentTheta, x, y, lambda, regularizer);
                currentGradient = costFunction.ComputeGradient(currentTheta, x, y, lambda, regularizer);

                if (currentCost < bestCost)
                {
                    double t = tempAlpha + (tempAlpha * ((currentCost / bestCost) * alpha));
                    if (t > tempAlpha)
                    {
                        resetCount--;

                        if (resetCount < (maxIterations * GradientDescent.MaxRetryPercentage))
                        {
                            var testTheta = currentTheta - t * currentGradient;
                            var testCost = costFunction.ComputeCost(testTheta, x, y, lambda, regularizer);

                            if (testCost < currentCost)
                            {
                                tempAlpha = t;
                                bestCost = currentCost = testCost;
                                currentGradient = costFunction.ComputeGradient(testTheta, x, y, lambda, regularizer);
                                bestTheta = currentTheta = testTheta;

                                continue;
                            }
                        }
                    }

                    bestCost = currentCost;
                    bestTheta = currentTheta;
                }
                else
                {
                    resetCount++;

                    if (tempAlpha > alpha)
                    {
                        tempAlpha = alpha;
                    }
                    else
                    {
                        tempAlpha = tempAlpha * 0.9;
                    }

                    if (resetCount > (maxIterations * GradientDescent.MaxRetryPercentage))
                    {
                        resetCount = 0;
                        currentTheta = bestTheta;
                        currentCost = bestCost;
                    }
                }
            }

            return new Tuple<double, Vector>(bestCost, bestTheta);
        }
    }
}
