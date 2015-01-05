using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Utils;
using numl.Math.LinearAlgebra;

namespace numl.PreProcessing
{
    /// <summary>
    /// Feature Normalisation extension methods
    /// </summary>
    public static class FeatureNormalizer
    {
        /// <summary>
        /// Performs feature scaling on the supplied value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="avg">The feature average</param>
        /// <param name="std">The standard deviation of the feature</param>
        /// <returns></returns>
        public static double FeatureScale(double value, double avg, double std)
        {
            return (value - avg) / std;
        }

        /// <summary>
        /// Performs feature scaling on the supplied array and returns a copy
        /// </summary>
        /// <param name="column">Column array to compute</param>
        /// <returns></returns>
        public static double[] FeatureScale(double[] column)
        {
            if (column == null)
                throw new ArgumentNullException("Column was null");

            double[] result = new double[column.Length];
            
            double avg = column.Average();
            double sdv = column.StandardDeviation(c => c, false);
            for (int x = 0; x < column.Length; x++)
            {
                result[x] = FeatureNormalizer.FeatureScale(column[x], avg, sdv);
            }
            return result;
        }

        /// <summary>
        /// Performs feature scaling on the supplied column vector and returns a copy
        /// </summary>
        /// <param name="column">Column vector to compute</param>
        /// <returns></returns>
        public static Vector FeatureScale(Vector column)
        {
            double[] temp = column.ToArray();
            return new Vector(FeatureNormalizer.FeatureScale(temp));
        }

        
    }
}
