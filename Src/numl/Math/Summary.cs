using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math
{
    /// <summary>
    /// Summary class.
    /// </summary>
    public class Summary
    {
        /// <summary>
        /// Vector of all columns and the average values for each.
        /// </summary>
        public Vector Average { get; set; }

        /// <summary>
        /// Vector of all columns and the min values for each.
        /// </summary>
        public Vector Minimum { get; set; }

        /// <summary>
        /// Vector of all columns and the median values for each.
        /// </summary>
        public Vector Median { get; set; }

        /// <summary>
        /// Vector of all columns and the max values for each.
        /// </summary>
        public Vector Maximum { get; set; }

        /// <summary>
        /// Vector of all columns and the standard deviation for each.
        /// </summary>
        public Vector StandardDeviation { get; set; }

        /// <summary>
        /// Summarizes a given Matrix.
        /// </summary>
        /// <param name="matrix">Matrix to summarize.</param>
        /// <param name="byVector">Indicates which direction to summarize, default is <see cref="VectorType.Row"/> indicating top-down.</param>
        /// <returns></returns>
        public static Summary Summarize(Matrix matrix, VectorType byVector = VectorType.Row)
        {
            return new Summary()
            {
                Average = matrix.Mean(byVector),
                StandardDeviation = matrix.StdDev(byVector),
                Minimum = matrix.Min(byVector),
                Maximum = matrix.Max(byVector),
                Median = matrix.Median(byVector)
            };
        }
    }
}
