using System;
using System.Linq;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace numl.Math.Kernels
{
    public class RBFKernel : IKernel
    {
        public double Sigma { get; private set; }

        public RBFKernel(double sigma)
        {
            Sigma = sigma;
        }


        public Matrix Compute(Matrix m)
        {
            var K = new DenseMatrix(m.RowCount, m.RowCount);

            // by definition a kernel matrix is symmetric;
            // therefore we can cut calculations in half
            for (int i = 0; i < m.RowCount; i++)
            {
                for (int j = i; j < m.RowCount; j++)
                {
                    var p = m.Row(i) - m.Row(j);
                    var xy = -1 * p.DotProduct(p);
                    K[i, j] = K[j, i] = System.Math.Exp(xy / (2 * System.Math.Pow(Sigma, 2)));
                }
            }

            return K;
        }

        public Vector Project(Matrix m, Vector x)
        {
            var K = new DenseVector(m.RowCount);

            for (int i = 0; i < K.Count; i++)
            {
                var p = m.Row(i) - x;
                var xy = -1 * p.DotProduct(p);
                K[i] = System.Math.Exp(xy / (2 * System.Math.Pow(Sigma, 2)));
            }

            return K;
        }
    }
}
