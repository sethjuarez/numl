using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Utils
{
    /// <summary>
    /// Extension methods for math and floating point operations.
    /// </summary>
    public static class MathHelpers
    {
        /// <summary>
        /// Constrains the values to be within the specified range.
        /// </summary>
        /// <param name="value">Value to clip.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns></returns>
        public static int Clip(this int value, int minValue, int maxValue)
        {
            value = System.Math.Min(value, maxValue);
            value = System.Math.Max(value, minValue);
            return value;
        }

        /// <summary>
        /// Constrains the values to be within the specified range.
        /// </summary>
        /// <param name="value">Value to clip.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns></returns>
        public static double Clip(this double value, double minValue, double maxValue)
        {
            value = System.Math.Min(value, maxValue);
            value = System.Math.Max(value, minValue);
            return value;
        }
    }
}
