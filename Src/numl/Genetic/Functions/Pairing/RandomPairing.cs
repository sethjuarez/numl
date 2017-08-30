using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using numl.Math.Probability;

namespace numl.Genetic.Functions.Pairing
{
    /// <summary>
    /// Random pairing.
    /// </summary>
    public class RandomPairing : PairingBase
    {
        /// <summary>
        /// Performs random pairing from the pool of chromosomes.
        /// <para>Each chromosome has a probability of 1/N of being paired.</para>
        /// </summary>
        /// <param name="pool">Population of chromosomes.</param>
        /// <returns>IChromosome pair.</returns>
        public override IEnumerable<(IChromosome, IChromosome)> Pair(IEnumerable<IChromosome> pool)
        {
            if (pool.Count() <= 2)
                throw new ArgumentException("Number of chromosomes must be greater than 2.", nameof(pool));

            int count = (pool.Count() - 1);

            foreach (var chromosome in pool)
            {
                int parent1 = this.Select(0, count);
                int parent2 = this.Select(0, count);

                while (parent1 == parent2 && !this.SelfPairing)
                {
                    parent2 = this.Select(0, count);
                }

                IChromosome c1 = pool.ElementAt(parent1);
                IChromosome c2 = pool.ElementAt(parent2);

                yield return (c1, c2);
            }
        }
    }
}
