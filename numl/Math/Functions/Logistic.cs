using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.Functions
{
    public class Logistic : Function
    {
        public override double Compute(double x)
        {
            return 1d / (1d + exp(-x));
        }

        public override double Derivative(double x)
        {
            var c = Compute(x);
            return c * (1d - c);
        }
    }
}
