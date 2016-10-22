// file:	Math\LinearAlgebra\MatrixOps.cs
//
// summary:	Implements the matrix ops class
using System;
using System.Collections.Generic;

namespace numl.Math.LinearAlgebra
{
    /// <summary>A matrix.</summary>
    public partial class Matrix
    {
        // --------------------- implicity operators
        /// <summary>Matrix casting operator.</summary>
        /// <param name="m">Matrix.</param>
        public static implicit operator Matrix(double[,] m)
        {
            return new Matrix(m);
        }
        /// <summary>Matrix casting operator.</summary>
        /// <param name="m">Matrix.</param>
        public static implicit operator Matrix(int[,] m)
        {
            var rows = m.GetLength(0);
            var cols = m.GetLength(1);
            var matrix = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                matrix[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                    matrix[i][j] = m[i, j];
            }
            return matrix;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Double[][]"/> to <see cref="Matrix"/>.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Matrix(double[][] m)
        {
            return new Matrix(m);
        }

        // --------------------- mathematical ops
        /// <summary>Equality operator.</summary>
        /// <param name="m1">The first Matrix.</param>
        /// <param name="m2">The second Matrix.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator ==(Matrix m1, Matrix m2)
        {
            return (ReferenceEquals(m1, null) && ReferenceEquals(m2, null) || m1.Equals(m2));
        }
        /// <summary>Inequality operator.</summary>
        /// <param name="m1">The first Matrix.</param>
        /// <param name="m2">The second Matrix.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !m1.Equals(m2);
        }

        /// <summary>Addition operator.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="m1">The first Matrix.</param>
        /// <param name="m2">The second Matrix.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
                throw new InvalidOperationException("Dimensions do not match");

            var result = new double[m1.Rows][];
            for (int i = 0; i < m1.Rows; i++)
            {
                result[i] = new double[m1.Cols];
                for (int j = 0; j < m1.Cols; j++)
                    result[i][j] = m1[i, j] + m2[i, j];
            }

            return result;
        }
        /// <summary>Subtraction operator.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="m1">The first Matrix.</param>
        /// <param name="m2">The second Matrix.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
                throw new InvalidOperationException("Dimensions do not match");

            var result = new double[m1.Rows][];
            for (int i = 0; i < m1.Rows; i++)
            {
                result[i] = new double[m1.Cols];
                for (int j = 0; j < m1.Cols; j++)
                    result[i][j] = m1[i, j] - m2[i, j];
            }

