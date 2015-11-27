using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using numl.Math.LinearAlgebra;

namespace numl.Math.Normalization
{
    /// <summary>
    /// Standard Min-Max Feature normalizer to scale features to be between -1 and +1.
    /// </summary>
    public class MinMaxNormalizer : INormalizer
    {
        /// <summary>
        /// Normalize a row vector using Min-Max normalization on the supplied feature properties.
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
                item[i] = (row[i] - properties.Minimum[i]) / (properties.Maximum[i] - properties.Minimum[i]);
                item[i] = (double.IsNaN(item[i]) || double.IsInfinity(item[i]) ? 0 : item[i]);
            }
            return item;
        }
    }
}
