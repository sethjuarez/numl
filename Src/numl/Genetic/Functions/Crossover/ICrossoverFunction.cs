using System;
using System.Collections.Generic;
using System.Text;

namespace numl.Genetic.Functions.Crossover
{
    /// <summary>
    /// ICrossoverFunction interface.
    /// </summary>
    public interface ICrossoverFunction
    {
        /// <summary>
        /// Performs genetic crossover, giving rise to a new child chromosome based on parent characteristics. 
        /// </summary>
        /// <param name="chromosome1">First parent chromosome.</param>
        /// <param name="chromosome2">Second parent chromosome.</param>
        /// <returns>IChromosome.</returns>
        IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2);
    }
}
