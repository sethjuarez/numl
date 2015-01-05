using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using numl.Math.Functions;

namespace numl.Functions.CostFunctions
{
    /// <summary>
    /// A Linear Cost Function.
    /// </summary>
    public class LinearCostFunction : ICostFunction
    {
                /// <summary>
        /// Compute the error cost of the given Theta parameter for the training and label sets
        /// </summary>
        /// <param name="theta">Learning Theta parameters</param>
        /// <param name="X">Training set</param>
        /// <param name="y">Training labels</param>
        /// <param name="lambda">Regularization constant</param>
        /// <param name="regularizer">Regularization term function.</param>
        /// <returns></returns>
        public double ComputeCost(Vector theta, Matrix X, Vector y, double lambda, IRegularizer regularizer)
        {
            int m = X.Rows;

            double j = 0.0;

            Vector s = (X * theta).ToVector();

            j = 1.0 / (2.0 * m) * ((s - y) ^ 2.0).Sum();

            if (lambda != 0)
            {
                j = regularizer.Regularize(j, theta, m, lambda);
            }

            return j;
        }

        /// <summary>
        /// Compute the error cost of the given Theta parameter for the training and label sets
        /// </summary>
        /// <param name="theta">Learning Theta parameters</param>
        /// <param name="X">Training set</param>
        /// <param name="y">Training labels</param>
        /// <param name="lambda">Regularisation constant</param>
        /// <param name="regularizer">Regularization term function.</param>
        /// <returns></returns>
        public Vector ComputeGradient(Vector theta, Matrix X, Vector y, double lambda, IRegularizer regularizer)
        {
            int m = X.Rows;
            Vector gradient = Vector.Zeros(theta.Length);

            Vector s = (X * theta).ToVector();

            for (int i = 0; i < theta.Length; i++)
            {
                gradient[i] = 1.0 / m * ((s - y) * X[i, VectorType.Col]).Sum();
            }

            if (lambda != 0)
            {
                gradient = regularizer.Regularize(theta, gradient, m, lambda);
            }

            return gradient;
        }
    }
}
