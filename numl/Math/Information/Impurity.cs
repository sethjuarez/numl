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
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.Information
{
    public enum ImpurityType
    {
        Entropy,
        Gini,
        Error
    }

    public abstract class Impurity
    {
        public double[] SplitValues { get; set; }
        internal Vector _x;
        internal Vector _y;
        internal bool _conditional;
        internal int _width;

        public int Width
        {
            get
            {
                return _width;
            }
        }

        private double _value = -1;
        public double Value
        {
            get
            {
                if (_value == -1)
                    // single item 
                    if (!_conditional)
                        _value = Calculate(_x);
                    else
                        _value = CalculateConditional(_y, _x);

                return _value;
            }
        }

        internal abstract double Calculate(Vector x);

        internal double CalculateConditional(Vector y, Vector x)
        {
            if (x == null && y == null)
                throw new InvalidOperationException("x and y do not exist!");

            var values = x.Distinct();
            var valueCount = values.Count();
            double result = 0;

            // binary split
            if (Width == 2)
            {
                // do binary split based on mean
                var mean = x.Mean();
                var ltSlice = y.Slice(x.Indices(d => d < mean));
                var gtSlice = y.Slice(x.Indices(d => d >= mean));

                var lh = Calculate(ltSlice);
                var rh = Calculate(gtSlice);

                result = (0.5 * lh) + (0.5 * rh);
                SplitValues = new double[] { x.Min(), mean, x.Max() + 1 };
            }
            // if valueCount has more values than
            // width allows, segment out data to
            // fit width
            else if (valueCount > Width)
            {
                double p = 0;
                double h = 0;
                // create segmentation
                // should really consider
                // some kind of distribution
                // for doing segmentation..
                SplitValues = x.Segment(Width);
                // for convenience ;)
                SplitValues[SplitValues.Length - 1] += 1;
                for (int i = 1; i < SplitValues.Length; i++)
                {
                    // get slice
                    var s = x.Indices(d => d >= SplitValues[i - 1] && d < SplitValues[i]);

                    // probability of slice
                    p = s.Count() / (double)x.Length;

                    // Impurity (y | x_i)
                    h = Calculate(y.Slice(s));

                    // sum up
                    result += p * h;
                }
            }
            // otherwise perform regular counting 
            // exercises to find cond entropy
            else
            {
                double p = 0;
                double h = 0;
                // says there is no width
                // since deterministic
                // slice values
                _width = 0;
                SplitValues = new double[valueCount];
                int k = -1;
                // each disting x value
                foreach (var i in values)
                {
                    SplitValues[++k] = i;

                    // get slice
                    var s = x.Indices(d => d == i);

                    // probability of x_i
                    p = s.Count() / (double)x.Length;

                    // Impurity (y | x_i)
                    h = Calculate(y.Slice(s));

                    // sum up
                    result += p * h;
                }
            }

            return result;
        }

        internal double _gain = -1;
        public double Gain()
        {
            if (_x == null && _y == null)
                throw new InvalidOperationException("x and y do not exist!");

            if (_gain == -1)
                _gain = Calculate(_y) - CalculateConditional(_y, _x);

            return _gain;
        }

        internal double _relGain = -1;
        public double RelativeGain()
        {
            if (_x == null && _y == null)
                throw new InvalidOperationException("x and y do not exist!");

            if (_relGain == -1)
            {
                var h_yx = CalculateConditional(_y, _x);
                var h_y = Calculate(_y);

                _relGain = (h_y - h_yx) / h_y;
            }

            return _relGain;
        }
    }
}
