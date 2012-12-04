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
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace numl.Utils
{
    public static class MathConversion
    {
        public static Vector ToVector(this IEnumerable<double> vector)
        {
            var v = vector.ToArray();

            // check sparsity
            var percent = (decimal)v.Count(d => d == 0) /
                          (decimal)v.Count();

            // more than half zero's? (maybe have this as a setting?)
            if (percent > .5m)
                return new SparseVector(v);
            else
                return new DenseVector(v);
        }

        private static Matrix Build(double[][] x, bool clip = false)
        {
            // rows
            int n = x.Length;
            if (n == 0)
                throw new InvalidOperationException("Empty matrix (n)");

            // cols (being nice here...)
            var cols = x.Select(v => v.Length);
            int d = cols.Max();

            if (d == 0)
                throw new InvalidOperationException("Empty matrix (d)");

            // total zeros in matrix
            var zeros = (from v in x select v.Count(i => i == 0)).Sum();

            // if irregularities in jagged matrix, need to 
            // pad rows with less columns with additional
            // zeros by subtractic max width with each
            // individual row and getting the sum
            var pad = cols.Select(c => d - c).Sum();

            // check sparsity
            var percent = (decimal)(zeros + pad) / (decimal)(n * d);

            Matrix m;
            if (percent > .5m)
                m = new SparseMatrix(n, clip ? d - 1 : d);
            else
                m = new DenseMatrix(n, clip ? d - 1 : d);

            return m;
        }

        public static Matrix ToMatrix(this IEnumerable<IEnumerable<double>> matrix)
        {
            // materialize
            double[][] x = (from v in matrix select v.ToArray()).ToArray();

            // determine matrix
            // size and type
            var m = Build(x);

            // fill 'er up!
            for (int i = 0; i < m.RowCount; i++)
                for (int j = 0; j < m.ColumnCount; j++)
                    if (j >= x[i].Length)  // over bound limits
                        m[i, j] = 0;       // pad overlow to 0
                    else
                        m[i, j] = x[i][j];

            return m;
        }

        public static Tuple<Matrix, Vector> ToExamples(this IEnumerable<IEnumerable<double>> matrix)
        {
            // materialize
            double[][] x = (from v in matrix select v.ToArray()).ToArray();

            // determine matrix
            // size and type
            var m = Build(x, true); // clip last col

            // fill 'er up!
            for (int i = 0; i < m.RowCount; i++)
                for (int j = 0; j < m.ColumnCount; j++)
                    if (j >= x[i].Length)  // over bound limits
                        m[i, j] = 0;       // pad overlow to 0
                    else
                        m[i, j] = x[i][j];

            // fill up vector
            DenseVector y = new DenseVector(m.RowCount);
            for (int i = 0; i < m.RowCount; i++)
                if (m.ColumnCount >= x[i].Length)
                    y[i] = 0;              // pad overflow to 0
                else
                    y[i] = x[i][m.ColumnCount];

            return new Tuple<Matrix, Vector>(m, y);

        }
    }
}
