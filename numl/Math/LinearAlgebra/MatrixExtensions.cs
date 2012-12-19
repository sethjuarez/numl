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
using numl.Math.Information;

namespace numl.Math.LinearAlgebra
{
    public static class MatrixExtensions
    {
        public static Matrix Stack(this Matrix m, Matrix t)
        {
            if (m.Cols != t.Cols)
                throw new InvalidOperationException("Invalid dimension for stack operation!");

            Matrix p = new Matrix(m.Rows + t.Rows, t.Cols);
            for (int i = 0; i < p.Rows; i++)
            {
                for (int j = 0; j < p.Cols; j++)
                {
                    if (i < m.Rows)
                        p[i, j] = m[i, j];
                    else
                        p[i, j] = t[i - m.Rows, j];
                }
            }

            return p;
        }

        public static Matrix VStack(this Matrix m, Matrix t)
        {
            if (m.Rows != t.Rows)
                throw new InvalidOperationException("Invalid dimension for stack operation!");

            Matrix p = new Matrix(m.Rows, m.Cols + t.Cols);
            for (int i = 0; i < p.Rows; i++)
            {
                for (int j = 0; j < p.Cols; j++)
                {
                    if (j < m.Cols)
                        p[i, j] = m[i, j];
                    else
                        p[i, j] = t[i, j - m.Cols];
                }
            }

            return p;
        }

        public static Matrix Cholesky(this Matrix m)
        {
            return Matrix.Cholesky(m);
        }

        public static Matrix Round(this Matrix m, int decimals = 0)
        {
            return Matrix.Round(m, decimals);
        }

        public static double Norm(this Matrix m)
        {
            return Matrix.Norm(m);
        }

        public static Tuple<Vector, Matrix> Eigs(this Matrix m)
        {
            return Matrix.Eigs(m);
        }

        public static Tuple<Matrix, Vector, Matrix> SVD(this Matrix m)
        {
            return Matrix.SVD(m);
        }

        public static Vector Mean(this Matrix source, VectorType t)
        {
            int count = t == VectorType.Row ? source.Cols : source.Rows;
            VectorType type = t == VectorType.Row ? VectorType.Column : VectorType.Row;
            Vector v = new Vector(count);
            for (int i = 0; i < count; i++)
                v[i] = source[i, type].Mean();
            return v;
        }

        public static double Max(this Matrix source)
        {
            double max = double.MinValue;
            for (int i = 0; i < source.Rows; i++)
                for (int j = 0; j < source.Cols; j++)
                    if (source[i, j] > max)
                        max = source[i, j];

            return max;
        }

        public static double Min(this Matrix source)
        {
            double min = double.MaxValue;
            for (int i = 0; i < source.Rows; i++)
                for (int j = 0; j < source.Cols; j++)
                    if (source[i, j] < min)
                        min = source[i, j];

            return min;
        }

        public static Matrix Covariance(this Matrix source, VectorType t = VectorType.Column)
        {
            int length = t == VectorType.Row ? source.Rows : source.Cols;
            Matrix m = new Matrix(length);
            for (int i = 0; i < length; i++)
                for (int j = i; j < length; j++) // symmetric matrix
                    m[i, j] = m[j, i] = source[i, t].Covariance(source[j, t]);
            return m;
        }

        public static Vector CovarianceDiag(this Matrix source, VectorType t = VectorType.Column)
        {
            int length = t == VectorType.Row ? source.Rows : source.Cols;
            Vector vector = new Vector(length);
            for (int i = 0; i < length; i++)
                vector[i] = source[i, t].Variance();
            return vector;
        }

        public static Matrix Correlation(this Matrix source, VectorType t = VectorType.Column)
        {
            int length = t == VectorType.Row ? source.Rows : source.Cols;
            Matrix m = new Matrix(length);
            for (int i = 0; i < length; i++)
                for (int j = i; j < length; j++) // symmetric matrix
                    m[i, j] = m[j, i] = source[i, t].Correlation(source[j, t]);
            return m;
        }

        public static IEnumerable<Vector> Reverse(this Matrix source, VectorType t = VectorType.Row)
        {
            int length = t == VectorType.Row ? source.Rows : source.Cols;
            for (int i = length - 1; i > -1; i--)
                yield return source[i, t];
        }

        public static IEnumerable<int> Indices(this Matrix source, Func<Vector, bool> f)
        {
            return MatrixExtensions.Indices(source, f, VectorType.Row);
        }

        public static IEnumerable<int> Indices(this Matrix source, Func<Vector, bool> f, VectorType t)
        {
            int max = t == VectorType.Row ? source.Rows : source.Cols;
            for (int i = 0; i < max; i++)
                if (f(source[i, t]))
                    yield return i;
        }

        public static Matrix Slice(this Matrix m, IEnumerable<int> indices)
        {
            return MatrixExtensions.Slice(m,indices, VectorType.Row);
        }

        public static Matrix Slice(this Matrix m, IEnumerable<int> indices, VectorType t)
        {
            var q = indices.Distinct();

            int rows = t == VectorType.Row ? q.Where(j => j < m.Rows).Count() : m.Rows;
            int cols = t == VectorType.Column ? q.Where(j => j < m.Cols).Count() : m.Cols;

            Matrix n = new Matrix(rows, cols);

            int i = -1;
            foreach (var j in q.OrderBy(k => k))
                n[++i, t] = m[j, t];

            return n;
        }

        public static Matrix Extract(this Matrix m, int x, int y, int width, int height, bool safe = true)
        {
            Matrix m2 = Matrix.Zeros(height, width);
            for (int i = y; i < y + height; i++)
                for (int j = x; j < x + width; j++)
                    if (safe && i < m.Rows && j < m.Cols)
                        m2[i - y, j - x] = m[i, j];

            return m2;
        }

        public static Matrix PolyKernel(this Matrix m, double d)
        {
            var K = new Matrix(m.Rows, m.Rows);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Rows; j++)
                    K[i, j] = System.Math.Pow((1 + m[i, VectorType.Row].Dot(m[j, VectorType.Row])), d);

            return K;
        }

        public static Vector PolyKernel(this Matrix m, Vector x, double d)
        {
            var K = Vector.Zeros(m.Rows);
            for (int i = 0; i < K.Length; i++)
                K[i] = System.Math.Pow((1 + m[i, VectorType.Row].Dot(x)), d);

            return K;
        }

        public static Matrix RBFKernel(this Matrix m, double sigma)
        {
            var K = new Matrix(m.Rows, m.Rows);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Rows; j++)
                {
                    var xy = -1 * (m[i, VectorType.Row] - m[j, VectorType.Row])
                                    .Dot(m[i, VectorType.Row] - m[j, VectorType.Row]);

                    K[i, j] = System.Math.Exp(xy / (2 * System.Math.Pow(sigma, 2)));
                }

            return K;
        }

        public static Vector RBFKernel(this Matrix m, Vector x, double sigma)
        {
            var K = Vector.Zeros(m.Rows);
            for (int i = 0; i < K.Length; i++)
            {
                var xy = -1 * (m[i, VectorType.Row] - x)
                                    .Dot(m[i, VectorType.Row] - x);

                K[i] = System.Math.Exp(xy / (2 * System.Math.Pow(sigma, 2)));
            }
            return K;
        }
    }
}
