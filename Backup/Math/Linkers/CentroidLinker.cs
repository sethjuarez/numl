// file:	Math\Linkers\CentroidLinker.cs
//
// summary:	Implements the centroid linker class
using System;
using System.Linq;
using numl.Math.Metrics;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Math.Linkers
{
    /// <summary>A centroid linker.</summary>
    public class CentroidLinker : ILinker
    {
        /// <summary>The metric.</summary>
        private readonly IDistance _metric;
        /// <summary>Constructor.</summary>
        /// <param name="metric">The metric.</param>
        public CentroidLinker(IDistance metric)
        {
            _metric = metric;
        }
        /// <summary>Distances.</summary>
        /// <param name="x">The IEnumerable&lt;Vector&gt; to process.</param>
        /// <param name="y">The IEnumerable&lt;Vector&gt; to process.</param>
        /// <returns>A double.</returns>
        public double Distance(IEnumerable<Vector> x, IEnumerable<Vector> y)
        {
            return _metric.Compute(x.Mean(), y.Mean());
        }
    }
}