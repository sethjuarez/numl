using System.Linq;
using numl.Math.LinearAlgebra;

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
            if (LogisticFunction == null)
                LogisticFunction = new Logistic();
        }

        /// <summary>
        /// Compute the error cost of the given Theta parameter for the training and label sets
        /// </summary>
        /// <param name="theta">Learning Theta parameters</param>
        /// <returns></returns>
        public override double ComputeCost(Vector theta)
        {
            int m = X.Rows;

            Vector s = (X * theta).ToVector();

            s = s.Calc(v => LogisticFunction.Compute(v));

            Vector slog = s.Calc(v => System.Math.Log(1.0 - v));

            double j = (-1.0 / m) * (Y.Dot(s.Log()) + ((1.0 - Y).Dot(slog)));

            if (Lambda != 0)
            {
                j = Regularizer.Regularize(j, theta, m, Lambda);
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

            s = s.Each(v => LogisticFunction.Compute(v));

            for (int i = 0; i < theta.Length; i++)
            {
                gradient[i] = (1.0 / m) * ((s - Y) * X[i, VectorType.Col]).Sum();
            }

            if (Lambda != 0)
            {
                gradient = Regularizer.Regularize(gradient, theta, m, Lambda);
            }

            return gradient;
        }
    }
}
