using System;
using System.Collections.Generic;
using System.Text;

using numl.Math.Probability;
using numl.Math;

namespace numl.Genetic.Functions.Mutation
{
    /// <summary>
    /// Gaussian mutation function.
    /// <para>Modifies sequences using a Gaussian distribution.</para>
    /// </summary>
    public class GaussianMutation : MutationBase
    {
        /// <summary>
        /// Gets or sets the mean of the distribution (default is 0).
        /// </summary>
        public double Mu { get; set; } = 0;

        /// <summary>
        /// Gets or sets the variance of the distribution (default is 1).
        /// </summary>
        public double Sigma { get; set; } = 1;

        /// <summary>
        /// Gets or sets whether the Gaussian sample is added or replaced with the current value (default is True).
        /// </summary>
        public bool Compound { get; set; } = true;

        /// <summary>
        /// Gets or sets the lower and upper limits of the mutated sequence values.
        /// </summary>
        public Range Range { get; set; }

        /// <summary>
        /// Initializes a new Gaussian mutation with mean 0 and deviation 1.
        /// </summary>
        public GaussianMutation() { }

        /// <summary>
        /// Initializes a new Gaussian mutation with the specified mean and standard deviation.
        /// </summary>
        /// <param name="mu">Mean of the population.</param>
        /// <param name="sigma">Standard deviation of the population.</param>
        public GaussianMutation(double mu, double sigma)
        {
            this.Mu = mu;
            this.Sigma = sigma;
        }

        /// <summary>
        /// Mutates the chromosome using Guassian mutation.
        /// </summary>
        /// <param name="chromosome">Chromosome to mutate.</param>
        /// <returns>IChromosome.</returns>
        public override IChromosome Mutate(IChromosome chromosome)
        {
            var clone = chromosome.Clone();

            double val = Sampling.GetNormal(this.Mu, this.Sigma);

            if (this.Compound)
            {
                val += clone.Sequence[this.N];
            }

            clone.Sequence[this.N] = (this.Range != null ? this.Range.Clip(val) : val);

            return clone;
        }
    }
}
