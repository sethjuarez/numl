using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using numl.Math.LinearAlgebra;
using numl.Math.Functions;
namespace numl.Math.Normalization
{
    /// <summary>
    /// Logistic Feature normalizer using sigmoid function to scale features to be between 0 and 1.
    /// </summary>
    public class LogisticNormalizer : INormalizer
    {
        /// <summary>
        /// Gets or sets the logistic function to use for scaling.
        /// </summary>
        public IFunction Logistic { get; set; }

        /// <summary>
        /// Initializes a new Logistic Feature Normalizer.
        /// </summary>
        public LogisticNormalizer()
        {
            this.Logistic = new Logistic();
        }
        /// <summary>
        /// Normalize a row vector using Logistic normalization.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Vector Normalize(Vector row, numl.Math.Summary properties)
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
