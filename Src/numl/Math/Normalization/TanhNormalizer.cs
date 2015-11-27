using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using numl.Math.LinearAlgebra;
using numl.Math.Functions;
namespace numl.Math.Normalization
{
    /// <summary>
    /// Hyperbolic Tangent Feature normalizer to scale features to be between -1 and +1.
    /// </summary>
    public class TanhNormalizer : INormalizer
    {
        /// <summary>
        /// Gets or sets the tangent function to use for scaling.
        /// </summary>
        public IFunction Tangent { get; set; }

        /// <summary>
        /// Initializes a new Hyperbolic Tangent Feature Normalizer.
        /// </summary>
        public TanhNormalizer()
        {
            this.Tangent = new Tanh();
        }

        /// <summary>
        /// Normalize a row vector using the hyperbolic tangent (tanh)function.
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
                item[i] = this.Tangent.Compute(row[i]);
            }
            return item;
        }
    }
}
