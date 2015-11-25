using System;
using System.Linq;

using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Math.Normalization;
namespace numl.Math.Normalization
{
    /// <summary>
    /// Z-Score Feature normalizer to scale features to be 0 mean centered (-1 to +1).
    /// </summary>
    public class ZScoreFeatureNormalizer : INormalizer
    {
        /// <summary>
        /// Normalize a row vector using Z-Score normalization on the supplied feature properties.
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
                item[i] = (row[i] - properties.Average[i]) / properties.StandardDeviation[i];
                item[i] = (double.IsNaN(item[i]) || double.IsInfinity(item[i]) ? 0d : item[i]);
            }
            return item;
        }
    }
}
