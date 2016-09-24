using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Functions.Cost
{
    /// <summary>
    /// Implements a logistic cost function
    /// </summary>
    public class LogisticCostFunction : CostFunction
    {
        /// <summary>
        /// Gets or sets the logistic function.
        /// </summary>
        public IFunction LogisticFunction { get; set; }

        /// <summary>
        /// Initializes a new LogisticCostFunction with the default sigmoid logistic function.
        /// </summary>
        public LogisticCostFunction()
        {
            if (this.LogisticFunction == null)
                this.LogisticFunction = new Math.Functions.Logistic();
        }

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

            s = s.Each(v => this.LogisticFunction.Compute(v));

            Vector slog = s.Copy().Each(v => System.Math.Log(System.Math.Abs(1.0 - v)));

            j = (-1.0 / m) * ( (this.Y.Dot(s.Log())) + ((1.0 - this.Y).Dot(slog)) );

            if (this.Lambda != 0)
            {
                j = this.Regularizer.Regularize(j, theta, m, this.Lambda);
            }

            return j;
        }

        /// <summary>
        /// Compute the error gradient of the given Theta parameter for the training and label sets
        /// </summary>
        /// <param name="theta">Learning Theta parameters</param>
        /// <returns></returns>
        public override Vector ComputeGradient(Vector theta)
        {
            int m = X.Rows;
            Vector gradient = Vector.Zeros(theta.Length);

            Vector s = (X * theta).ToVector();

            s = s.Each(v => this.LogisticFunction.Compute(v));

            for (int i = 0; i < theta.Length; i++)
            {
                gradient[i] = (1.0 / m) * ((s - this.Y) * X[i, VectorType.Col]).Sum();
            }

            if (this.Lambda != 0)
            {
                gradient = this.Regularizer.Regularize(theta, gradient, m, this.Lambda);
            }

            return gradient;
        }
    }
}
