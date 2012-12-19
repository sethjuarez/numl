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
using numl.Math;

using numl.Math.LinearAlgebra;

namespace numl.Math.Metrics
{
    public sealed class PearsonCorrelation : ISimilarity
    {
        public double Compute(Vector x, Vector y)
        {
            if (x.Length != y.Length)
                throw new InvalidOperationException("Cannot compute similarity between two unequally sized Vectors!");

            var xSum = x.Sum();
            var ySum = y.Sum();
            
            var xElem = (x^2).Sum() - ((xSum * xSum) / x.Length);
            var yElem = (y^2).Sum() - ((ySum * ySum) / y.Length);

            return (x.Dot(y) - ((xSum * ySum) / x.Length)) / System.Math.Sqrt(xElem * yElem);
        }
    }
}