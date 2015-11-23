using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Utils;

namespace numl.Features
{
    /// <summary>
    /// FeatureProperties class.
    /// </summary>
    public class FeatureProperties
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
        /// Instantiate new FeatureProperties object.
        /// </summary>
        public FeatureProperties()
        {
        }
    }
}
