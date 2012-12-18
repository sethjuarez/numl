using System;
using System.Linq;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace numl.Math.Kernels
{
    public class PolyKernel : IKernel
    {
        public double Dimension { get; private set; }

        public PolyKernel(double dimension)
        {
            Dimension = dimension;
        }

        public Matrix Compute(Matrix m)
        {
            // by definition a kernel matrix is symmetric;
            // therefore we can cut calculations in half
            var K = new DenseMatrix(m.RowCount, m.RowCount);
            for (int i = 0; i < m.RowCount; i++)
                for (int j = i; j < m.RowCount; j++)
                    K[i, j] = K[j, i] = System.Math.Pow((1 + m.Row(i).DotProduct(m.Row(j))), Dimension);

            return K;
        }

        public Vector Project(Matrix m, Vector x)
        {
            var K = new DenseVector(m.RowCount);
            for (int i = 0; i < K.Count; i++)
                K[i] = System.Math.Pow(1 + m.Row(i).DotProduct(x), Dimension);

            return K;
        }
    }
}
