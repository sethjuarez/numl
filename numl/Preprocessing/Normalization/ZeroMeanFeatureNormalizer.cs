using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Utils;
using numl.Math.LinearAlgebra;
using numl.Features;
using numl.Math;

namespace numl.Preprocessing.Normalization
{
    /// <summary>
    /// Zero-Mean Feature normalizer to scale features to be zero centered.
    /// </summary>
    public class ZeroMeanFeatureNormalizer : IFeatureNormalizer
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
                item[i] = (row[i] - properties.Average[i]);
            }
            return item;
        }
    }
}