            return result;
        }

        /// <summary>In memory addition of double to matrix.</summary>
        /// <param name="m">Matrix.</param>
        /// <param name="s">double.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix operator +(Matrix m, double s)
        {
            var result = new double[m.Rows][];
            for (int i = 0; i < m.Rows; i++)
            {
                result[i] = new double[m.Cols];
                for (int j = 0; j < m.Cols; j++)
                    result[i][j] = m[i][j] + s;
            }
            return result;
        }
        /// <summary>Addition operator.</summary>
        /// <param name="s">The double to process.</param>
        /// <param name="m">The Matrix to process.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix operator +(double s, Matrix m)
        {
            return m + s;
        }
        /// <summary>Subtract double from every element in the Matrix.</summary>
        /// <param name="m">Matrix.</param>
        /// <param name="s">Double.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix operator -(Matrix m, double s)
        {
            var result = new double[m.Rows][];
            for (int i = 0; i < m.Rows; i++)
            {
                result[i] = new double[m.Cols];
                for (int j = 0; j < m.Cols; j++)
                    result[i][j] = m[i, j] - s;
            }
            return result;
        }
        /// <summary>Subtraction operator.</summary>
        /// <param name="s">The double to process.</param>
        /// <param name="m">The Matrix to process.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix operator -(double s, Matrix m)
        {
            // subtracting matrix so every item
            // in matrix is negated and s is added
            return (-1 * m) + s;
        }
        /// <summary>matrix multiplication.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="m1">left hand side.</param>
        /// <param name="m2">right hand side.</param>
        /// <returns>matrix.</returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.Cols != m2.Rows)
                throw new InvalidOperationException("Invalid multiplication dimenstion");

            var result = new double[m1.Rows][];
            for (int i = 0; i < m1.Rows; i++)
            {
                result[i] = new double[m2.Cols];
                for (int j = 0; j < m2.Cols; j++)
                    for (int k = 0; k < m1.Cols; k++)
                        result[i][j] += m1[i, k] * m2[k, j];
            }

            return result;
        }
        /// <summary>Scalar matrix multiplication.</summary>
        /// <param name="s">scalar.</param>
        /// <param name="m">matrix.</param>
        /// <returns>matrix.</returns>
        public static Matrix operator *(double s, Matrix m)
        {
            var result = new double[m.Rows][];
            for (int i = 0; i < m.Rows; i++)
            {
                result[i] = new double[m.Cols];
                for (int j = 0; j < m.Cols; j++)
                    result[i][j] = s * m[i, j];
            }

            return result;
        }
        /// <summary>reverse.</summary>
        /// <param name="m">.</param>
        /// <param name="s">.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix operator *(Matrix m, double s)
        {
            return s * m;
        }
        /// <summary>Multiplication operator.</summary>
        /// <param name="m">The Matrix to process.</param>
        /// <param name="v">The Vector to process.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix operator *(Matrix m, Vector v)
        {
            Vector ans = Matrix.Dot(m, v);
            return ans.ToMatrix(VectorType.Col);
        }
        /// <summary>Multiplication operator.</summary>
        /// <param name="v">The Vector to process.</param>
        /// <param name="m">The Matrix to process.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix operator *(Vector v, Matrix m)
        {
            Vector ans = Matrix.Dot(v, m);
            return ans.ToMatrix(VectorType.Row);
        }
        /// <summary>
        /// Solves Ax = b for x If A is not square or the system is overdetermined, this operation solves
        /// the linear least squares A.T * A x = A.T * b.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="A">Matrix A.</param>
        /// <param name="b">Vector b.</param>
        /// <returns>x.</returns>
        public static Vector operator /(Matrix A, Vector b)
        {
            if (A.Rows != b.Length)
                throw new InvalidOperationException("Matrix row count does not match vector length!");

            // LLS
            if (A.Rows != A.Cols)
            {
                Matrix C = A.T * A;
                Matrix L = C.Cholesky();
                Vector d = (A.T * b).ToVector();
                Vector z = Matrix.Forward(L, d);
                Vector x = Matrix.Backward(L.T, z);
                return x;
            }
            // regular solve
            else
            {

                // need to be smarter here....
                return ((A ^ -1) * b).ToVector();
            }


        }
        /// <summary>Division operator.</summary>
        /// <param name="A">The Matrix to process.</param>
        /// <param name="b">The double to process.</param>
        /// <returns>The result of the operation.</returns>
        public static Matrix operator /(Matrix A, double b)
        {
            var result = new double[A.Rows][];
            for (int i = 0; i < A.Rows; i++)
            {
                result[i] = new double[A.Cols];
                for (int j = 0; j < A.Cols; j++)
                    result[i][j] = A[i, j] / b;
            }
            return result;
        }
        /// <summary>
        /// Matrix inverse using pivoted Gauss-Jordan elimination with partial pivoting
        /// See:http://www.cse.illinois.edu/iem/linear_equations/gauss_jordan/for python implementaion.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="mat">Matrix.</param>
        /// <param name="n">-1.</param>
        /// <returns>Inverse (or exception if matrix is singular)</returns>
        public static Matrix operator ^(Matrix mat, int n)
        {
            if (mat.Rows != mat.Cols)
                throw new InvalidOperationException("Can only find powers of square matrices!");

            if (n == 0)
                return Identity(mat.Rows, mat.Cols);
            if (n == 1)
                return mat.Copy();
            if (n == -1)
            {
                //warning, this is not the most efficient way to solve a system of Linear Equations. Consider using the \ operator
                return Inverse(mat);
            }
            var negative = n < 0;
            var pow = System.Math.Abs(n);
            var scratch = mat.Copy();
            for (int i = 0; i < pow; i++)
                scratch = scratch * mat;

            if (!negative)
                return scratch;
            else
            {
                //warning, this is not the most efficient way to solve a system of Linear Equations. Consider using the \ operator
                return Inverse(scratch);
            }
        }

        /// <summary>
        /// Creates an inverse of the current matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse()
        {
            return Inverse(this);
        }

        /// <summary>Inverses the given matrix.</summary>
        /// <exception cref="SingularMatrixException">Thrown when a Singular Matrix error condition occurs.</exception>
        /// <param name="mat">Matrix.</param>
        /// <returns>A Matrix.</returns>
        private static Matrix Inverse(Matrix mat)
        {
            // working space
            Matrix matrix = new Matrix(mat.Rows, 2 * mat.Cols);
            // copy over colums
            for (int i = 0; i < mat.Cols; i++)
                matrix[i, VectorType.Col] = mat[i, VectorType.Col];

            // fill in identity
            for (int i = mat.Cols; i < 2 * mat.Cols; i++)
                matrix[i - mat.Cols, i] = 1;

            int maxrow;
            double c;
            for (int y = 0; y < matrix.Rows; y++)
            {
                maxrow = y;
                for (int y2 = y + 1; y2 < matrix.Rows; y2++)
                    if (System.Math.Abs(matrix[y2, y]) > System.Math.Abs(matrix[maxrow, y]))
                        maxrow = y2;

                // swap rows
                matrix.SwapRow(maxrow, y);

                // uh oh
                if (System.Math.Abs(matrix[y][y]) <= 0.00000000001) throw new SingularMatrixException("Matrix is becoming unstable!");

                for (int y2 = y + 1; y2 < matrix.Rows; y2++)
                {
                    c = matrix[y2, y] / matrix[y, y];
                    for (int x = y; x < matrix.Cols; x++)
                        matrix[y2, x] -= matrix[y, x] * c;
                }
            }

            // back substitute
            for (int y = matrix.Rows - 1; y >= 0; y--)
            {
                c = matrix[y][y];
                for (int y2 = 0; y2 < y; y2++)
                    for (int x = matrix.Cols - 1; x > y - 1; x--)
                        matrix[y2, x] -= matrix[y, x] * matrix[y2, y] / c;

                matrix[y, y] /= c;
                for (int x = matrix.Rows; x < matrix.Cols; x++)
                    matrix[y, x] /= c;
            }

            // generate result
            Matrix result = new Matrix(mat.Rows);
            for (int i = mat.Cols; i < 2 * mat.Cols; i++)
                result[i - mat.Cols, VectorType.Col] = matrix[i, VectorType.Col];

            return result;
        }

        #region Logical Selectors

        /// <summary>
        /// Returns an array of index (i, j) pairs matching indices that are equal to the supplied value.
        /// </summary>
        /// <param name="mat">Matrix.</param>
        /// <param name="val">Value.</param>
        /// <returns></returns>
        public static IEnumerable<int[]> operator ==(Matrix mat, double val)
        {
            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Cols; j++)
                    if (mat[i, j] == val)
                        yield return new int[] { i, j };
        }

        /// <summary>
        /// Returns an array of index (i, j) pairs matching indices that are not equal to the supplied value.
        /// </summary>
        /// <param name="mat">Matrix.</param>
        /// <param name="val">Value.</param>
        /// <returns></returns>
        public static IEnumerable<int[]> operator !=(Matrix mat, double val)
        {
            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Cols; j++)
                    if (mat[i, j] != val)
                        yield return new int[] { i, j };
        }

        /// <summary>
        /// Returns an array of index (i, j) pairs matching indices that are less than the supplied value.
        /// </summary>
        /// <param name="mat">Matrix.</param>
        /// <param name="val">Value.</param>
        /// <returns></returns>
        public static IEnumerable<int[]> operator <(Matrix mat, double val)
        {
            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Cols; j++)
                    if (mat[i, j] < val)
                        yield return new int[] { i, j };
        }

        /// <summary>
        /// Returns an array of index (i, j) pairs matching indices that are greater than the supplied value.
        /// </summary>
        /// <param name="mat">Matrix.</param>
        /// <param name="val">Value.</param>
        /// <returns></returns>
        public static IEnumerable<int[]> operator >(Matrix mat, double val)
        {
            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Cols; j++)
                    if (mat[i, j] > val)
                        yield return new int[] { i, j };
        }

        /// <summary>
        /// Returns an array of index (i, j) pairs matching indices that are less than or equal to the supplied value.
        /// </summary>
        /// <param name="mat">Matrix.</param>
        /// <param name="val">Value.</param>
        /// <returns></returns>
        public static IEnumerable<int[]> operator <=(Matrix mat, double val)
        {
            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Cols; j++)
                    if (mat[i, j] <= val)
                        yield return new int[] { i, j };
        }

        /// <summary>
        /// Returns an array of index (i, j) pairs matching indices that are greater than or equal to the supplied value.
        /// </summary>
        /// <param name="mat">Matrix.</param>
        /// <param name="val">Value.</param>
        /// <returns></returns>
        public static IEnumerable<int[]> operator >=(Matrix mat, double val)
        {
            for (int i = 0; i < mat.Rows; i++)
                for (int j = 0; j < mat.Cols; j++)
                    if (mat[i, j] >= val)
                        yield return new int[] { i, j };
        }

        #endregion
    }
}
