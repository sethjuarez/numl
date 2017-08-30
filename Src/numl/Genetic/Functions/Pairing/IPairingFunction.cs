using System;
using System.Collections.Generic;
using System.Text;

namespace numl.Genetic.Functions.Pairing
{
    /// <summary>
    /// IPairingFunction interface.
    /// </summary>
    public interface IPairingFunction
    {
        /// <summary>
        /// Returns an enumerable of pairs from the genetic pool. 
        /// </summary>
        /// <param name="pool">Global pool of chromosomes.</param>
        /// <returns>IChromosome pair.</returns>
        IEnumerable<(IChromosome, IChromosome)> Pair(IEnumerable<IChromosome> pool);
    }
}
