// file:	Math\LinearAlgebra\MatrixExtensions.cs
//
// summary:	Implements the matrix extensions class
using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.LinearAlgebra
{
    /// <summary>A matrix extensions.</summary>
    public static class MatrixExtensions
    {
        /// <summary>A Matrix extension method that stacks.</summary>
        /// <param name="m">Matrix.</param>
        /// <param name="t">Row or Column sum.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Stack(this Matrix m, Matrix t)
        {
            return Matrix.Stack(m, t);
        }
        /// <summary>A Matrix extension method that stacks.</summary>
        /// <param name="m">Matrix.</param>
        /// <param name="t">Row or Column sum.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix VStack(this Matrix m, Matrix t)
        {
            return Matrix.VStack(m, t);
        }
        /// <summary>
        /// A Matrix extension method that determines the mean of the given parameters.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="t">Row or Column sum.</param>
        /// <returns>The mean value.</returns>
        public static Vector Mean(this Matrix source, VectorType t)
        {
            return Matrix.Mean(source, t);
        }

        /// <summary>
        /// Computes the standard deviation of the given matrix
        /// </summary>
        /// <param name="source"></param>
        /// <param name="t">Return a Row or Column vector</param>
        /// <returns></returns>
        public static Vector StdDev(this Matrix source, VectorType t)
        {
            return Matrix.StdDev(source, t);
        }

        /// <summary>
        /// A Matrix extension method that determines the maximum of the given parameters.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>The maximum value.</returns>
        public static double Max(this Matrix source)
        {
            return Matrix.Max(source);
        }
        /// <summary>
        /// A Matrix extension method that determines the minimum of the given parameters.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <returns>The minimum value.</returns>
        public static double Min(this Matrix source)
        {
            return Matrix.Min(source);
        }
        /// <summary>A Matrix extension method that covariances.</summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="t">(Optional) Row or Column sum.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Covariance(this Matrix source, VectorType t = VectorType.Col)
        {
            return Matrix.Covariance(source, t);
        }
        /// <summary>A Matrix extension method that covariance diagram.</summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="t">(Optional) Row or Column sum.</param>
        /// <returns>A Vector.</returns>
        public static Vector CovarianceDiag(this Matrix source, VectorType t = VectorType.Col)
        {
            return Matrix.CovarianceDiag(source, t);
        }
        /// <summary>A Matrix extension method that correlations.</summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="t">(Optional) Row or Column sum.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Correlation(this Matrix source, VectorType t = VectorType.Col)
        {
            return Matrix.Correlation(source, t);
        }
        /// <summary>Enumerates reverse in this collection.</summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="t">(Optional) Row or Column sum.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process reverse in this collection.
        /// </returns>
        public static IEnumerable<Vector> Reverse(this Matrix source, VectorType t = VectorType.Row)
        {
            return Matrix.Reverse(source, t);
        }
        /// <summary>Enumerates indices in this collection.</summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="f">The Func&lt;Vector,bool&gt; to process.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process indices in this collection.
        /// </returns>
        public static IEnumerable<int> Indices(this Matrix source, Func<Vector, bool> f)
        {
            return Matrix.Indices(source, f);
        }
        /// <summary>Enumerates indices in this collection.</summary>
        /// <param name="source">The source to act on.</param>
        /// <param name="f">The Func&lt;Vector,bool&gt; to process.</param>
        /// <param name="t">Row or Column sum.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process indices in this collection.
        /// </returns>
        public static IEnumerable<int> Indices(this Matrix source, Func<Vector, bool> f, VectorType t)
        {
            return Matrix.Indices(source, f, t);
        }
        /// <summary>A Matrix extension method that slices.</summary>
        /// <param name="m">Matrix.</param>
        /// <param name="indices">The indices.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Slice(this Matrix m, IEnumerable<int> indices)
        {
            return Matrix.Slice(m, indices);
        }
        /// <summary>A Matrix extension method that slices.</summary>
        /// <param name="m">Matrix.</param>
        /// <param name="indices">The indices.</param>
        /// <param name="t">Row or Column sum.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Slice(this Matrix m, IEnumerable<int> indices, VectorType t)
        {
            return Matrix.Slice(m, indices, t);
        }
        /// <summary>A Matrix extension method that extracts this object.</summary>
        /// <param name="m">Matrix.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="safe">(Optional) true to safe.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Extract(this Matrix m, int x, int y, int width, int height, bool safe = true)
        {
            return Matrix.Extract(m, x, y, width, height, safe);
        }
        /// <summary>A Matrix extension method that choleskies the given m.</summary>
        /// <param name="m">Matrix.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Cholesky(this Matrix m)
        {
            return Matrix.Cholesky(m);
        }
        /// <summary>A Matrix extension method that rounds.</summary>
        /// <param name="m">Matrix.</param>
        /// <param name="decimals">(Optional) the decimals.</param>
        /// <returns>A Matrix.</returns>
        public static Matrix Round(this Matrix m, int decimals = 0)
        {
            return Matrix.Round(m, decimals);
        }
        /// <summary>A Matrix extension method that normals.</summary>
        /// <param name="m">Matrix.</param>
        /// <returns>A double.</returns>
        public static double Norm(this Matrix m)
        {
            return Matrix.Norm(m, 2);
        }
        /// <summary>A Matrix extension method that normals.</summary>
        /// <param name="m">Matrix.</param>
        /// <param name="p">The double to process.</param>
        /// <returns>A double.</returns>
        public static double Norm(this Matrix m, double p)
        {
            return Matrix.Norm(m, p);
        }
        /// <summary>A Matrix extension method that eigs the given m.</summary>
        /// <param name="m">Matrix.</param>
        /// <returns>A Tuple&lt;Vector,Matrix&gt;</returns>
        public static Tuple<Vector, Matrix> Eigs(this Matrix m)
        {
            return Matrix.Evd(m);
        }
        /// <summary>A Matrix extension method that svds the given m.</summary>
        /// <param name="m">Matrix.</param>
        /// <returns>A Tuple&lt;Matrix,Vector,Matrix&gt;</returns>
        public static Tuple<Matrix, Vector, Matrix> SVD(this Matrix m)
        {
            return Matrix.SVD(m);
        }
        /// <summary>Computes the trace of a matrix.</summary>
        /// <param name="m">Matrix.</param>
        /// <returns>trace.</returns>
        public static double Trace(this Matrix m)
        {
            return Matrix.Trace(m);
        }
        /// <summary>
        /// Computes the sum of either the rows or columns of a matrix and returns a vector.
        /// </summary>
        /// <param name="m">Matrix.</param>
        /// <returns>Vector Sum.</returns>
        public static double Sum(this Matrix m)
        {
            return Matrix.Sum(m);
        }
        /// <summary>
        /// Computes the sum of either the rows or columns of a matrix and returns a vector.
        /// </summary>
        /// <param name="m">Matrix.</param>
        /// <param name="t">Row or Column sum.</param>
        /// <returns>Vector Sum.</returns>
        public static Vector Sum(this Matrix m, VectorType t)
        {
            return Matrix.Sum(m, t);
        }
        /// <summary>
        /// Computes the sum of either the rows or columns of a matrix and returns a vector.
        /// </summary>
        /// <param name="m">Matrix.</param>
        /// <param name="i">Zero-based index of the.</param>
        /// <param name="t">Row or Column sum.</param>
        /// <returns>Vector Sum.</returns>
        public static double Sum(Matrix m, int i, VectorType t)
        {
            return Matrix.Sum(m, i, t);
        }
        /// <summary>A Matrix extension method that diagrams the given m.</summary>
        /// <param name="m">Matrix.</param>
        /// <returns>A Vector.</returns>
        public static Vector Diag(this Matrix m)
        {
            return Matrix.Diag(m);
        }
        /// <summary>A Matrix extension method that statistics.</summary>
        /// <param name="m">Matrix.</param>
        /// <returns>A Matrix[].</returns>
        public static Matrix[] Stats(this Matrix m)
        {
            return Matrix.Stats(m, VectorType.Row);
        }
        /// <summary>A Matrix extension method that statistics.</summary>
        /// <param name="m">Matrix.</param>
        /// <param name="t">Row or Column sum.</param>
        /// <returns>A Matrix[].</returns>
        public static Matrix[] Stats(this Matrix m, VectorType t)
        {
            return Matrix.Stats(m, t);
        }
        /// <summary>
        /// computes matrix determinant NOTE: currently using cholesky factorization to save time so non
        /// symmetric positive semi-definite matrices will cause problems...
        /// </summary>
        /// <param name="m">Matrix.</param>
        /// <returns>Determinant.</returns>
        public static double Det(this Matrix m)
        {
            return Matrix.Det(m);
        }
    }
}
