// file:	Supervised\Regression\LinearRegression.cs
//
// summary:	Implements the linear regression class
using System;
using System.Linq;
using System.Collections.Generic;

using numl.Math.LinearAlgebra;
using numl.PreProcessing;
using numl.Functions;

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
        }

        /// <summary>Generate Linear Regression model based on a set of examples.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix x, Vector y)
        {
            // create initial theta
            Vector theta = Vector.Ones(x.Cols + 1);
            Matrix copy = x.Copy();

            // normalise features
            for (int i = 0; i < copy.Cols; i++)
            {
                var j = FeatureNormalizer.FeatureScale(copy[i, VectorType.Col]);
                for (int k = 0; k < copy.Rows; k++)
                {
                    copy[k, i] = j[k];
                }
            }

            // add intercept term
            copy = copy.Insert(Vector.Ones(copy.Rows), 0, VectorType.Col);
            
            // run gradient descent
            var run = GradientDescent.Run(theta, copy, y, this.MaxIterations, this.LearningRate, new Functions.CostFunctions.LinearCostFunction(), 
                this.Lambda, new Regularization());

            // once converged create model and apply theta

            LinearRegressionModel model = new LinearRegressionModel(x.Mean(VectorType.Row), x.StdDev(VectorType.Row))
            { 
                Descriptor = this.Descriptor,
                Theta = run.Item2
            };

            return model;
        }
    }
}
