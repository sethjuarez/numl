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

using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Math
{
    public static class Helpers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IEnumerable<double> Slice(this IEnumerable<double> x, Func<double, bool> where)
        {
            foreach (double d in x)
                if (where(d))
                    yield return d;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public static IEnumerable<double> Slice(this IEnumerable<double> x, IEnumerable<int> indices)
        {
            int i = -1;
            // x enumerator
            var enumx = x.GetEnumerator();
            // ordered indices
            var enumi = indices.Distinct().OrderBy(k => k).GetEnumerator();
            if (enumi.MoveNext() && enumx.MoveNext())
            {
                do
                {
                    if (enumi.Current == ++i)
                    {
                        yield return enumx.Current;
                        if (!enumi.MoveNext())
                            break;
                    }
                } while (enumx.MoveNext());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public static Matrix Slice(this Matrix x, IEnumerable<int> indices)
        {
            var q = indices.Distinct()
                           .OrderBy(k => k);
            var count = q.Count();
            if (count == 0)
                throw new InvalidOperationException("Cannot slice a a matrix with no valid indices");

            Matrix m = (Matrix)x.CreateMatrix(count, x.ColumnCount);
            int i = -1;
            foreach (int j in q)
                m.SetRow(++i, x.Row(j));
            return m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public static Vector Slice(this Vector x, IEnumerable<int> indices)
        {
            var q = indices.Distinct()
                           .OrderBy(k => k);
            var count = q.Count();
            if (count == 0)
                throw new InvalidOperationException("Cannot slice a a matrix with no valid indices");

            Vector v = (Vector)x.CreateVector(count);
            int i = -1;
            foreach (int j in q)
                v[++i] = x[j];
            return v;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IEnumerable<int> Indices(this IEnumerable<double> x, Func<double, bool> where)
        {
            int i = -1;
            foreach (double d in x)
            {
                i++;
                if (where(d))
                    yield return i;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<int> Indices(this Matrix x, Func<Vector, bool> f)
        {
            foreach (var v in x.RowEnumerator())
                if (f((Vector)v.Item2))
                    yield return v.Item1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="segments"></param>
        /// <returns></returns>
        public static Range[] Segment(this IEnumerable<double> x, int segments)
        {
            if (segments < 2)
                throw new InvalidOperationException("Invalid Segment Length, must > 1");

            // list max and min
            var max = x.Max();
            var min = x.Min();

            // range size
            var range = (max - min) / segments;

            // create range array
            var ranges = new Range[segments];

            // first element starts with min
            ranges[0] = Range.Make(min, min + range);

            // each subsequent element is max
            // of previous and max of previous
            // plus the appropriate range
            for (int i = 1; i < segments; i++)
                ranges[i] = Range.Make(ranges[i - 1].Max, ranges[i - 1].Max + range);

            // make last range slightly larger 
            // to maintain r.Min <= d < r.Max
            ranges[ranges.Length - 1].Max += .01;

            return ranges;
        }

    }
}
