using System;
using System.Linq;
using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.Regression
{
    /// <summary>A logistic regression generator.</summary>
    public class LogisticRegressionGenerator : Generator
    {
        /// <summary>
        /// The regularisation term Lambda
        /// </summary>
        public double Lambda { get; set; }

        /// <summary>
        /// The additional number of polynomial features to create, i.e. for polynomial regression.
        /// <para>(A higher value may overfit training data)</para>
        /// </summary>
        public int PolynomialFeatures { get; set; }

        /// <summary>Gets or sets the maximum iterations used with gradient descent.</summary>
        /// <para>The default is 500</para>
        /// <value>The maximum iterations.</value>
        public int MaxIterations { get; set; }

        /// <summary>Gets or sets the learning rate used with gradient descent.</summary>
        /// <para>The default value is 0.01</para>
        /// <value>The learning rate.</value>
        public double LearningRate { get; set; }

        /// <summary>
        /// Gets or sets the logit function to use.
        /// </summary>
        public IFunction LogisticFunction { get; set; }

        /// <summary>
        /// Initialises a LogisticRegressionGenerator object
        /// </summary>
        public LogisticRegressionGenerator()
        {
            this.Lambda = 1;
            this.MaxIterations = 500;
            this.PolynomialFeatures = 0;
            this.LearningRate = 0.3;
            this.LogisticFunction = new Math.Functions.Logistic();

            this.NormalizeFeatures = true;
        }

        /// <summary>Generate Logistic Regression model based on a set of examples.</summary>
        /// <param name="X">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix X, Vector y)
        {
            X = IncreaseDimensions(X, this.PolynomialFeatures);

            this.Preprocess(X);

            // guarantee 1/0 based label vector
            y = y.ToBinary(f => f == 1d, falseValue: 0d);

            // add intercept term
            X = X.Insert(Vector.Ones(X.Rows), 0, VectorType.Col, false);

            Vector theta = Vector.Rand(X.Cols);

            // run gradient descent
            var optimizer = new numl.Math.Optimization.Optimizer(theta, this.MaxIterations, this.LearningRate)
            {
                CostFunction = new numl.Math.Functions.Cost.LogisticCostFunction()
                {
                    X = X,
                    Y = y,
                    Lambda = this.Lambda,
                    Regularizer = new numl.Math.Functions.Regularization.L2Regularizer(),
                    LogisticFunction = this.LogisticFunction
                }
            };

            optimizer.Run();

            LogisticRegressionModel model = new LogisticRegressionModel()
            {
                Descriptor = this.Descriptor,
                NormalizeFeatures = base.NormalizeFeatures,
                FeatureNormalizer = base.FeatureNormalizer,
                FeatureProperties = base.FeatureProperties,
                Theta = optimizer.Properties.Theta,
                LogisticFunction = this.LogisticFunction,
                PolynomialFeatures = this.PolynomialFeatures
            };

            return model;
        }

        /// <summary>
        /// Adds a specified number of polynomial features to the training set Matrix.
        /// </summary>
        /// <param name="x">Training set</param>
        /// <param name="polynomialFeatures">Number of polynomial features to add</param>
        /// <returns></returns>
        public static Matrix IncreaseDimensions(Matrix x, int polynomialFeatures)
        {
            Matrix Xtemp = x.Copy();
            int maxCols = Xtemp.Cols;
            for (int j = 0; j < maxCols - 1; j++)
            {
                for (int k = 0; k <= polynomialFeatures; k++)
                {
                    for (int m = 0; m <= k; m++)
                    {
                        Vector v = (Xtemp[j, VectorType.Col].ToVector() ^ (double)(k - m)) * (Xtemp[j + 1, VectorType.Col] ^ (double)m).ToVector();
                        Xtemp = Xtemp.Insert(v, Xtemp.Cols - 1, VectorType.Col);
                    }
                }
            }
            return Xtemp;
        }
    }
}
