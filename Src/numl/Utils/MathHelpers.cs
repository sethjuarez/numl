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
        /// Constrains the value to be within the specified range.
        /// </summary>
        /// <param name="value">Value to clip.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>Double.</returns>
        public static int Clip(this int value, int minValue, int maxValue)
        {
            value = System.Math.Min(value, maxValue);
            value = System.Math.Max(value, minValue);
            return value;
        }

        /// <summary>
        /// Constrains the value to be within the specified range.
        /// </summary>
        /// <param name="value">Value to clip.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>Double.</returns>
        public static double Clip(this double value, double minValue, double maxValue)
        {
            value = System.Math.Min(value, maxValue);
            value = System.Math.Max(value, minValue);
            return value;
        }

        /// <summary>
        /// Returns the number of significant digits after the decimal point.
        /// </summary>
        /// <param name="value">Value to compute precision.</param>
        /// <param name="significantOnly">If true, counts significant digits only.</param>
        /// <returns></returns>
        public static int GetPrecision(this double value, bool significantOnly = true)
        {
            int precision = 0;

            while (value * System.Math.Pow(10, precision) != System.Math.Round(value * System.Math.Pow(10, precision)))
            {
                precision++;
            }

            return precision;
        }

        /// <summary>
        /// Rescales the current value to be it's equivalent value in the new range scale.
        /// </summary>
        /// <param name="value">Value to rescale to the new range.</param>
        /// <param name="scaleMin">Minimum value in the current range..</param>
        /// <param name="scaleMax">Maximum value in the current range.</param>
        /// <param name="newScaleMin">Minimum value in the new range.</param>
        /// <param name="newScaleMax">Maximum value in the new range.</param>
        /// <returns>Double.</returns>
        public static double Rescale(this double value, double scaleMin, double scaleMax, double newScaleMin, double newScaleMax)
        {
            return (((newScaleMax - newScaleMin) * (value - scaleMin)) / (scaleMax - scaleMin));
        }

        /// <summary>
        /// Returns the running average of the current and new values.
        /// </summary>
        /// <param name="value">Current average value.</param>
        /// <param name="newValue">New value.</param>
        /// <param name="n">Number of previous samples.</param>
        /// <returns>Double.</returns>
        public static double RunningAverage(this double value, double newValue, int n)
        {
            return (value + newValue) / n;
        }

        /// <summary>
        /// Converts the angle in degrees to radians.
        /// </summary>
        /// <param name="degrees">Angle in degrees.</param>
        /// <returns>Double.</returns>
        public static double ToRadians(this double degrees)
        {
            return degrees * (System.Math.PI / 180.0d);
        }

        /// <summary>
        /// Converts the angle in radians to degrees.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>Double.</returns>
        public static double ToDegrees(this double radians)
        {
            return radians * (180.0d / System.Math.PI);
        }

    }
}
