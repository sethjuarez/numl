using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Utils;
using numl.Math.LinearAlgebra;
using numl.Features;

namespace numl.Preprocessing.Normalization
{
    /// <summary>
    /// Z-Score Feature normalizer to scale features to be 0 mean centered (-1 to +1).
    /// </summary>
    public class ZScoreFeatureNormalizer : IFeatureNormalizer
    {
        /// <summary>
        /// Normalize a row vector using Z-Score normalization on the supplied feature properties.
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
                item[i] = (row[i] - properties.Average[i]) / properties.StandardDeviation[i];
                item[i] = (double.IsNaN(item[i]) || double.IsInfinity(item[i]) ? 0d : item[i]);
            }
            return item;
        }
    }
}
