// file:	Supervised\Perceptron\KernelPerceptronModel.cs
//
// summary:	Implements the kernel perceptron model class
using System;
using System.Linq;
using numl.Math.Kernels;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.Perceptron
{
    /// <summary>A data Model for the kernel perceptron.</summary>
    public class KernelPerceptronModel : Model
    {
        /// <summary>Gets or sets the kernel.</summary>
        /// <value>The kernel.</value>
        public IKernel Kernel { get; set; }
        /// <summary>Gets or sets the y coordinate.</summary>
        /// <value>The y coordinate.</value>
        public Vector Y { get; set; }
        /// <summary>Gets or sets a.</summary>
        /// <value>a.</value>
        public Vector A { get; set; }
        /// <summary>Gets or sets the x coordinate.</summary>
        /// <value>The x coordinate.</value>
        public Matrix X { get; set; }
        /// <summary>Predicts the given o.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        public override double Predict(Vector y)
        {
            this.Preprocess(y);

            var K = Kernel.Project(X, y);
            double v = 0;
            for (int i = 0; i < A.Length; i++)
                v += A[i] * Y[i] * K[i];

            return v;
        }
    }
}
