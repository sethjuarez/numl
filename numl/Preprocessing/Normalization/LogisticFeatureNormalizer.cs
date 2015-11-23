using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Features;
using numl.Math.LinearAlgebra;

namespace numl.Preprocessing.Normalization
{
    /// <summary>
    /// Logistic Feature normalizer using sigmoid function to scale features to be between 0 and 1.
    /// </summary>
    public class LogisticFeatureNormalizer : IFeatureNormalizer
    {
        /// <summary>
        /// Gets or sets the logistic function to use for scaling.
        /// </summary>
        public Math.Functions.IFunction Logistic { get; set; }

        /// <summary>
        /// Initializes a new Logistic Feature Normalizer.
        /// </summary>
        public LogisticFeatureNormalizer()
        {
            this.Logistic = new Math.Functions.Logistic();
        }
        /// <summary>
        /// Normalize a row vector using Logistic normalization.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Vector Normalize(Vector row, FeatureProperties properties)
        {
            if (row == null)
            {
                throw new ArgumentNullException("Row was null");
            }
            double[] item = new double[row.Length];
            for (int i = 0; i < row.Length; i++)
            {
                item[i] = this.Logistic.Compute(row[i]);
            }
            return item;
        }
    }
}
