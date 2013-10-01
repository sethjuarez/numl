using System;
using System.Linq;
using System.Collections.Generic;

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

        public static Matrix Covariance(this Matrix source, VectorType t = VectorType.Col)
        {
            return Matrix.Covariance(source, t);
        }

        public static Vector CovarianceDiag(this Matrix source, VectorType t = VectorType.Col)
        {
            return Matrix.CovarianceDiag(source, t);
        }

        public static Matrix Correlation(this Matrix source, VectorType t = VectorType.Col)
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
            return Matrix.Norm(m, 2);
        }

        public static double Norm(this Matrix m, double p)
        {
            return Matrix.Norm(m, p);
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

        public static Matrix[] Stats(this Matrix m)
        {
            return Matrix.Stats(m, VectorType.Row);
        }

        public static Matrix[] Stats(this Matrix m, VectorType t)
        {
            return Matrix.Stats(m, t);
        }

        /// <summary>
        /// computes matrix determinant
        /// NOTE: currently using cholesky factorization
        /// to save time so non symmetric positive semi-definite
        /// matrices will cause problems...
        /// </summary>
        /// <param name="m">Matrix</param>
        /// <returns>Determinant</returns>
        public static double Det(this Matrix m)
        {
            return Matrix.Det(m);
        }
    }
}
