using System;
using System.Linq;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace numl.Math.Kernels
{
    public interface IKernel
    {
        Matrix Compute(Matrix m);
        Vector Project(Matrix m, Vector x);
    }
}