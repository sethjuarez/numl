using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Functions
{
    public interface IFunction
    {
        double Compute(double x);
        double Derivative(double x);
        Vector Compute(Vector x);
        Vector Derivative(Vector x);
    }
}
