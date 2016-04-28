// file:	Math\Kernels\RBFKernel.cs
//
// summary:	Implements the rbf kernel class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Math.Functions;

namespace numl.Math.Kernels
{
    /// <summary>
    /// Logistic Kernel for computing the similarity between the inner product space.
    /// logit(L * xi * xj)
    /// </summary>
    public class LogisticKernel : IKernel
    {
        /// <summary>
        /// Returns True (always) indicating this is a linear kernel.
        /// </summary>
        public bool IsLinear
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the Lambda modifier value (default is 1).
        /// </summary>
        public double Lambda { get; set; }

        /// <summary>
        /// Gets or sets the logistic function (default is Sigmoid).
        /// </summary>
        /// <returns></returns>
        public IFunction LogisticFunction { get; set; }

        /// <summary>Default constructor (Sigmoid logit function).</summary>
        public LogisticKernel()
        {
            Lambda = 1d;
            LogisticFunction = new Logistic();
        }

        /// <summary>Computes a Logistic Kernel matrix from the given input matrix.</summary>
        /// <param name="m">Input Matrix.</param>
        /// <returns>Logistic Kernel Matrix.</returns>
        public Matrix Compute(Matrix m)
        {
            var K = Matrix.Zeros(m.Rows);

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = i; j < m.Rows; j++)
                {
                    var xy = m[i].Dot(m[j]);
                    K[i, j] = K[j, i] = LogisticFunction.Compute(Lambda * xy);
                }
            }

            return K;
        }

        /// <summary>
        /// Computes the logistic kernel function between the two input vectors.
        /// </summary>
        /// <param name="v1">Vector one.</param>
        /// <param name="v2">Vector two.</param>
        /// <returns>Similarity.</returns>
        public double Compute(Vector v1, Vector v2)
        {
            return LogisticFunction.Compute(v1.Dot(v2));
        }

        /// <summary>Projects vector into a logistic kernel space.</summary>
        /// <param name="m">Kernel Matrix.</param>
        /// <param name="x">Vector in original space.</param>
        /// <returns>Vector in logistic kernel space.</returns>
        public Vector Project(Matrix m, Vector x)
        {
            var K = Vector.Zeros(m.Rows);

            for (int i = 0; i < K.Length; i++)
            {
                var xy = m[i].Dot(x);
                K[i] = LogisticFunction.Compute(Lambda * xy);
            }

            return K;
        }

        
    }
}
