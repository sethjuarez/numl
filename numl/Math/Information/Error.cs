using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Information
{
    public class Error : Impurity
    {
        public override double Calculate(Vector x)
        {
            if (x == null)
                throw new InvalidOperationException("x does not exist!");

            double length = x.Count();

            var e = from i in x.Distinct()
                     let q = (from j in x
                              where j == i
                              select j).Count()
                     select q / length;

            return 1 - e.Max();
        }
    }
}
