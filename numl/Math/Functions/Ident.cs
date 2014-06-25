using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Math.Functions
{
    public class Ident : Function
    {
        public override double Compute(double x)
        {
            return x;
        }

        public override double Derivative(double x)
        {
            return 1;
        }
    }
}
