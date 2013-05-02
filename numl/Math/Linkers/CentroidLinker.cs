using System;
using System.Linq;
using numl.Math.Metrics;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Linkers
{
    public class CentroidLinker : ILinker
    {
        private readonly IDistance _metric;
        public CentroidLinker(IDistance metric)
        {
            _metric = metric;
        }

        public double Distance(IEnumerable<Vector> x, IEnumerable<Vector> y)
        {
            return _metric.Compute(x.Mean(), y.Mean());
        }
    }
}