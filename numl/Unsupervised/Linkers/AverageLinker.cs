using System;
using System.Collections.Generic;
using System.Linq;
using numl.Math;
using numl.Math.Metrics;

namespace numl.Unsupervised.Linkers
{
    public class AverageLinker : ILinker
    {
        private IDistance _distanceMetric;

        public AverageLinker(IDistance distanceMetric)
        {
            _distanceMetric = distanceMetric;
        }

        public double Distance(IEnumerable<Vector> x, IEnumerable<Vector> y)
        {
            double distanceSum = 0;

            for (int i = 0; i < x.Count(); i++)
            {
                for (int j = i + 1; j < y.Count(); j++)
                {
                    distanceSum += _distanceMetric.Compute(x.ElementAt(i), y.ElementAt(j));
                }
            }

            return distanceSum / (x.Count() * y.Count());
        }
    }
}