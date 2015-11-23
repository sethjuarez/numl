using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using numl.Utils;
using numl.Model;
using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using numl.Supervised.Classification;
using numl.PreProcessing;
using numl.Features;

namespace numl.Supervised.Regression
{
    /// <summary>
    /// A Logistic Regression Model object
    /// </summary>
    public class LogisticRegressionModel : Model, IClassifier
    {
        /// <summary>
        /// Theta parameters vector mapping X to y.
        /// </summary>
        public Vector Theta { get; set; }

        /// <summary>
        /// Logistic function
        /// </summary>
        public IFunction LogisticFunction { get; set; }

        /// <summary>
        /// The additional number of quadratic features to create as used in generating the model
        /// </summary>
        public int PolynomialFeatures { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogisticRegressionModel()
        {
            PolynomialFeatures = 0;
        }

        /// <summary>
        /// Computes the probability of the prediction being True.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double PredictRaw(Vector x)
        {
            Vector xCopy =  (this.NormalizeFeatures ? 
                                this.FeatureNormalizer.Normalize(x.IncreaseDimensions(this.PolynomialFeatures), this.FeatureProperties) 
                                : x.IncreaseDimensions(this.PolynomialFeatures));
            return this.LogisticFunction.Compute(xCopy.Insert(0, 1.0, false).Dot(Theta));
        }

        /// <summary>
        /// Create a prediction based on the learned Theta values and the supplied test item.
        /// </summary>
        /// <param name="x">Training record</param>
        /// <returns></returns>
        public override double Predict(Vector x)
        {
            return this.PredictRaw(x) >= 0.5d ? 1.0d : 0.0d;
        }
    }
}
