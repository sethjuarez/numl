/*
 Copyright (c) 2012 Seth Juarez

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math;
using numl.Math.LinearAlgebra;

namespace numl.Math.Information
{
    public abstract class Impurity
    {
        /// <summary>
        /// Calculated ranges used for
        /// segmented splits. This is
        /// generated when a segmented
        /// conditional impurity value
        /// is calculated.
        /// </summary>
        public Range[] Segments { get; set; }

        public bool Segmented { get; set; }

        /// <summary>
        /// Calculates impurity measure of <c>x</c>.
        /// </summary>
        /// <param name="x">The list in question</param>
        /// <returns>Impurity measure</returns>
        public abstract double Calculate(Vector x);

        /// <summary>
        /// Calculates segmented conditional impurity of <c>y | x</c>
        /// When stipulating segments (s), X is broken up into 
        /// s many segments therefore P(X=x_s) becomes a range probability
        /// rather than a fixed probability. In essence the average over
        /// H(Y|X = x) becomes SUM_s [ p_s * H(Y|X = x_s) ]. The values that
        /// were used to do the split are stored in the Splits member.
        /// </summary>
        /// <param name="y">Target impurity</param>
        /// <param name="x">Conditioned impurity</param>
        /// <param name="segments">Number of segments over x to condition upon</param>
        /// <returns>Segmented conditional impurity measure</returns>
        public double SegmentedConditional(Vector y, Vector x, int segments)
        {
            if (x == null && y == null)
                throw new InvalidOperationException("x and y do not exist!");

            return SegmentedConditional(y, x, x.Segment(segments));
        }

        /// <summary>
        /// Calculates segmented conditional impurity of <c>y | x</c>
        /// When stipulating ranges (r), X is broken up into 
        /// |r| many segments therefore P(X=x_r) becomes a range probability
        /// rather than a fixed probability. In essence the average over
        /// H(Y|X = x) becomes SUM_s [ p_r * H(Y|X = x_r) ]. The values that
        /// were used to do the split are stored in the Splits member.
        /// </summary>
        /// <param name="y">Target impurity</param>
        /// <param name="x">Conditioned impurity</param>
        /// <param name="segments">Number of segments over x to condition upon</param>
        /// <returns>Segmented conditional impurity measure</returns>
        public double SegmentedConditional(Vector y, Vector x, IEnumerable<Range> ranges)
        {
            if (x == null && y == null)
                throw new InvalidOperationException("x and y do not exist!");

            double p = 0,               // probability of slice
                   h = 0,               // impurity of y | x_i : ith slice
                   result = 0,          // aggregated sum
                   count = x.Count();   // total items in list

            Segments = ranges.OrderBy(r => r.Min).ToArray();
            Segmented = true;

            // for each range calculate
            // conditional impurity and
            // aggregate results
            foreach (Range range in Segments)
            {
                // get slice
                var s = x.Indices(d => d >= range.Min && d < range.Max);
                // slice probability
                p = s.Count() / count;
                // impurity of (y | x_i)
                h = Calculate(x.Slice(s));
                // sum up
                result += p * h;
            }

            return result;
        }

        /// <summary>
        /// Calculates conditional impurity of <c>y | x</c>
        /// R(Y|X) is the average of H(Y|X = x) over all possible values
        /// X may take. 
        /// </summary>
        /// <param name="y">Target impurity</param>
        /// <param name="x">Conditioned impurity</param>
        /// <param name="width">Split of values over x to condition upon</param>
        /// <returns>Conditional impurity measure</returns>
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
            Segmented = false;

            
            // for each distinct value 
            // calculate conditional impurity
            // and aggregate results
            foreach (var i in values)
            {
                // get slice
                var s = x.Indices(d => d == i);
                // slice probability
                p = s.Count() / count;
                // impurity of (y | x_i)
                h = Calculate(x.Slice(s));
                // sum up
                result += p * h;
            }

            return result;
        }
        
        /// <summary>
        /// Calculates information gain of <c>y | x</c>
        /// </summary>
        /// <param name="y">Target impurity</param>
        /// <param name="x">Conditioned impurity</param>
        /// <param name="width">Split of values over x to condition upon</param>
        /// <returns>Information gain using appropriate measure</returns>
        public double Gain(Vector y, Vector x)
        {
            return Calculate(y) - Conditional(y, x);
        }

        public double SegmentedGain(Vector y, Vector x, int segments)
        {
            return Calculate(y) - SegmentedConditional(y, x, segments);
        }

        public double SegmentedGain(Vector y, Vector x, IEnumerable<Range> ranges)
        {
            return Calculate(y) - SegmentedConditional(y, x, ranges);
        }

        /// <summary>
        /// Calculates relative information gain of <c>y | x</c>
        /// </summary>
        /// <param name="y">Target impurity</param>
        /// <param name="x">Conditioned impurity</param>
        /// <param name="width">Split of values over x to condition upon</param>
        /// <returns>Relative information gain using appropriate measure</returns>
        public double RelativeGain(Vector y, Vector x)
        {
            var h_yx = Conditional(y, x);
            var h_y = Calculate(y);
            return (h_y - h_yx) / h_y;
        }

        public double SegmentedRelativeGain(Vector y, Vector x, int segments)
        {
            var h_yx = SegmentedConditional(y, x, segments);
            var h_y = Calculate(y);
            return (h_y - h_yx) / h_y;
        }

        public double SegmentedRelativeGain(Vector y, Vector x, IEnumerable<Range> ranges)
        {
            var h_yx = SegmentedConditional(y, x, ranges);
            var h_y = Calculate(y);
            return (h_y - h_yx) / h_y;
        }
    }
}
