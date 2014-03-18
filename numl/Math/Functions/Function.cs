using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Functions
{
    public abstract class Function : IFunction
    {
        internal double exp(double x) { return System.Math.Exp(x); }
        internal double pow(double x, double a) { return System.Math.Pow(x, a); }
        public abstract double Compute(double x);

        public abstract double Derivative(double x);

        public Vector Compute(Vector x)
        {
            return x.Calc(d => Compute(d));
        }

        public Vector Derivative(Vector x)
        {
            return x.Calc(d => Derivative(d));
        }
    }
}
