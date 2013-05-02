using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Metrics
{
    public interface ISimilarity
    {
        double Compute(Vector x, Vector y);
    }
}
