using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Kernels
{
    public interface IKernel
    {
        Matrix Compute(Matrix m);
        Vector Project(Matrix m, Vector x);
    }
}