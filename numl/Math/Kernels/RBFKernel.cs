using System;
using System.Linq;
using System.Collections.Generic;
using numl.Math.LinearAlgebra;

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
            var K = Matrix.Zeros(m.Rows);

            // by definition a kernel matrix is symmetric;
            // therefore we can cut calculations in half
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = i; j < m.Rows; j++)
                {
                    var p = m[i] - m[j];
                    var xy = -1 * p.Dot(p);
                    K[i, j] = K[j, i] = System.Math.Exp(xy / (2 * System.Math.Pow(Sigma, 2)));
                }
            }

            return K;
        }

        public Vector Project(Matrix m, Vector x)
        {
            var K = Vector.Zeros(m.Rows);

            for (int i = 0; i < K.Length; i++)
            {
                var p = m[i] - x;
                var xy = -1 * p.Dot(p);
                K[i] = System.Math.Exp(xy / (2 * System.Math.Pow(Sigma, 2)));
            }

            return K;
        }
    }
}
