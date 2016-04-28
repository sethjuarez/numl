// file:	Math\Kernels\PolyKernel.cs
//
// summary:	Implements the polygon kernel class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Kernels
{
    /// <summary>Polynomial kernel of arbitrary dimension.</summary>
    public class PolyKernel : IKernel
    {
        /// <summary>
        /// Returns False (always) indicating this is a non-linear kernel.
        /// </summary>
        public bool IsLinear
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Specifies dimensionality of projection based on (1 + x.T y)^d where d is the dimension.
        /// </summary>
        /// <value>The dimension.</value>
        public double Dimension { get; set; } 

        /// <summary>ctor.</summary>
        /// <param name="dimension">Polynomial Kernel Dimension.</param>
        public PolyKernel(double dimension)
        {
            Dimension = dimension;
        }

        public PolyKernel()
        {
            Dimension = 1;
        }
        /// <summary>Computes polynomial kernel of the specified degree (in Dimension)</summary>
        /// <param name="m">Input Matrix.</param>
        /// <returns>Kernel Matrix.</returns>
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

        /// <summary>
        /// Computes the polynomial kernel function between the two input vectors.
        /// </summary>
        /// <param name="v1">Vector one.</param>
        /// <param name="v2">Vector two.</param>
        /// <returns>Similarity.</returns>
        public double Compute(Vector v1, Vector v2)
        {
            return System.Math.Pow((1d + v1.Dot(v2)), Dimension);
        }

        /// <summary>Projects vector into polynomial kernel space.</summary>
        /// <param name="m">Polynomial Kernel Matrix.</param>
        /// <param name="x">Vector in original space.</param>
        /// <returns>Vector in polynomial kernel space.</returns>
        public Vector Project(Matrix m, Vector x)
        {
            var K = Vector.Zeros(m.Rows);
            for (int i = 0; i < K.Length; i++)
                K[i] = System.Math.Pow(1 + m[i].Dot(x), Dimension);

            return K;
        }
    }
}
