using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using numl.Utils;

namespace numl.Genetic.Functions.Pairing
{
    /// <summary>
    /// Tournament selection (pairing).
    /// <para>Successive rounds of evaluations on random samples, of a given tournament size, 
    /// are performed to yield the best two pairs for crossover.</para>
    /// </summary>
    public class TournamentPairing : PairingBase
    {
        /// <summary>
        /// Gets or sets the tournament size (default is binary).
        /// <para>A larger tournament size will preserve diversity in the population at the 
        /// risk of delaying convergence.</para>
        /// </summary>
        public int Size { get; set; } = 2;

        /// <summary>
        /// Runs a single bifold tournament and returns the winning pair.
        /// </summary>
        /// <param name="current">Current chromosome.</param>
        /// <param name="tournament">Tournament population.</param>
        /// <returns>IChromosome pair.</returns>
        private (IChromosome, IChromosome) Match(IChromosome current, IEnumerable<IChromosome> tournament)
        {
            IChromosome tree1 = current;
            IChromosome tree2 = current;

            if (!this.SelfPairing)
            {
                tree1 = (tournament.ElementAt(0) != current ? tournament.ElementAt(0) : tournament.ElementAt(1));
            }

            for (int i = 1; i < tournament.Count(); i++)
            {
                bool even = (i % 2 == 0);

                IChromosome test = tournament.ElementAt(i);

                if (even)
                {
                    if (test.Weight > tree2.Weight)
                        tree2 = test;
                }
                else
                {
                    if (test.Weight > tree1.Weight)
                        tree1 = test;
                }
            }

            return (tree1, tree2);
        }

        /// <summary>
        /// Performs tournament selection over the population and returns the best pairs.
        /// </summary>
        /// <param name="pool">Population of chromosomes.</param>
        /// <returns>IChromosome pair.</returns>
        public override IEnumerable<(IChromosome, IChromosome)> Pair(IEnumerable<IChromosome> pool)
        {
            int max = System.Math.Min(this.Size * 2, pool.Count());

            foreach (var chromosome in pool)
            {
                (IChromosome first, IChromosome second) = this.Match(chromosome, pool.Shuffle().Take(max));

                yield return (first, second);
            }
        }
    }
}
