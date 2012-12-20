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
            return Matrix.Stack(m, t);
        }

        public static Matrix VStack(this Matrix m, Matrix t)
        {
            return Matrix.VStack(m, t);
        }

        public static Vector Mean(this Matrix source, VectorType t)
        {
            return Matrix.Mean(source, t);
        }

        public static double Max(this Matrix source)
        {
            return Matrix.Max(source);
        }

        public static double Min(this Matrix source)
        {
            return Matrix.Min(source);
        }

        public static Matrix Covariance(this Matrix source, VectorType t = VectorType.Column)
        {
            return Matrix.Covariance(source, t);
        }

        public static Vector CovarianceDiag(this Matrix source, VectorType t = VectorType.Column)
        {
            return Matrix.CovarianceDiag(source, t);
        }

        public static Matrix Correlation(this Matrix source, VectorType t = VectorType.Column)
        {
            return Matrix.Correlation(source, t);
        }

        public static IEnumerable<Vector> Reverse(this Matrix source, VectorType t = VectorType.Row)
        {
            return Matrix.Reverse(source, t);
        }

        public static IEnumerable<int> Indices(this Matrix source, Func<Vector, bool> f)
        {
            return Matrix.Indices(source, f);
        }

        public static IEnumerable<int> Indices(this Matrix source, Func<Vector, bool> f, VectorType t)
        {
            return Matrix.Indices(source, f, t);
        }

        public static Matrix Slice(this Matrix m, IEnumerable<int> indices)
        {
            return Matrix.Slice(m, indices);
        }

        public static Matrix Slice(this Matrix m, IEnumerable<int> indices, VectorType t)
        {
            return Matrix.Slice(m, indices, t);
        }

        public static Matrix Extract(this Matrix m, int x, int y, int width, int height, bool safe = true)
        {
            return Matrix.Extract(m, x, y, width, height, safe);
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
            return Matrix.Evd(m);
        }

        public static Tuple<Matrix, Vector, Matrix> SVD(this Matrix m)
        {
            return Matrix.SVD(m);
        }

        /// <summary>
        /// Computes the trace of a matrix
        /// </summary>
        /// <returns>trace</returns>
        public static double Trace(this Matrix m)
        {
            return Matrix.Trace(m);
        }

        public static double Sum(this Matrix m)
        {
            return Matrix.Sum(m);
        }


        /// <summary>
        /// Computes the sum of either the rows 
        /// or columns of a matrix and returns
        /// a vector
        /// </summary>
        /// <param name="t">Row or Column sum</param>
        /// <returns>Vector Sum</returns>
        public static Vector Sum(this Matrix m, VectorType t)
        {
            return Matrix.Sum(m, t);
        }

        public static double Sum(Matrix m, int i, VectorType t)
        {
            return Matrix.Sum(m, i, t);
        }

        public static Vector Diag(this Matrix m)
        {
            return Matrix.Diag(m);
        }

    }
}
