using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using numl.Math.Functions;
using numl.Functions;

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
        /// The additional number of quadratic features to create.
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
        /// Initialises a LogisticRegressionGenerator object
        /// </summary>
        public LogisticRegressionGenerator()
        {
            Lambda = 1;
            MaxIterations = 500;
            PolynomialFeatures = 5;
            LearningRate = 0.3;
        }

        /// <summary>Generate Logistic Regression model based on a set of examples.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix x, Vector y)
        {
            // create initial theta
            Matrix copy = x.Copy();

            copy = PreProcessing.FeatureDimensions.IncreaseDimensions(copy, PolynomialFeatures);

            // add intercept term
            copy = copy.Insert(Vector.Ones(copy.Rows), 0, VectorType.Col);

            Vector theta = Vector.Ones(copy.Cols);

            var run = GradientDescent.Run(theta, copy, y, this.MaxIterations, this.LearningRate, new Functions.CostFunctions.LogisticCostFunction(), 
                this.Lambda, new Regularization());

            LogisticRegressionModel model = new LogisticRegressionModel()
            {
                Descriptor = this.Descriptor,
                Theta = run.Item2,
                LogisticFunction = new Math.Functions.Logistic(),
                PolynomialFeatures = this.PolynomialFeatures
            };

            return model;
        }
    }
}
