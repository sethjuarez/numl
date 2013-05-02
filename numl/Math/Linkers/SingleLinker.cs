using System;
using System.Linq;
using numl.Math.Metrics;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Linkers
{
    public class SingleLinker : ILinker
    {
        private readonly IDistance _metric;
        public SingleLinker(IDistance metric)
        {
            _metric = metric;
        }

        public double Distance(IEnumerable<Vector> x, IEnumerable<Vector> y)
        {
            double distance = -1;
            double leastDistance = Int32.MaxValue;

            for (int i = 0; i < x.Count(); i++)
            {
                for (int j = i + 1; j < y.Count(); j++)
                {
                    distance = _metric.Compute(x.ElementAt(i), y.ElementAt(j));

                    if (distance < leastDistance)
                        leastDistance = distance;
                }
            }

            return leastDistance;
        }
    }
}
