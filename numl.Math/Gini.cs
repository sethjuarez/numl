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

namespace numl.Math
{
    public class Gini : Impurity
    {
        internal Gini()
        {
            _conditional = false;
            _width = Int16.MaxValue;
        }

        public Gini(Vector x, Vector y = null, int width = 2)
        {
            _x = x;
            if (y != null)
                _y = y;
            _width = width;
            _conditional = false;
        }

        internal override double Calculate(Vector x)
        {
            if (x == null)
                throw new InvalidOperationException("x does not exist!");

            var px = (from i in x.Distinct()
                      let q = (from j in x
                               where j == i
                               select j).Count()
                      select (q / (double)x.Length)).ToArray();

            var g = 1 - (from j in px
                         select j * j).Sum();
            
            // thought this was the right one...
            //for (int i = 0; i < px.Length; i++)
            //    for (int j = i + 1; j < px.Length; j++)
            //        imp += px[i] * px[j];

            return System.Math.Round(g, 4); ;
        }

        public static Gini Of(Vector x)
        {
            return new Gini { _x = x, _conditional = false };
        }
    }
}
