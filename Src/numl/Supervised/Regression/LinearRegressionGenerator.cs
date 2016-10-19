// file:	Supervised\Regression\LinearRegression.cs
//
// summary:	Implements the linear regression class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.Regression
{
    /// <summary>A linear regression generator.</summary>
    public class LinearRegressionGenerator : Generator
    {
        /// <summary>
        /// The regularisation term Lambda
        /// </summary>
        public double Lambda { get; set; }

        /// <summary>Gets or sets the learning rate used with gradient descent.</summary>
        /// <para>The default value is 0.01</para>
        /// <value>The learning rate.</value>
        public double LearningRate { get; set; }

        /// <summary>Gets or sets the maximum iterations used with gradient descent.</summary>
        /// <para>The default is 500</para>
        /// <value>The maximum iterations.</value>
        public int MaxIterations { get; set; }

        /// <summary>
        /// Initialise a new LinearRegressionGenerator
        /// </summary>
        public LinearRegressionGenerator()
        {
            this.MaxIterations = 500;
            this.LearningRate = 0.01;

            this.NormalizeFeatures = true;
        }

        /// <summary>Generate Linear Regression model based on a set of examples.</summary>
        /// <param name="X">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix X, Vector y)
        {
            this.Preprocess(X);

            // copy matrix
            Matrix copy = X.Copy();

            // add intercept term
            copy = copy.Insert(Vector.Ones(copy.Rows), 0, VectorType.Col);

            // create initial theta
            Vector theta = Vector.Rand(copy.Cols);

            // run gradient descent
            var optimizer = new numl.Math.Optimization.Optimizer(theta, this.MaxIterations, this.LearningRate)
            {
                CostFunction = new numl.Math.Functions.Cost.LinearCostFunction()
                {
                    X = copy,
                    Y = y,
                    Lambda = this.Lambda,
                    Regularizer = new numl.Math.Functions.Regularization.L2Regularizer()
                }
            };

            optimizer.Run();

            // once converged create model and apply theta

            LinearRegressionModel model = new LinearRegressionModel()
            { 
                Descriptor = this.Descriptor,
                NormalizeFeatures = base.NormalizeFeatures,
                FeatureNormalizer = base.FeatureNormalizer,
                FeatureProperties = base.FeatureProperties,
                Theta = optimizer.Properties.Theta
            };

            return model;
        }
    }
}
