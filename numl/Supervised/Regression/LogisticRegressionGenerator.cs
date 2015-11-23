using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using numl.Math.Functions;
using numl.Optimization;
using numl.Optimization.Functions;
using numl.Optimization.Functions.CostFunctions;

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
        /// The additional number of quadratic features to create, i.e. for polynomial regression.
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
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix x, Vector y)
        {
            Matrix copy = PreProcessing.FeatureDimensions.IncreaseDimensions(x.Copy(), this.PolynomialFeatures);

            this.Preprocess(copy, y);

            // guarantee 1/0 based label vector
            y = y.ToBinary(f => f == 1d);

            // add intercept term
            copy = copy.Insert(Vector.Ones(copy.Rows), 0, VectorType.Col, false);

            Vector theta = Vector.Rand(copy.Cols);

            // run gradient descent
            var optimizer = new Optimizer(theta, this.MaxIterations, this.LearningRate)
            {
                CostFunction = new LogisticCostFunction()
                {
                    X = copy,
                    Y = y,
                    Lambda = this.Lambda,
                    Regularizer = new L2Regularizer(),
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
    }
}
