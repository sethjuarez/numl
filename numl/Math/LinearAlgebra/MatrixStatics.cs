using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace numl.Math.LinearAlgebra
{
    public partial class Matrix
    {
        /// <summary>
        /// Computes the trace of a matrix
        /// </summary>
        /// <returns>trace</returns>
        public static double Trace(Matrix m)
        {
            double t = 0;
            for (int i = 0; i < m.Rows && i < m.Cols; i++)
                t += m[i, i];
            return t;
        }

        /// <summary>
        /// Computes the sum of every element of the matrix
        /// </summary>
        /// <returns>sum</returns>
        public static double Sum(Matrix m)
        {
            double sum = 0;
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    sum += m[i, j];
            return sum;
        }

        /// <summary>
        /// Computes the sum of either the rows 
        /// or columns of a matrix and returns
        /// a vector
        /// </summary>
        /// <param name="t">Row or Column sum</param>
        /// <returns>Vector Sum</returns>
        public static Vector Sum(Matrix m, VectorType t)
        {
            if (t == VectorType.Row)
            {
                Vector result = new Vector(m.Cols);
                for (int i = 0; i < m.Cols; i++)
                    for (int j = 0; j < m.Rows; j++)
                        result[i] += m[j, i];
                return result;
            }
            else
            {
                Vector result = new Vector(m.Rows);
                for (int i = 0; i < m.Rows; i++)
                    for (int j = 0; j < m.Cols; j++)
                        result[i] += m[i, j];
                return result;
            }
        }

        public static double Sum(Matrix m, int i, VectorType t)
        {
            return m[i, t].Sum();
        }

        /// <summary>
        /// Standard Matrix Norm
        /// </summary>
        /// <param name="A">Input Matrix</param>
        /// <returns>Standard Norm (double)</returns>
        public static double Norm(Matrix A, double p)
        {
            double norm = 0;
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < A.Cols; j++)
                    norm += System.Math.Pow(System.Math.Abs(A[i, j]), p);
            return System.Math.Pow(norm, 1d/p);
        }

        /// <summary>
        /// Matrix Frobenius Norm
        /// </summary>
        /// <param name="A">Input Matrix</param>
        /// <returns>Frobenius Norm (double)</returns>
        public static double FrobeniusNorm(Matrix A)
        {
            return System.Math.Sqrt((A.T * A).Trace());
        }

        /// <summary>
        /// Eigen Decomposition
        /// </summary>
        /// <param name="A">Input Matrix</param>
        /// <returns>Tuple(Eigen Values, Eigen Vectors)</returns>
        public static Tuple<Vector, Matrix> Evd(Matrix A)
        {
            Evd eigs = new Evd(A);
            eigs.compute();
            return new Tuple<Vector, Matrix>(eigs.Eigenvalues, eigs.Eigenvectors);
        }

        /// <summary>
        /// Singular Value Decomposition
        /// </summary>
        /// <param name="A">Input Matrix</param>
        /// <returns>Tuple(Matrix U, Vector S, Matrix V)</returns>
        public static Tuple<Matrix, Vector, Matrix> SVD(Matrix A)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT IMPLEMENTED!
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static Tuple<Matrix, Matrix> LU(Matrix A)
        {
            // TODO: FINISH ALGORITHM
            if (A.Rows != A.Cols)
                throw new InvalidOperationException("Factorization requires a symmetric positive semidefinite matrix!");

            int n = A.Rows;

            Matrix L = Matrix.Identity(n);
            Matrix U = Matrix.Zeros(n);
            Vector v = Vector.Zeros(n);
            for (int j = 0; j < n; j++)
            {
                if (j == 0)
                {
                    for (int i = j; i < n; i++)
                        v[i] = A[i, j];
                }
                else
                {

                }

                if (j < n)
                {

                }

                U[j, j] = v[j];
            }

            int q = 0;
            if (q < 1)
                throw new NotImplementedException("Not quite done yet ;)");

            return new Tuple<Matrix, Matrix>(L, U);
        }

        /// <summary>
        /// Cholesky Factorization of a Matrix
        /// </summary>
        /// <param name="m">Input Matrix</param>
        /// <returns>Cholesky Faxtorization (R.T would be other matrix)</returns>
        public static Matrix Cholesky(Matrix m)
        {
            if (m.Rows != m.Cols)
                throw new InvalidOperationException("Factorization requires a symmetric positive semi-definite matrix!");

            int n = m.Rows;
            Matrix A = m.Copy();

            for (int k = 0; k < n; k++)
            {
                if (A[k, k] <= 0)
                    throw new SingularMatrixException("Matrix is not symmetric positive semi-definite!");

                A[k, k] = System.Math.Sqrt(A[k, k]);
                for (int j = k + 1; j < n; j++)
                    A[j, k] = A[j, k] / A[k, k];

                for (int j = k + 1; j < n; j++)
                    for (int i = j; i < n; i++)
                        A[i, j] = A[i, j] - A[i, k] * A[j, k];
            }

            // put back zeros...
            for (int i = 0; i < n; i++)
                for (int j = i + 1; j < n; j++)
                    A[i, j] = 0;

            return A;
        }

        /// <summary>
        /// Matrix Roundoff
        /// </summary>
        /// <param name="m">Input Matrix</param>
        /// <param name="decimals">Max number of decimals (default 0 - integral members)</param>
        /// <returns>Rounded Matrix</returns>
        public static Matrix Round(Matrix m, int decimals = 0)
        {
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    m[i, j] = System.Math.Round(m[i, j], decimals);
            return m;
        }

        /// <summary>
        /// Modified Gram-Schmidt QR Factorization
        /// </summary>
        /// <param name="A">Matrix A</param>
        /// <returns>Tuple(Q, R)</returns>
        public static Tuple<Matrix, Matrix> QR(Matrix A)
        {
            int n = A.Rows;
            Matrix R = Matrix.Zeros(n);
            Matrix Q = A.Copy();
            for (int k = 0; k < n; k++)
            {
                R[k, k] = Q[k, VectorType.Col].Norm();
                Q[k, VectorType.Col] = Q[k, VectorType.Col] / R[k, k];

                for (int j = k + 1; j < n; j++)
                {
                    R[k, j] = Vector.Dot(Q[k, VectorType.Col], A[j, VectorType.Col]);
                    Q[j, VectorType.Col] = Q[j, VectorType.Col] - (R[k, j] * Q[k, VectorType.Col]);
                }
            }

            return new Tuple<Matrix, Matrix>(Q, R);
        }

        internal static Vector Backward(Matrix A, Vector b)
        {
            Vector x = Vector.Zeros(b.Length);
            for (int i = b.Length - 1; i > -1; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < b.Length; j++)
                    sum += A[i, j] * x[j];

                x[i] = (b[i] - sum) / A[i, i];
            }

            return x;
        }

        internal static Vector Forward(Matrix A, Vector b)
        {
            Vector x = Vector.Zeros(b.Length);
            for (int i = 0; i < b.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < i; j++)
                    sum += A[i, j] * x[j];

                x[i] = (b[i] - sum) / A[i, i];
            }

            return x;
        }

        /// <summary>
        /// Dot product between a matrix and a vector
        /// </summary>
        /// <param name="x">Matrix x</param>
        /// <param name="v">Vector v</param>
        /// <returns>Vector dot product</returns>
        public static Vector Dot(Matrix x, Vector v)
        {
            if (v.Length != x.Cols)
                throw new InvalidOperationException("objects are not aligned");

            Vector toReturn = Vector.Zeros(x.Rows);
            for (int i = 0; i < toReturn.Length; i++)
                toReturn[i] = Vector.Dot(x[i, VectorType.Row], v);
            return toReturn;
        }

        public static Vector Dot(Vector v, Matrix x)
        {
            if (v.Length != x.Rows)
                throw new InvalidOperationException("objects are not aligned");

            Vector toReturn = Vector.Zeros(x.Cols);
            for (int i = 0; i < toReturn.Length; i++)
                toReturn[i] = Vector.Dot(x[i, VectorType.Col], v);
            return toReturn;
        }


        public static Vector Mean(Matrix source, VectorType t)
        {
            int count = t == VectorType.Row ? source.Cols : source.Rows;
            VectorType type = t == VectorType.Row ? VectorType.Col : VectorType.Row;
            Vector v = new Vector(count);
            for (int i = 0; i < count; i++)
                v[i] = source[i, type].Mean();
            return v;
        }

        public static double Max(Matrix source)
        {
            double max = double.MinValue;
            for (int i = 0; i < source.Rows; i++)
                for (int j = 0; j < source.Cols; j++)
                    if (source[i, j] > max)
                        max = source[i, j];

            return max;
        }

        public static double Min(Matrix source)
        {
            double min = double.MaxValue;
            for (int i = 0; i < source.Rows; i++)
                for (int j = 0; j < source.Cols; j++)
                    if (source[i, j] < min)
                        min = source[i, j];

            return min;
        }

        public static Matrix Covariance(Matrix source, VectorType t = VectorType.Col)
        {
            int length = t == VectorType.Row ? source.Rows : source.Cols;
            Matrix m = new Matrix(length);
            //for (int i = 0; i < length; i++)
            Parallel.For(0, length, i =>
                //for (int j = i; j < length; j++) // symmetric matrix
                Parallel.For(i, length, j =>
                    m[i, j] = m[j, i] = source[i, t].Covariance(source[j, t])));
            return m;
        }

        public static Vector CovarianceDiag(Matrix source, VectorType t = VectorType.Col)
        {
            int length = t == VectorType.Row ? source.Rows : source.Cols;
            Vector vector = new Vector(length);
            for (int i = 0; i < length; i++)
                vector[i] = source[i, t].Variance();
            return vector;
        }

        public static Matrix Correlation(Matrix source, VectorType t = VectorType.Col)
        {
            int length = t == VectorType.Row ? source.Rows : source.Cols;
            Matrix m = new Matrix(length);
            for (int i = 0; i < length; i++)
                for (int j = i; j < length; j++) // symmetric matrix
                    m[i, j] = m[j, i] = source[i, t].Correlation(source[j, t]);
            return m;
        }

        public static IEnumerable<Vector> Reverse(Matrix source, VectorType t = VectorType.Row)
        {
            int length = t == VectorType.Row ? source.Rows : source.Cols;
            for (int i = length - 1; i > -1; i--)
                yield return source[i, t];
        }

        public static IEnumerable<int> Indices(Matrix source, Func<Vector, bool> f)
        {
            return Matrix.Indices(source, f, VectorType.Row);
        }

        public static IEnumerable<int> Indices(Matrix source, Func<Vector, bool> f, VectorType t)
        {
            int max = t == VectorType.Row ? source.Rows : source.Cols;
            for (int i = 0; i < max; i++)
                if (f(source[i, t]))
                    yield return i;
        }


        //---------------- structural
        /// <summary>
        /// Stack a set of vectors into a matrix
        /// </summary>
        /// <param name="type"></param>
        /// <param name="vectors"></param>
        /// <returns></returns>
        internal static Matrix Stack(VectorType type, params Vector[] vectors)
        {
            if (vectors.Length == 0)
                throw new InvalidOperationException("Cannot construct Matrix from empty vector set!");

            if (!vectors.All(v => v.Length == vectors[0].Length))
                throw new InvalidOperationException("Vectors must all be of the same length!");

            int n = type == VectorType.Row ? vectors.Length : vectors[0].Length;
            int d = type == VectorType.Row ? vectors[0].Length : vectors.Length;

            Matrix m = Matrix.Zeros(n, d);
            for (int i = 0; i < vectors.Length; i++)
                m[i, type] = vectors[i];

            return m;
        }

        public static Matrix Stack(params Vector[] vectors)
        {
            return Matrix.Stack(VectorType.Row, vectors);
        }

        public static Matrix VStack(params Vector[] vectors)
        {
            return Matrix.Stack(VectorType.Col, vectors);
        }

        public static Matrix Stack(Matrix m, Matrix t)
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

        public static Matrix VStack(Matrix m, Matrix t)
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

        public static Matrix Slice(Matrix m, IEnumerable<int> indices)
        {
            return MatrixExtensions.Slice(m, indices, VectorType.Row);
        }

        public static Matrix Slice(Matrix m, IEnumerable<int> indices, VectorType t)
        {
            var q = indices.Distinct();

            int rows = t == VectorType.Row ? q.Where(j => j < m.Rows).Count() : m.Rows;
            int cols = t == VectorType.Col ? q.Where(j => j < m.Cols).Count() : m.Cols;

            Matrix n = new Matrix(rows, cols);

            int i = -1;
            foreach (var j in q.OrderBy(k => k))
                n[++i, t] = m[j, t];

            return n;
        }

        public static Matrix Extract(Matrix m, int x, int y, int width, int height, bool safe = true)
        {
            Matrix m2 = Matrix.Zeros(height, width);
            for (int i = y; i < y + height; i++)
                for (int j = x; j < x + width; j++)
                    if (safe && i < m.Rows && j < m.Cols)
                        m2[i - y, j - x] = m[i, j];

            return m2;
        }

        public static Vector Diag(Matrix m)
        {
            var length = m.Cols > m.Rows ? m.Rows : m.Cols;
            var v = Vector.Zeros(length);
            for (int i = 0; i < length; i++)
                v[i] = m[i, i];
            return v;
        }
    }
}
