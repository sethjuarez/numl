using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Metrics
{
    public interface IDistance
    {
        double Compute(Vector x, Vector y);
    }
}
