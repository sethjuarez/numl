using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;

namespace numl.Math
{
    internal static class Numerics
    {

        private static MathNet.Numerics.LinearAlgebra.Double.Matrix ConvertToMathNetMatrix(this Matrix matrix)
        {
            var m = new MathNet.Numerics.LinearAlgebra.Double.SparseMatrix(matrix.Rows, matrix.Cols);
            for (int i = 0; i < matrix.Rows; i++)
                for (int j = 0; j < matrix.Cols; j++)
                    m[i, j] = matrix[i, j];

            return m;
        }

        private static Matrix ConvertToNumlMatrix(this MathNet.Numerics.LinearAlgebra.Generic.Matrix<double> matrix)
        {
            var m = new Matrix(matrix.RowCount, matrix.ColumnCount);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    m[i, j] = matrix[i, j];

            return m;
        }

        private static Vector ConvertToNumlVector(this MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex> d)
        {
            Vector values = new Vector(d.Count);
            for (int i = 0; i < values.Length; i++)
                if (d[i].Imaginary != 0)
                    throw new InvalidOperationException("Imaginary eigen values");
                else
                    values[i] = d[i].Real;
            return values;
        }

        private static Vector ConvertToNumlVector(this MathNet.Numerics.LinearAlgebra.Generic.Vector<double> d)
        {
            Vector values = new Vector(d.Count);
            for (int i = 0; i < values.Length; i++)
                values[i] = d[i];
            return values;
        }

        internal static Tuple<Vector, Matrix> Eigs(Matrix A)
        {
            var evd = new UserEvd(A.ConvertToMathNetMatrix());
            var d = evd.EigenValues();
            var v = evd.EigenVectors();
            var values = d.ConvertToNumlVector();
            var matrix = v.ConvertToNumlMatrix();

            return new Tuple<Vector, Matrix>(values, matrix);
        }

        internal static Tuple<Matrix, Vector, Matrix> SVD(Matrix A)
        {
            var svd = new UserSvd(A.ConvertToMathNetMatrix(), true);
            var U = svd.U().ConvertToNumlMatrix();
            var S = svd.S().ConvertToNumlVector();
            var V = svd.VT().ConvertToNumlMatrix();

            return new Tuple<Matrix, Vector, Matrix>(U, S, V);
        }
    }
}
