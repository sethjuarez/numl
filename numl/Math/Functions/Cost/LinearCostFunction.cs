using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Functions.Cost
{
    /// <summary>
    /// A Linear Cost Function.
    /// </summary>
    public class LinearCostFunction : CostFunction
    {
        /// <summary>
        /// Compute the error cost of the given Theta parameter for the training and label sets
        /// </summary>
        /// <param name="theta">Learning Theta parameters</param>
        /// <returns></returns>
        public override double ComputeCost(Vector theta)
        {
            int m = X.Rows;

            double j = 0.0;

            Vector s = (X * theta).ToVector();

            j = 1.0 / (2.0 * m) * ((s - this.Y) ^ 2.0).Sum();

            if (this.Lambda != 0)
            {
                j = this.Regularizer.Regularize(j, theta, m, this.Lambda);
            }

            return j;
        }

        /// <summary>
        /// Compute the error cost of the given Theta parameter for the training and label sets
        /// </summary>
        /// <param name="theta">Learning Theta parameters</param>
        /// <returns></returns>
        public override Vector ComputeGradient(Vector theta)
        {
            int m = X.Rows;
            Vector gradient = Vector.Zeros(theta.Length);

            Vector s = (X * theta).ToVector();

            for (int i = 0; i < theta.Length; i++)
            {
                gradient[i] = 1.0 / m * ((s - this.Y) * X[i, VectorType.Col]).Sum();
            }

            if (this.Lambda != 0)
            {
                gradient = this.Regularizer.Regularize(theta, gradient, m, this.Lambda);
            }

            return gradient;
        }
    }
}
