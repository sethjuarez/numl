using System;
using System.Collections.Generic;
using System.Text;

using numl.Math.Probability;

namespace numl.Genetic.Functions.Pairing
{
    /// <summary>
    /// PairingBase abstract class.
    /// </summary>
    public abstract class PairingBase : IPairingFunction
    {
        /// <summary>
        /// Gets or sets whether chromosomes can be paired with themselves.
        /// </summary>
        public bool SelfPairing { get; set; } = false;

        /// <summary>
        /// Picks a random sample from a uniform distribution.
        /// </summary>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Maximum.</param>
        /// <returns>Int.</returns>
        protected int Select(int min, int max)
        {
            return Sampling.GetUniform(min, max);
        }

        /// <summary>
        /// When overridden, performs pairing and returns a single pair selection for crossover.
        /// </summary>
        /// <param name="pool">Population.</param>
        /// <returns>IChromosome pair.</returns>
        public abstract IEnumerable<(IChromosome, IChromosome)> Pair(IEnumerable<IChromosome> pool);
    }
}
