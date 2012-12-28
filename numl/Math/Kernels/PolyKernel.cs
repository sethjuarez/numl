using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

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
            var K = Matrix.Zeros(m.Rows);
            for (int i = 0; i < m.Rows; i++)
                for (int j = i; j < m.Rows; j++)
                    K[i, j] = K[j, i] = System.Math.Pow((1 + m[i].Dot(m[j])), Dimension);

            return K;
        }

        public Vector Project(Matrix m, Vector x)
        {
            var K = Vector.Zeros(m.Rows);
            for (int i = 0; i < K.Length; i++)
                K[i] = System.Math.Pow(1 + m[i].Dot(x), Dimension);

            return K;
        }
    }
}
