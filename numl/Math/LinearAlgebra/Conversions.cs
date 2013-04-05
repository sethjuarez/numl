using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.LinearAlgebra
{
    public static class Conversions
    {
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
            //var percent = (decimal)(zeros + pad) / (decimal)(n * d);

            Matrix m = Matrix.Zeros(n, clip ? d - 1 : d);

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
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
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
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    if (j >= x[i].Length)  // over bound limits
                        m[i, j] = 0;       // pad overlow to 0
                    else
                        m[i, j] = x[i][j];

            // fill up vector
            Vector y = Vector.Zeros(m.Rows);
            for (int i = 0; i < m.Rows; i++)
                if (m.Cols >= x[i].Length)
                    y[i] = 0;              // pad overflow to 0
                else
                    y[i] = x[i][m.Cols];

            return new Tuple<Matrix, Vector>(m, y);

        }
    }
}
