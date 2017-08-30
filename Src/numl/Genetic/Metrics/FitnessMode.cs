using System;
using System.Collections.Generic;
using System.Text;

namespace numl.Genetic.Metrics
{
    /// <summary>
    /// Optimization Mode.
    /// </summary>
    public enum FitnessMode
    {
        /// <summary>
        /// Maximizes the expectation value.
        /// </summary>
        Maximize,
        /// <summary>
        /// Minimizes the expectation value.
        /// </summary>
        Minimize
    }
}
