// file:	Supervised\Regression\LinearRegression.cs
//
// summary:	Implements the linear regression class
using System;
using System.Linq;
using System.Collections.Generic;

using numl.Math.LinearAlgebra;
using numl.PreProcessing;

namespace numl.Supervised.Regression
{
    /// <summary>A linear regression generator.</summary>
    public class LinearRegressionGenerator : Generator
    {
        /// <summary>
        /// If the feature cutoff count is reached, the generator will use gradient descent 
        /// for initialisation instead of normal equations. This reduces processor complexity.
        /// <para>The default value is false.</para>
        /// </summary>
        public bool UseGradientDescent { get; set; }

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
            this.UseGradientDescent = false;
            this.MaxIterations = 500;
            this.LearningRate = 0.01;
        }

        /// <summary>Generate model based on a set of examples.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix x, Vector y)
        {
            // create initial theta
            int width = x.Cols;

            Vector theta = Vector.Zeros(3);
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

            if (!this.UseGradientDescent)
            {
                theta = ((((copy.Transpose() * copy).Inverse()) * copy.Transpose()) * y).ToVector();
            }
            else
            {
                int m = copy.Rows;
                
                // run gradient descent on theta
                for (int i = 0; i < this.MaxIterations; i++)
                {
                    theta = theta - (this.LearningRate / m * (((copy * theta).ToVector()) - y) * copy).ToVector();
                }
            }

            // once converged create model and apply theta

            LinearRegressionModel model = new LinearRegressionModel(x.Mean(VectorType.Row), x.StdDev(VectorType.Row)) { 
                Theta = theta
            };

            return model;
        }
    }
}
