using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math
{
    /// <summary>
    /// FeatureProperties class.
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
    }
}
