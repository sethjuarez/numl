﻿// file:	Math\LinearAlgebra\MatrixStatics.cs
//
// summary:	Implements the matrix statics class
using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.LinearAlgebra
{
	/// <summary>A matrix.</summary>
	public partial class Matrix
	{
		/// <summary>
		/// Performs an element wise operation on the input Matrix.
		/// </summary>
		/// <param name="m">Matrix.</param>
		/// <param name="fnElementWiseOp">Function to apply.</param>
		/// <returns>A Matrix.</returns>
		public static Matrix Each(Matrix m, Func<double, double> fnElementWiseOp)
		{
			var copy = m.ToArray();
			for (int i = 0; i < m.Rows; i++)
			{
				for (int j = 0; j < m.Cols; j++)
				{
					copy[i][j] = fnElementWiseOp(copy[i][j]);
				}
			}
			return copy;
		}

		/// <summary>
		/// Performs an element wise operation on the input Matrix.
		/// </summary>
		/// <param name="m">Matrix.</param>
		/// <param name="fnElementWiseOp">Function to update each cell specified by the value and cell coordinates.</param>
		/// <returns>A Matrix.</returns>
		public static Matrix Each(Matrix m, Func<double, int, int, double> fnElementWiseOp)
		{
			var copy = m.ToArray();
			for (int i = 0; i < m.Rows; i++)
			{
				for (int j = 0; j < m.Cols; j++)
				{
					copy[i][j] = fnElementWiseOp(copy[i][j], i, j);
				}
			}
			return copy;
		}

		/// <summary>
		/// Performs an element-wise operation on the input Matrices.
		/// </summary>
		/// <param name="m1">First Matrix.</param>
		/// <param name="m2">Second Matrix.</param>
		/// <param name="fnElementWiseOp">Operation to perform on the value from the first and second matrices.</param>
		/// <returns>A Matrix.</returns>
		public static Matrix Each(Matrix m1, Matrix m2, Func<double, double, double> fnElementWiseOp)
		{
			if (m1.Rows != m2.Rows)
				throw new InvalidOperationException("The row dimensions do not match");
			if (m1.Cols != m2.Cols)
				throw new InvalidOperationException("The column dimensions do not match");

			var copy = m1.ToArray();
			for (int i = 0; i < m1.Rows; i++)
			{
				for (int j = 0; j < m1.Cols; j++)
				{
					copy[i][j] = fnElementWiseOp(m1[i, j], m2[i, j]);
				}
			}
			return copy;
		}

		/// <summary>Computes the trace of a matrix.</summary>
		/// <param name="m">Input Matrix.</param>
		/// <returns>trace.</returns>
		public static double Trace(Matrix m)
		{
			double t = 0;
			for (int i = 0; i < m.Rows && i < m.Cols; i++)
				t += m[i, i];
			return t;
		}
		/// <summary>Computes the sum of every element of the matrix.</summary>
		/// <param name="m">Input Matrix.</param>
		/// <returns>sum.</returns>
		public static double Sum(Matrix m)
		{
			double sum = 0;
			for (int i = 0; i < m.Rows; i++)
				for (int j = 0; j < m.Cols; j++)
					sum += m[i, j];
			return sum;
		}
		/// <summary>
		/// Computes the sum of either the rows or columns of a matrix and returns a vector.
		/// </summary>
		/// <param name="m">Input Matrix.</param>
		/// <param name="t">Row or Column sum.</param>
		/// <returns>Vector Sum.</returns>
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
		/// <summary>Computes the sum of every element of the matrix.</summary>
		/// <param name="m">Input Matrix.</param>
		/// <param name="i">Zero-based index of the.</param>
		/// <param name="t">Row or Column sum.</param>
		/// <returns>sum.</returns>
		public static double Sum(Matrix m, int i, VectorType t)
		{
			return m[i, t].Sum();
		}
		/// <summary>Standard Matrix Norm.</summary>
		/// <param name="A">Input Matrix.</param>
		/// <param name="p">The double to process.</param>
		/// <returns>Standard Norm (double)</returns>
		public static double Norm(Matrix A, double p)
		{
			double norm = 0;
			for (int i = 0; i < A.Rows; i++)
				for (int j = 0; j < A.Cols; j++)
					norm += System.Math.Pow(System.Math.Abs(A[i, j]), p);
			return System.Math.Pow(norm, 1d/p);
		}
		/// <summary>Matrix Frobenius Norm.</summary>
		/// <param name="A">Input Matrix.</param>
		/// <returns>Frobenius Norm (double)</returns>
		public static double FrobeniusNorm(Matrix A)
		{
			return System.Math.Sqrt((A.T * A).Trace());
		}
		/// <summary>Eigen Decomposition.</summary>
		/// <param name="A">Input Matrix.</param>
		/// <returns>Tuple(Eigen Values, Eigen Vectors)</returns>
		public static Tuple<Vector, Matrix> Evd(Matrix A)
		{
			Evd eigs = new Evd(A);
			eigs.Compute();
			return new Tuple<Vector, Matrix>(eigs.Eigenvalues, eigs.Eigenvectors);
		}
		/// <summary>Singular Value Decomposition.</summary>
		/// <exception cref="NotImplementedException">Thrown when the requested operation is unimplemented.</exception>
		/// <param name="A">Input Matrix.</param>
		/// <returns>Tuple(Matrix U, Vector S, Matrix V)</returns>
		public static Tuple<Matrix, Vector, Matrix> SVD(Matrix A)
		{
			throw new NotImplementedException();
		}
		/// <summary>NOT IMPLEMENTED!</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="A">.</param>
		/// <returns>A Tuple&lt;Matrix,Matrix,Matrix&gt;</returns>
		public static Tuple<Matrix, Matrix, Matrix> LU(Matrix A)
		{
			// TODO: FINISH ALGORITHM
			if (A.Rows != A.Cols)
				throw new InvalidOperationException("Factorization requires a symmetric positive semidefinite matrix!");

			int n = A.Rows;

			Matrix P = Pivot(A);
			Matrix M = P * A;

			Matrix L = Identity(n);
			Matrix U = Zeros(n);

			for (int j = 0; j < n; j++)
			{
				L[j, j] = 1;

				for (int i = 0; i < j + 1; i++)
				{
					U[i, j] = M[i, j];
					for (int k = 0; k < i; k++)
						U[i, j] -= U[k, j] * L[i, k];
				}


				for (int i = j; i < n; i++)
				{
					L[i, j] = M[i, j];
					for (int k = 0; k < j; k++)
						L[i, j] -= U[k, j] * L[i, k];

					if (U[j, j] == 0)
						System.Diagnostics.Debug.WriteLine("Unstable divisor...");

					L[i, j] /= U[j, j];
				}
			}

			return new Tuple<Matrix,Matrix,Matrix>(P, L, U);
		}
		/// <summary>Pivots the given m.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="M">The Matrix to process.</param>
		/// <returns>A Matrix.</returns>
		public static Matrix Pivot(Matrix M)
		{
			if (M.Rows != M.Cols)
				throw new InvalidOperationException("Factorization requires a symmetric positive semidefinite matrix!");

			var m = M.Rows;
			var P = Identity(m);
			Tuple<int, double> row = new Tuple<int, double>(0, 0);
			for (int j = 0; j < m; j++)
			{
				row = new Tuple<int, double>(j, 0);
				for (int i = j; i < m; i++)
					if (row.Item2 < System.Math.Abs(M[i, j]))
						row = new Tuple<int, double>(i, System.Math.Abs(M[i, j]));

				if (row.Item1 != j)
					P.SwapRow(j, row.Item1);
			}

			return P;
		}

		/// <summary>Cholesky Factorization of a Matrix.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <exception cref="SingularMatrixException">Thrown when a Singular Matrix error condition occurs.</exception>
		/// <param name="m">Input Matrix.</param>
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
		/// <summary>Matrix Roundoff.</summary>
		/// <param name="m">Input Matrix.</param>
		/// <param name="decimals">(Optional) Max number of decimals (default 0 - integral members)</param>
		/// <returns>Rounded Matrix.</returns>
		public static Matrix Round(Matrix m, int decimals = 0)
		{
			for (int i = 0; i < m.Rows; i++)
				for (int j = 0; j < m.Cols; j++)
					m[i, j] = System.Math.Round(m[i, j], decimals);
			return m;
		}
		/// <summary>Modified Gram-Schmidt QR Factorization.</summary>
		/// <param name="A">Matrix A.</param>
		/// <returns>Tuple(Q, R)</returns>
		public static Tuple<Matrix, Matrix> QR(Matrix A)
		{
			int n = A.Rows;
			Matrix R = Zeros(n);
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
		/// <summary>Backwards.</summary>
		/// <param name="A">Input Matrix.</param>
		/// <param name="b">The Vector to process.</param>
		/// <returns>A Vector.</returns>
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
		/// <summary>Forwards.</summary>
		/// <param name="A">Input Matrix.</param>
		/// <param name="b">The Vector to process.</param>
		/// <returns>A Vector.</returns>
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
		/// <summary>Dot product between a matrix and a vector.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="x">Matrix x.</param>
		/// <param name="v">Vector v.</param>
		/// <returns>Vector dot product.</returns>
		public static Vector Dot(Matrix x, Vector v)
		{
			if (v.Length != x.Cols)
				throw new InvalidOperationException("objects are not aligned");

			Vector toReturn = Vector.Zeros(x.Rows);
			for (int i = 0; i < toReturn.Length; i++)
				toReturn[i] = Vector.Dot(x[i, VectorType.Row], v);
			return toReturn;
		}
		/// <summary>Dot product between a matrix and a vector.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="v">Vector v.</param>
		/// <param name="x">Matrix x.</param>
		/// <returns>Vector dot product.</returns>
		public static Vector Dot(Vector v, Matrix x)
		{
			if (v.Length != x.Rows)
				throw new InvalidOperationException("objects are not aligned");

			Vector toReturn = Vector.Zeros(x.Cols);
			for (int i = 0; i < toReturn.Length; i++)
				toReturn[i] = Vector.Dot(x[i, VectorType.Col], v);
			return toReturn;
		}
		/// <summary>Determines the mean of the given parameters.</summary>
		/// <param name="source">Source for the.</param>
		/// <param name="t">Row or Column sum.</param>
		/// <returns>The mean value.</returns>
		public static Vector Mean(Matrix source, VectorType t)
		{
			int count = t == VectorType.Row ? source.Cols : source.Rows;
			VectorType type = t == VectorType.Row ? VectorType.Col : VectorType.Row;
			Vector v = new Vector(count);
			for (int i = 0; i < count; i++)
				v[i] = source[i, type].Mean();
			return v;
		}

		/// <summary>
		/// Computes the standard deviation of the given matrix
		/// </summary>
		/// <param name="source"></param>
		/// <param name="t">Use column or row (default: Col)</param>
		/// <returns></returns>
		public static Vector StdDev(Matrix source, VectorType t = VectorType.Col)
		{
			int count = t == VectorType.Row ? source.Cols : source.Rows;
			VectorType type = t == VectorType.Row ? VectorType.Col : VectorType.Row;
			Vector v = new Vector(count);
			for (int i = 0; i < count; i++)
				v[i] = source[i, type].StdDev();
			return v;
		}

		/// <summary>
		/// Returns a vector of the maximum values for each row/column.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="t">Use column or row (default: Col)</param>
		/// <returns></returns>
		public static Vector Max(Matrix source, VectorType t = VectorType.Col)
		{
			int num = (t == VectorType.Row ? source.Cols : source.Rows);
			VectorType vectorType = (t == VectorType.Row ? VectorType.Col : VectorType.Row);
			Vector vectors = new Vector(num);
			for (int i = 0; i < num; i++)
			{
				vectors[i] = Enumerable.Max(source[i, vectorType]);
			}
			return vectors;
		}

		/// <summary>Determines the maximum of the given parameters.</summary>
		/// <param name="source">Source for the.</param>
		/// <returns>The maximum value.</returns>
		public static double Max(Matrix source)
		{
			double max = double.MinValue;
			for (int i = 0; i < source.Rows; i++)
				for (int j = 0; j < source.Cols; j++)
					if (source[i, j] > max)
						max = source[i, j];

			return max;
		}

		/// <summary>
		/// Returns a vector of the minimum values for each row/column.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="t">Use column or row (default: Col)</param>
		/// <returns></returns>
		public static Vector Min(Matrix source, VectorType t = VectorType.Col)
		{
			int num = (t == VectorType.Row ? source.Cols : source.Rows);
			VectorType vectorType = (t == VectorType.Row ? VectorType.Col : VectorType.Row);
			Vector vectors = new Vector(num);
			for (int i = 0; i < num; i++)
			{
				vectors[i] = Enumerable.Min(source[i, vectorType]);
			}
			return vectors;
		}

		/// <summary>Determines the minimum of the given parameters.</summary>
		/// <param name="source">Source for the.</param>
		/// <returns>The minimum value.</returns>
		public static double Min(Matrix source)
		{
			double min = double.MaxValue;
			for (int i = 0; i < source.Rows; i++)
				for (int j = 0; j < source.Cols; j++)
					if (source[i, j] < min)
						min = source[i, j];

			return min;
		}

		/// <summary>
		/// Returns a vector of the median values for each row or column.
		/// </summary>
		/// <param name="source">Matrix.</param>
		/// <param name="t">VectorType.</param>
		/// <returns></returns>
		public static Vector Median(Matrix source, VectorType t = VectorType.Col)
		{
			var vectors = (t == VectorType.Row ? source.GetCols() : source.GetRows());
			return vectors.Select(s => s.Median()).ToVector();
		}

		/// <summary>Covariances.</summary>
		/// <param name="source">Source for the.</param>
		/// <param name="t">(Optional) Row or Column sum.</param>
		/// <returns>A Matrix.</returns>
		public static Matrix Covariance(Matrix source, VectorType t = VectorType.Col)
		{
			int length = t == VectorType.Row ? source.Rows : source.Cols;
			Matrix m = new Matrix(length);
			//for (int i = 0; i < length; i++)
			for (int i = 0; i < length; i++)
			{
				//for (int j = i; j < length; j++) // symmetric matrix
				for (int j = i; j < length; j++)
					m[i, j] = m[j, i] = source[i, t].Covariance(source[j, t]);
			}
			return m;
		}

		/// <summary>Covariance diagram.</summary>
		/// <param name="source">Source for the.</param>
		/// <param name="t">(Optional) Row or Column sum.</param>
		/// <returns>A Vector.</returns>
		public static Vector CovarianceDiag(Matrix source, VectorType t = VectorType.Col)
		{
			int length = t == VectorType.Row ? source.Rows : source.Cols;
			Vector vector = new Vector(length);
			for (int i = 0; i < length; i++)
				vector[i] = source[i, t].Variance();
			return vector;
		}
		/// <summary>Correlations.</summary>
		/// <param name="source">Source for the.</param>
		/// <param name="t">(Optional) Row or Column sum.</param>
		/// <returns>A Matrix.</returns>
		public static Matrix Correlation(Matrix source, VectorType t = VectorType.Col)
		{
			int length = t == VectorType.Row ? source.Rows : source.Cols;
			Matrix m = new Matrix(length);
			for (int i = 0; i < length; i++)
				for (int j = i; j < length; j++) // symmetric matrix
					m[i, j] = m[j, i] = source[i, t].Correlation(source[j, t]);
			return m;
		}
		/// <summary>Enumerates reverse in this collection.</summary>
		/// <param name="source">Source for the.</param>
		/// <param name="t">(Optional) Row or Column sum.</param>
		/// <returns>
		/// An enumerator that allows foreach to be used to process reverse in this collection.
		/// </returns>
		public static IEnumerable<Vector> Reverse(Matrix source, VectorType t = VectorType.Row)
		{
			int length = t == VectorType.Row ? source.Rows : source.Cols;
			for (int i = length - 1; i > -1; i--)
				yield return source[i, t];
		}
		/// <summary>Enumerates indices in this collection.</summary>
		/// <param name="source">Source for the.</param>
		/// <param name="f">The Func&lt;Vector,bool&gt; to process.</param>
		/// <returns>
		/// An enumerator that allows foreach to be used to process indices in this collection.
		/// </returns>
		public static IEnumerable<int> Indices(Matrix source, Func<Vector, bool> f)
		{
			return Indices(source, f, VectorType.Row);
		}
		/// <summary>Enumerates indices in this collection.</summary>
		/// <param name="source">Source for the.</param>
		/// <param name="f">The Func&lt;Vector,bool&gt; to process.</param>
		/// <param name="t">Row or Column sum.</param>
		/// <returns>
		/// An enumerator that allows foreach to be used to process indices in this collection.
		/// </returns>
		public static IEnumerable<int> Indices(Matrix source, Func<Vector, bool> f, VectorType t)
		{
			int max = t == VectorType.Row ? source.Rows : source.Cols;
			for (int i = 0; i < max; i++)
				if (f(source[i, t]))
					yield return i;
		}

		/// <summary>
		/// Sorts the given Matrix by the specified row or column selector and returns the new Matrix
		/// </summary>
		/// <param name="source">The Matrix</param>
		/// <param name="keySelector">Property selector to sort by.</param>
		/// <param name="t">Specifies whether to sort horizontally or vertically.</param>
		/// <param name="ascending">Determines whether to sort ascending or descending (Default: True)</param>
		/// <returns>New Matrix and Vector of original indices.</returns>
		public static Matrix Sort(Matrix source, Func<Vector, double> keySelector, VectorType t, bool ascending = true)
		{
			Vector v;
			return Sort(source, keySelector, t, ascending, out v);
		}
		/// <summary>
		/// Sorts the given Matrix by the specified row or column selector and returns the new Matrix
		/// along with the original indices.
		/// </summary>
		/// <param name="source">The Matrix</param>
		/// <param name="keySelector">Property selector to sort by.</param>
		/// <param name="t">Specifies whether to sort horizontally or vertically.</param>
		/// <param name="ascending">Determines whether to sort ascending or descending (Default: True)</param>
		/// <param name="indices">Vector of <paramref name="t"/> indices in the original Matrix before the sort operation.</param>
		/// <returns>New Matrix and Vector of original indices.</returns>
		public static Matrix Sort(Matrix source, Func<Vector, double> keySelector, VectorType t, bool ascending, out Vector indices)
		{
			int max = (t == VectorType.Row ? source.Rows : source.Cols);
			indices = Vector.Zeros(max);

			List<Vector> vects = new List<Vector>(max);

			IEnumerable<Vector> arrays = (t == VectorType.Row ? source.GetRows() : source.GetCols());

			KeyValuePair<Vector, int>[] sort = (ascending ?
													  arrays.Select((i, v) => new KeyValuePair<Vector, int>(i, v))
															.OrderBy(o => keySelector(o.Key))
														 :
													  arrays.Select((i, v) => new KeyValuePair<Vector, int>(i, v))
															.OrderByDescending(o => keySelector(o.Key))).ToArray();

			indices = sort.Select(s => s.Value).ToVector();

			return sort.Select(s => s.Key).ToMatrix(t);
		}

		//---------------- structural
		/// <summary>Stack a set of vectors into a matrix.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="type">.</param>
		/// <param name="vectors">.</param>
		/// <returns>A Matrix.</returns>
		internal static Matrix Stack(VectorType type, params Vector[] vectors)
		{
			if (vectors.Length == 0)
				throw new InvalidOperationException("Cannot construct Matrix from empty vector set!");

			if (!vectors.All(v => v.Length == vectors[0].Length))
				throw new InvalidOperationException("Vectors must all be of the same length!");

			int n = type == VectorType.Row ? vectors.Length : vectors[0].Length;
			int d = type == VectorType.Row ? vectors[0].Length : vectors.Length;

			Matrix m = Zeros(n, d);
			for (int i = 0; i < vectors.Length; i++)
				m[i, type] = vectors[i];

			return m;
		}
		/// <summary>Stack a set of vectors into a matrix.</summary>
		/// <param name="vectors">.</param>
		/// <returns>A Matrix.</returns>
		public static Matrix Stack(params Vector[] vectors)
		{
			return Matrix.Stack(VectorType.Row, vectors);
		}
		/// <summary>Stacks.</summary>
		/// <param name="vectors">.</param>
		/// <returns>A Matrix.</returns>
		public static Matrix VStack(params Vector[] vectors)
		{
			return Stack(VectorType.Col, vectors);
		}
		/// <summary>Stack a set of vectors into a matrix.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="m">Input Matrix.</param>
		/// <param name="t">Row or Column sum.</param>
		/// <returns>A Matrix.</returns>
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
		/// <summary>Stacks.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="m">Input Matrix.</param>
		/// <param name="t">Row or Column sum.</param>
		/// <returns>A Matrix.</returns>
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
		/// <summary>Slices.</summary>
		/// <param name="m">Input Matrix.</param>
		/// <param name="indices">The indices.</param>
		/// <returns>A Matrix.</returns>
		public static Matrix Slice(Matrix m, IEnumerable<int> indices)
		{
			return MatrixExtensions.Slice(m, indices, VectorType.Row);
		}
		/// <summary>Slices.</summary>
		/// <param name="m">Input Matrix.</param>
		/// <param name="indices">The indices.</param>
		/// <param name="t">Row or Column sum.</param>
		/// <returns>A Matrix.</returns>
		public static Matrix Slice(Matrix m, IEnumerable<int> indices, VectorType t)
		{
			var q = indices.Distinct();

			int rows = t == VectorType.Row ? q.Count(j => j < m.Rows) : m.Rows;
			int cols = t == VectorType.Col ? q.Count(j => j < m.Cols) : m.Cols;

			Matrix n = new Matrix(rows, cols);

			int i = -1;
			foreach (var j in q.OrderBy(k => k))
				n[++i, t] = m[j, t];

			return n;
		}
		/// <summary>
		/// Unshapes the given Matrix into a Vector form along the <paramref name="dimensionType"/> axis.
		/// <para>Reads from the source Matrix and stacks from right to left when <paramref name="dimensionType"/> equals 'Col' otherwise uses a bottom up approach.</para>
		/// </summary>
		/// <param name="m">The Matrix to act on.</param>
		/// <param name="dimensionType">Type of the dimension to use when unrolling the Matrix.</param>
		/// <returns>Matrix.</returns>
		public static Vector Unshape(Matrix m, VectorType dimensionType = VectorType.Col)
		{
			return Vector.Combine((dimensionType == VectorType.Col ? m.GetCols().ToArray() : m.GetRows().ToArray()));
		}
		/// <summary>
		/// Reshapes the supplied Vector into a Matrix form.
		/// </summary>
		/// <param name="v">Source vector to act on.</param>
		/// <param name="dimension">Length of the specified dimension.</param>
		/// <param name="dimensionType">Dimension type to use for creating a <paramref name="dimension"/> by n matrix.</param>
		/// <param name="byVector">Direction to process, i.e. Row = Fill Down then Right, or Col = Fill Right then Down</param>
		/// <returns></returns>
		public static Matrix Reshape(Vector v, int dimension, VectorType dimensionType = VectorType.Col, VectorType byVector = VectorType.Row)
		{
			int x = (dimensionType == VectorType.Row ? dimension : v.Length / dimension);
			int y = (dimensionType == VectorType.Col ? dimension : v.Length / dimension);
			return Reshape(v, x, y, byVector);
		}
		/// <summary>
		/// Reshapes the supplied Vector into a Matrix form.
		/// </summary>
		/// <param name="v">Vector to reshape.</param>
		/// <param name="rows">Height of the matrix to return</param>
		/// <param name="columns">Width of the matrix to return</param>
		/// <param name="byVector">Direction to process, i.e. Row = Fill Down then Right, or Col = Fill Right then Down</param>
		/// <returns></returns>
		public static Matrix Reshape(Vector v, int rows, int columns, VectorType byVector = VectorType.Row)
		{
			if (rows * columns != v.Length)
				throw new InvalidOperationException(
					string.Format("Cannot reshape Vector of length {0} into a {1} x {2} Matrix.", v.Length, rows, columns));

			Matrix m = new Matrix(rows, columns);

			int counter = 0;

			switch (byVector)
			{
				case VectorType.Row:
					{
						for (int i = 0; i < columns; i++)
							for (int k = 0; k < rows; k++)
								m[k, i] = v[counter++];
					} break;
				case VectorType.Col:
					{
						for (int i = 0; i < rows; i++)
							for (int k = 0; k < columns; k++)
								m[i, k] = v[counter++];
					} break;
			}

			return m;
		}

		/// <summary>
		/// Reshapes the supplied matrix into a new matrix shape.
		/// </summary>
		/// <param name="m">Matrix to reshape.</param>
		/// <param name="rows">Number of rows of the new matrix.</param>
		/// <param name="cols">Number of columns of the new matrix.</param>
		/// <returns>Matrix.</returns>
		public static Matrix Reshape(Matrix m, int rows, int cols)
		{
			Matrix result = new Matrix(rows, cols);

			int width = (rows > m.Rows ? m.Rows : rows);
			int height = (cols > m.Cols ? m.Cols : cols);

			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
					result[y, x] = m[y, x];

			return result;
		}

		/// <summary>Extracts this object.</summary>
		/// <param name="m">Input Matrix.</param>
		/// <param name="x">Matrix x.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="safe">(Optional) true to safe.</param>
		/// <returns>A Matrix.</returns>
		public static Matrix Extract(Matrix m, int x, int y, int width, int height, bool safe = true)
		{
			Matrix m2 = Zeros(height, width);
			for (int i = y; i < y + height; i++)
				for (int j = x; j < x + width; j++)
					if (safe && i < m.Rows && j < m.Cols)
						m2[i - y, j - x] = m[i, j];

			return m2;
		}
		/// <summary>Diagrams the given m.</summary>
		/// <param name="m">Input Matrix.</param>
		/// <returns>A Vector.</returns>
		public static Vector Diag(Matrix m)
		{
			var length = m.Cols > m.Rows ? m.Rows : m.Cols;
			var v = Vector.Zeros(length);
			for (int i = 0; i < length; i++)
				v[i] = m[i, i];
			return v;
		}
		/// <summary>Statistics.</summary>
		/// <param name="x">Matrix x.</param>
		/// <param name="t">(Optional) Row or Column sum.</param>
		/// <returns>A Matrix[].</returns>
		public static Matrix[] Stats(Matrix x, VectorType t = VectorType.Row)
		{
			int length = t == VectorType.Row ? x.Cols : x.Rows;
			var type = t == VectorType.Row ? VectorType.Col : VectorType.Row;
			Matrix[] result = new Matrix[length];
			for (int i = 0; i < length; i++)
				result[i] = x[i, type].Stats();
			return result;
		}
		/// <summary>Dets the given x coordinate.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="x">Matrix x.</param>
		/// <returns>A double.</returns>
		public static double Det(Matrix x)
		{
			//     0, 1, 2
			//    --------
			// 0 | a, b, c
			// 1 | d, e, f
			// 2 | g, h, i

			if (x.Rows != x.Cols)
				throw new InvalidOperationException("Can only compute determinants of square matrices");
			int n = x.Rows;
			if (n == 1) return x[0, 0];
			else if (n == 2) return x[0, 0] * x[1, 1] - x[0, 1] * x[1, 0];
			else if (n == 3) // aei + bfg + cdh - ceg - bdi - afh
				return x[0, 0] * x[1, 1] * x[2, 2] +
					   x[0, 1] * x[1, 2] * x[2, 0] +
					   x[0, 2] * x[1, 0] * x[2, 1] -
					   x[0, 2] * x[1, 1] * x[2, 0] -
					   x[0, 1] * x[1, 0] * x[2, 2] -
					   x[0, 0] * x[1, 2] * x[2, 1];
			else 
			{
				// ruh roh, time for generalized determinant
				// to save time we'll do a cholesky factorization
				// (note, this requires a symmetric positive-semidefinite
				// matrix or it might explode....)
				// and square the product of the diagonals
				//return System.Math.Pow(x.Cholesky().Diag().Prod(), 2);

				// switched to QR since it is safer...
				// fyi:  determinant of a triangular matrix is the
				//       product of its diagonals
				// also: det(AB) = det(A) * det(B)
				// that's how we come up with this crazy thing
				var qr = QR(x);
				return qr.Item1.Diag().Prod() * qr.Item2.Diag().Prod();
				
			}
		}
	}
}
