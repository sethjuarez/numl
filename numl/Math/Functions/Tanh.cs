using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.Functions
{
    public class Tanh : Function
    {
        public override double Compute(double x)
        {
            return (exp(x) - exp(-x)) / (exp(x) + exp(-x));
        }

        public override double Derivative(double x)
        {
            return 1 - pow(Compute(x), 2);
        }
    }
}
