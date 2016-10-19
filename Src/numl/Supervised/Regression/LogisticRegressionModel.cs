using System;
using System.Linq;
using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Supervised.Classification;
using numl.Utils;

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
        /// The additional number of polynomial features to apply as used when generating the model.
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
            x = IncreaseDimensions(x, this.PolynomialFeatures);

            this.Preprocess(x);

            return LogisticFunction.Compute(x.Insert(0, 1.0, false).Dot(Theta));
        }

        /// <summary>
        /// Create a prediction based on the learned Theta values and the supplied test item.
        /// </summary>
        /// <param name="x">Training record</param>
        /// <returns></returns>
        public override double Predict(Vector x)
        {
            return this.PredictRaw(x) >= 0.5d ? Ject.DefaultTruthValue : Ject.DefaultFalseValue;
        }

        /// <summary>
        /// Adds a specified number of polynomial features to the training / test Vector.
        /// </summary>
        /// <param name="x">Training / Testing record</param>
        /// <param name="polynomialFeatures">Number of polynomial features to add</param>
        /// <returns></returns>
        public static Vector IncreaseDimensions(Vector x, int polynomialFeatures)
        {
            Vector xtemp = x.Copy();
            int maxCols = xtemp.Length;
            for (int j = 0; j < maxCols - 1; j++)
            {
                for (int k = 0; k <= polynomialFeatures; k++)
                {
                    for (int m = 0; m <= k; m++)
                    {
                        double v = (System.Math.Pow(xtemp[j], k - m) * System.Math.Pow(xtemp[j + 1], m));
                        xtemp = xtemp.Insert(xtemp.Length - 1, v);
                    }
                }
            }
            return xtemp;
        }
    }
}
