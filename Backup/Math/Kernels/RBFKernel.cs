// file:	Math\Kernels\RBFKernel.cs
//
// summary:	Implements the rbf kernel class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Kernels
{
    /// <summary>
    /// The Radial Basis Function (RBF) Kernel is a projection into infinite dimensional space and
    /// acts as a pseudo similarity measure in the projected inner product space. It is governed by
    /// exp(||x - x'||2 / 2sigm^2)
    /// </summary>
    public class RBFKernel : IKernel
    {
        /// <summary>RBF free parameter.</summary>
        /// <value>The sigma.</value>
        public double Sigma { get; private set; }
        /// <summary>ctor.</summary>
        /// <param name="sigma">Input Parameter.</param>
        public RBFKernel(double sigma)
        {
            Sigma = sigma;
        }
        /// <summary>Computes RBF Kernel with provided free sigma parameter.</summary>
        /// <param name="m">Input Matrix.</param>
        /// <returns>RBF Kernel Matrix.</returns>
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
        /// <summary>Projects vector into rbf kernel space.</summary>
        /// <param name="m">RBF Kernel Matrix.</param>
        /// <param name="x">Vector in original space.</param>
        /// <returns>Vector in RBF kernel space.</returns>
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
