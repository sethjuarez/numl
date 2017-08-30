using System;
using System.Collections.Generic;
using System.Text;

using numl.Math.LinearAlgebra;

namespace numl.Genetic
{
    /// <summary>
    /// IChromosome interface.
    /// </summary>
    public interface IChromosome : IComparable
    {
        /// <summary>
        /// Gets or sets the weight of the chromosome.
        /// <para>The weight is the normalized likelihood of the fitness of the chromosome.
        /// A greater fit corresponds to a higher weight.</para>
        /// </summary>
        double Weight { get; set; }

        /// <summary>
        /// Gets or sets the fitness of this chromosome.
        /// </summary>
        double Fitness { get; set; }

        /// <summary>
        /// Gets the generation of this chromosome.
        /// </summary>
        int Generation { get; set; }

        /// <summary>
        /// Gets or sets the genetic sequence of the chromosome.
        /// </summary>
        Vector Sequence { get; set; }

        /// <summary>
        /// Clones the current chromosome.
        /// </summary>
        /// <returns></returns>
        IChromosome Clone();
    }
}
