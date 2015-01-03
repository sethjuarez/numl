// file:	Math\Information\Impurity.cs
//
// summary:	Implements the impurity class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Information
{
    /// <summary>An impurity.</summary>
    public abstract class Impurity
    {
        /// <summary>
        /// Calculated ranges used for segmented splits. This is generated when a segmented conditional
        /// impurity value is calculated.
        /// </summary>
        /// <value>The segments.</value>
        public Range[] Segments { get; set; }
        /// <summary>Gets or sets a value indicating whether the discrete.</summary>
        /// <value>true if discrete, false if not.</value>
        public bool Discrete { get; set; }
        /// <summary>Calculates impurity measure of x.</summary>
        /// <param name="x">The list in question.</param>
        /// <returns>Impurity measure.</returns>
        public abstract double Calculate(Vector x);
        /// <summary>
        /// Calculates segmented conditional impurity of y | x When stipulating segments (s), X is broken
        /// up into s many segments therefore P(X=x_s) becomes a range probability rather than a fixed
        /// probability. In essence the average over H(Y|X = x) becomes SUM_s [ p_s * H(Y|X = x_s) ]. The
        /// values that were used to do the split are stored in the Splits member.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="y">Target impurity.</param>
        /// <param name="x">Conditioned impurity.</param>
        /// <param name="segments">Number of segments over x to condition upon.</param>
        /// <returns>Segmented conditional impurity measure.</returns>
        public double SegmentedConditional(Vector y, Vector x, int segments)
        {
            if (x == null && y == null)
                throw new InvalidOperationException("x and y do not exist!");

            return SegmentedConditional(y, x, x.Segment(segments));
        }
        /// <summary>
        /// Calculates segmented conditional impurity of y | x When stipulating ranges (r), X is broken
        /// up into
        /// |r| many segments therefore P(X=x_r) becomes a range probability
        /// rather than a fixed probability. In essence the average over H(Y|X = x) becomes SUM_s [ p_r *
        /// H(Y|X = x_r) ]. The values that were used to do the split are stored in the Splits member.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="y">Target impurity.</param>
        /// <param name="x">Conditioned impurity.</param>
        /// <param name="ranges">Number of segments over x to condition upon.</param>
        /// <returns>Segmented conditional impurity measure.</returns>
        public double SegmentedConditional(Vector y, Vector x, IEnumerable<Range> ranges)
        {
            if (x == null && y == null)
                throw new InvalidOperationException("x and y do not exist!");

            double p = 0,               // probability of slice
                   h = 0,               // impurity of y | x_i : ith slice
                   result = 0,          // aggregated sum
                   count = x.Count();   // total items in list

            Segments = ranges.OrderBy(r => r.Min).ToArray();
            Discrete = false;

            // for each range calculate
            // conditional impurity and
            // aggregate results
            foreach (Range range in Segments)
            {
                // get slice
                var s = x.Indices(d => d >= range.Min && d < range.Max);
                // slice probability
                p = (double)s.Count() / (double)count;
                // impurity of (y | x_i)
                h = Calculate(y.Slice(s));
                // sum up
                result += p * h;
            }

            return result;
        }
        /// <summary>
        /// Calculates conditional impurity of y | x R(Y|X) is the average of H(Y|X = x) over all
        /// possible values X may take.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="y">Target impurity.</param>
        /// <param name="x">Conditioned impurity.</param>
        /// <returns>Conditional impurity measure.</returns>
        public double Conditional(Vector y, Vector x)
        {
            if (x == null && y == null)
                throw new InvalidOperationException("x and y do not exist!");

            double p = 0,               // probability of slice
                   h = 0,               // impurity of y | x_i : ith slice
                   result = 0,          // aggregated sum
                   count = x.Count();   // total items in list

            var values = x.Distinct().OrderBy(z => z);  // distinct values to split on

            Segments = values.Select(z => Range.Make(z, z)).ToArray();
            Discrete = true;

            
            // for each distinct value 
            // calculate conditional impurity
            // and aggregate results
            foreach (var i in values)
            {
                // get slice
                var s = x.Indices(d => d == i);
                // slice probability
                p = (double)s.Count() / (double)count;
                // impurity of (y | x_i)
                h = Calculate(y.Slice(s));
                // sum up
                result += p * h;
            }

            return result;
        }
        /// <summary>Calculates information gain of y | x.</summary>
        /// <param name="y">Target impurity.</param>
        /// <param name="x">Conditioned impurity.</param>
        /// <returns>Information gain using appropriate measure.</returns>
        public double Gain(Vector y, Vector x)
        {
            return Calculate(y) - Conditional(y, x);
        }
        /// <summary>Segmented gain.</summary>
        /// <param name="y">Target impurity.</param>
        /// <param name="x">The list in question.</param>
        /// <param name="segments">Number of segments over x to condition upon.</param>
        /// <returns>A double.</returns>
        public double SegmentedGain(Vector y, Vector x, int segments)
        {
            return Calculate(y) - SegmentedConditional(y, x, segments);
        }
        /// <summary>Segmented gain.</summary>
        /// <param name="y">Target impurity.</param>
        /// <param name="x">The list in question.</param>
        /// <param name="ranges">Number of segments over x to condition upon.</param>
        /// <returns>A double.</returns>
        public double SegmentedGain(Vector y, Vector x, IEnumerable<Range> ranges)
        {
            return Calculate(y) - SegmentedConditional(y, x, ranges);
        }
        /// <summary>Calculates relative information gain of y | x.</summary>
        /// <param name="y">Target impurity.</param>
        /// <param name="x">Conditioned impurity.</param>
        /// <returns>Relative information gain using appropriate measure.</returns>
        public double RelativeGain(Vector y, Vector x)
        {
            var h_yx = Conditional(y, x);
            var h_y = Calculate(y);
            return (h_y - h_yx) / h_y;
        }
        /// <summary>
        /// Calculates relative information gain of y | x with a specified number of segments.
        /// </summary>
        /// <param name="y">Target impurity.</param>
        /// <param name="x">Conditioned impurity.</param>
        /// <param name="segments">Number of segments.</param>
        /// <returns>Relative segmented information gain using appropriate measure.</returns>
        public double SegmentedRelativeGain(Vector y, Vector x, int segments)
        {
            var h_yx = SegmentedConditional(y, x, segments);
            var h_y = Calculate(y);
            return (h_y - h_yx) / h_y;
        }
        /// <summary>Calculates relative information gain of y | x with under specified ranges.</summary>
        /// <param name="y">Target impurity.</param>
        /// <param name="x">Conditioned impurity.</param>
        /// <param name="ranges">Range breakdown.</param>
        /// <returns>Relative segmented information gain using appropriate measure.</returns>
        public double SegmentedRelativeGain(Vector y, Vector x, IEnumerable<Range> ranges)
        {
            var h_yx = SegmentedConditional(y, x, ranges);
            var h_y = Calculate(y);
            return (h_y - h_yx) / h_y;
        }
    }
}
