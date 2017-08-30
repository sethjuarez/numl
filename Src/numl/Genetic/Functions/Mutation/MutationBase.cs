using System;
using System.Collections.Generic;
using System.Text;

namespace numl.Genetic.Functions.Mutation
{
    /// <summary>
    /// MutationBase abstract class.
    /// </summary>
    public abstract class MutationBase : IMutationFunction
    {
        /// <summary>
        /// Gets or sets the location for mutation within the genetic sequence.
        /// </summary>
        public int N { get; set; }

        /// <summary>
        /// Implements a mutation function on the current chromosome.
        /// </summary>
        /// <param name="chromosome">Chromosome to mutate.</param>
        /// <returns>IChromosome.</returns>
        public abstract IChromosome Mutate(IChromosome chromosome);
    }
}
