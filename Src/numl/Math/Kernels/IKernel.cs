// file:	Math\Kernels\IKernel.cs
//
// summary:	Declares the IKernel interface
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Kernels
{
    /// <summary>
    /// In machine learning there is something called the Kernel Trick. In essence it allows for the
    /// mapping of observations in any general space into an inner product space (or Reproducing
    /// Kernel Hilbert Space). This trick thereby creates (or one hopes) linear separability in the
    /// augmented inner product space where simple linear classifiers perform extremely well.
    /// </summary>
    public interface IKernel
    {
        /// <summary>
        /// Returns a boolean indicating whether this is a linear kernel.
        /// </summary>
        bool IsLinear { get; }

        /// <summary>Computes the Kernel Matrix using the given input.</summary>
        /// <param name="m">Input Matrix.</param>
        /// <returns>Kernel Matrix.</returns>
        Matrix Compute(Matrix m);

        /// <summary>
        /// Computes the kernel function between the two input vectors.
        /// </summary>
        /// <param name="v1">Vector one.</param>
        /// <param name="v2">Vector two.</param>
        /// <returns>Similarity.</returns>
        double Compute(Vector v1, Vector v2);

        /// <summary>Projects the vector <c>x</c> into the corresponding inner product space.</summary>
        /// <param name="m">Kernel Matrix.</param>
        /// <param name="x">Vector in original space.</param>
        /// <returns>Vector in inner product space.</returns>
        Vector Project(Matrix m, Vector x);
    }
}