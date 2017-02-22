// file:	Math\Range.cs
//
// summary:	Implements the range class
using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math
{
    /// <summary>A range.</summary>
    public class Range
    {
        /// <summary>Gets or sets the minimum.</summary>
        /// <value>The minimum value.</value>
        public double Min { get; private set; }
        /// <summary>Gets or sets the maximum.</summary>
        /// <value>The maximum value.</value>
        public double Max { get; private set; }
        /// <summary>Tests.</summary>
        /// <param name="d">The double to process.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public bool Test(double d)
        {
            return d >= Min && d <= Max;
        }
        /// <summary>Constructor taking min and max vaue to create Range.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public Range(double min, double max)
        {
            Min = min;
            Max = max;
        }
        /// <summary>Constructor taking only minimum value and creating slightly greated max.</summary>
        /// <param name="min">The minimum.</param>
        public Range(double min) : this(min, min + 0.00001)
        {
        }
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("[{0}, {1})", Min, Max);
        }
    }
}
