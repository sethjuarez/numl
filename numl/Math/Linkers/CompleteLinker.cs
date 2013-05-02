using System;
using System.Linq;
using numl.Math.Metrics;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Linkers
{
    public class CompleteLinker : ILinker
    {
         private IDistance _metric;

        public CompleteLinker(IDistance metric)
        {
            _metric = metric;
        }
        public double Distance(IEnumerable<Vector> x, IEnumerable<Vector> y)
        {
            double distance = -1;
            double maxDistance = double.MinValue;

            for (int i = 0; i < x.Count(); i++)
            {
                for (int j = i+1; j < y.Count(); j++)
                {
                    distance = _metric.Compute(x.ElementAt(i), y.ElementAt(j));

                    if (distance > maxDistance)
                        maxDistance = distance;
                }
            }

            return maxDistance;
        }
    }
}