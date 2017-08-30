using System;
using System.Collections.Generic;
using System.Text;

using numl.Genetic.Metrics;

namespace numl.Genetic.Functions.Selection
{
    /// <summary>
    /// ISelectionFunction interface.
    /// </summary>
    public interface ISelectionFunction
    {
        /// <summary>
        /// Selects chromosomes from a candidate pool using the supplied fitness function for potential inclusion in the next generation. 
        /// </summary>
        /// <param name="pool">Pool of chromosomes or potential candidates.</param>
        /// <param name="metric">Fitness function for testing chromosomes for evaluating chromosomes.</param>
        /// <param name="fitnessMode">Fitness mode.</param>
        /// <returns>IEnumerable&lt;IChromosome&gt;</returns>
        IEnumerable<IChromosome> Select(IEnumerable<IChromosome> pool, FitnessMode fitnessMode);
    }
}
