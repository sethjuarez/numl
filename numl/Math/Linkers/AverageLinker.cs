using System;
using System.Linq;
using numl.Math.Metrics;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Linkers
{
    public class AverageLinker : ILinker
    {
        private IDistance _metric;

        public AverageLinker(IDistance metric)
        {
            _metric = metric;
        }

        public double Distance(IEnumerable<Vector> x, IEnumerable<Vector> y)
        {
            double distanceSum = 0;

            int xCount = x.Count();
            int yCount = y.Count();
            for (int i = 0; i < xCount; i++)
                for (int j = i + 1; j < yCount; j++)
                    distanceSum += _metric.Compute(x.ElementAt(i), y.ElementAt(j));

            return distanceSum / (double)(xCount * yCount);
        }
    }
}