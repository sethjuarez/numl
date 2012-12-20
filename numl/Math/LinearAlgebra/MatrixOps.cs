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

namespace numl.Math.LinearAlgebra
{
    public partial class Matrix
    {
        // --------------------- operators
        public static implicit operator Matrix(double[,] m)
        {
            return new Matrix(m);
        }

        public static implicit operator Matrix(int[,] m)
        {
            Matrix matrix = new Matrix();
            matrix._asTransposeRef = false;
            matrix.Rows = m.GetLength(0);
            matrix.Cols = m.GetLength(1);
            matrix._matrix = new double[matrix.Rows][];
            for (int i = 0; i < matrix.Rows; i++)
            {
                matrix._matrix[i] = new double[matrix.Cols];
                for (int j = 0; j < matrix.Cols; j++)
                    matrix._matrix[i][j] = m[i, j];
            }

            return matrix;
        }

        // --------------------- mathematical ops

        public static bool operator ==(Matrix m1, Matrix m2)
        {
            return (object.ReferenceEquals(m1, null) && object.ReferenceEquals(m2, null) || m1.Equals(m2));
        }

        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !m1.Equals(m2);
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
                throw new InvalidOperationException("Dimensions do not match");

            var result = Matrix.Zeros(m1.Rows, m1.Cols);
            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m1.Cols; j++)
                    result[i, j] = m1[i, j] + m2[i, j];

            return result;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
                throw new InvalidOperationException("Dimensions do not match");

            var result = Matrix.Zeros(m1.Rows, m1.Cols);
            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m1.Cols; j++)
                    result[i, j] = m1[i, j] - m2[i, j];

            return result;
        }

        /// <summary>
        /// In memory addition of double to matrix
        /// </summary>
        /// <param name="m">Matrix</param>
        /// <param name="s">double</param>
        /// <returns></returns>
        public static Matrix operator +(Matrix m, double s)
        {
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    m[i, j] += s;
            return m;
        }

        public static Matrix operator +(double s, Matrix m)
        {
            return m + s;
        }

        /// <summary>
        /// Subtract double from every element in the Matrix
        /// </summary>
        /// <param name="m">Matrix</param>
        /// <param name="s">Double</param>
        /// <returns></returns>
        public static Matrix operator -(Matrix m, double s)
        {
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    m[i, j] -= s;
            return m;
        }

        public static Matrix operator -(double s, Matrix m)
        {
            return m - s;
        }

        /// <summary>
        /// matrix multiplication
        /// </summary>
        /// <param name="m1">left hand side</param>
        /// <param name="m2">right hand side</param>
        /// <returns>matrix</returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.Cols != m2.Rows)
                throw new InvalidOperationException("Invalid multiplication dimenstion");

            var result = Matrix.Zeros(m1.Rows, m2.Cols);
            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m2.Cols; j++)
                    for (int k = 0; k < m1.Cols; k++)
                        result[i, j] += m1[i, k] * m2[k, j];

            return result;
        }

        /// <summary>
        /// Scalar matrix multiplication
        /// </summary>
        /// <param name="s">scalar</param>
        /// <param name="m">matrix</param>
        /// <returns>matrix</returns>
        public static Matrix operator *(double s, Matrix m)
        {
            var result = Matrix.Zeros(m.Rows, m.Cols);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    result[i, j] = s * m[i, j];

            return result;
        }

        /// <summary>
        /// reverse
        /// </summary>
        /// <param name="m"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix m, double s)
        {
            return s * m;
        }

        public static Matrix operator *(Matrix m, Vector v)
        {
            Vector ans = Matrix.Dot(m, v);
            return ans.ToMatrix(VectorType.Column);
        }

        public static Matrix operator *(Vector v, Matrix m)
        {
            Vector ans = Matrix.Dot(v, m);
            return ans.ToMatrix(VectorType.Row);
        }

        /// <summary>
        /// Solves Ax = b for x
        /// If A is not square or the system is overdetermined, this operation
        /// solves the linear least squares A.T * A x = A.T * b
        /// </summary>
        /// <param name="A">Matrix A</param>
        /// <param name="b">Vector b</param>
        /// <returns>x</returns>
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

        public static Matrix operator /(Matrix A, double b)
        {
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < A.Cols; j++)
                    A[i, j] /= b;
            return A;
        }

        /// <summary>
        /// Matrix inverse using pivoted Gauss-Jordan elimination with partial pivoting
        /// See:http://www.cse.illinois.edu/iem/linear_equations/gauss_jordan/
        /// for python implementaion
        /// </summary>
        /// <param name="mat">Matrix</param>
        /// <param name="n">-1</param>
        /// <returns>Inverse (or exception if matrix is singular)</returns>
        public static Matrix operator ^(Matrix mat, int n)
        {
            if (n != -1)
                throw new InvalidOperationException("This is a hack for the inverse, please use it as such!");
            if (mat.Rows != mat.Cols)
                throw new InvalidOperationException("Can only find inverse of square matrix!");

            // working space
            Matrix matrix = new Matrix(mat.Rows, 2 * mat.Cols);
            // copy over colums
            for (int i = 0; i < mat.Cols; i++)
                matrix[i, VectorType.Column] = mat[i, VectorType.Column];

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
                result[i - mat.Cols, VectorType.Column] = matrix[i, VectorType.Column];

            return result;
        }
        
    }
}
