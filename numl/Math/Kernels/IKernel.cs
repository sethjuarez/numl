using System;
using System.Linq;
using System.Collections.Generic;
using numl.Math.LinearAlgebra;

namespace numl.Math.Kernels
{
    public interface IKernel
    {
        Matrix Compute(Matrix m);
        Vector Project(Matrix m, Vector x);
    }
}