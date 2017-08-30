using System;
using System.Collections.Generic;
using System.Text;

namespace numl.Genetic.Functions.Mutation
{
    /// <summary>
    /// IMutationFunction interface.
    /// </summary>
    public interface IMutationFunction
    {
        /// <summary>
        /// Performs genetic mutation on the chromosome.
        /// </summary>
        /// <param name="chromosome">Chromosome to mutate.</param>
        /// <returns>IChromosome.</returns>
        IChromosome Mutate(IChromosome chromosome);
    }
}
