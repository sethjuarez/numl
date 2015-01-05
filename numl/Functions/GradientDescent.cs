using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using numl.Math.Functions;

namespace numl.Functions
{
    /// <summary>
    /// Gradient Descent
    /// </summary>
    public static class GradientDescent
    {
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
        public static Tuple<double, Vector> Run(Vector theta, Matrix x, Vector y, int maxIterations, double learningRateAlpha, 
            ICostFunction costFunction, double lambda, IRegularizer regularizer)
        {
            Vector bestTheta = theta.Copy();
            double bestCost = double.PositiveInfinity;

            double currentCost = 0;
            Vector currentGradient = theta.Copy();

            for (int i = 0; i <= maxIterations; i++)
            {
                currentCost = costFunction.ComputeCost(bestTheta, x, y, lambda, regularizer);
                currentGradient = costFunction.ComputeGradient(bestTheta, x, y, lambda, regularizer);

                if (currentCost < bestCost)
                {
                    bestTheta = bestTheta - learningRateAlpha * currentGradient;
                    bestCost = currentCost;
                }
                else
                {
                    learningRateAlpha = learningRateAlpha * 0.99;
                }
            }

            return new Tuple<double, Vector>(bestCost, bestTheta);
        }
    }
}
