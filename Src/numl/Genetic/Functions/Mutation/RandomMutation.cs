using System;
using System.Collections.Generic;
using System.Text;

using numl.Math;
using numl.Math.Probability;

namespace numl.Genetic.Functions.Mutation
{
    /// <summary>
    /// Random replacement mutation.
    /// <para>Randomly replaces a gene with a random value between min and max, sampled 
    /// from a uniform distribution.</para>
    /// </summary>
    public class RandomMutation : MutationBase
    {
        /// <summary>
        /// Gets or sets the range of values used in replacement.
        /// </summary>
        public Range Range { get; set; } = (0, 1);

        /// <summary>
        /// Initializes a new Random mutator with replacement between range 0 and 1.
        /// </summary>
        public RandomMutation() { }

        /// <summary>
        /// Initializes a new Random mutator using the specified replacement range.
        /// </summary>
        /// <param name="minValue">Minimum value used during random replacement.</param>
        /// <param name="maxValue">Maximum value used during random replacement.</param>
        public RandomMutation(double minValue, double maxValue)
        {
            this.Range = new Range(minValue, maxValue);
        }

        /// <summary>
        /// Mutates the chromosome using random replacement.
        /// </summary>
        /// <param name="chromosome">Chromosome to mutate.</param>
        /// <returns>IChromosome.</returns>
        public override IChromosome Mutate(IChromosome chromosome)
        {
            var clone = chromosome.Clone();

            clone.Sequence[this.N] = Sampling.GetUniform(this.Range.Min, this.Range.Max);

            return clone;
        }
    }
}
