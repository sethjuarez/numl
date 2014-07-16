// file:	Math\Range.cs
//
// summary:	Implements the range class
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace numl.Math
{
    /// <summary>A range.</summary>
    [XmlRoot("Range")]
    public class Range
    {
        /// <summary>Gets or sets the minimum.</summary>
        /// <value>The minimum value.</value>
        [XmlAttribute("Min")]
        public double Min { get; set; }
        /// <summary>Gets or sets the maximum.</summary>
        /// <value>The maximum value.</value>
        [XmlAttribute("Max")]
        public double Max { get; set; }
        /// <summary>Tests.</summary>
        /// <param name="d">The double to process.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public bool Test(double d)
        {
            return d >= Min && d < Max;
        }
        /// <summary>Makes.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>A Range.</returns>
        public static Range Make(double min, double max)
        {
            return new Range { Min = min, Max = max };
        }
        /// <summary>Makes.</summary>
        /// <param name="min">The minimum.</param>
        /// <returns>A Range.</returns>
        public static Range Make(double min)
        {
            return new Range { Min = min, Max = min + 0.00001 };
        }
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("[{0}, {1})", Min, Max);
        }
    }
}
