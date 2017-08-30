using System;
using System.Collections.Generic;
using System.Text;

using numl.Math.Probability;

namespace numl.Genetic.Functions.Mutation
{
    /// <summary>
    /// Implements a chained mutation function.
    /// </summary>
    public class MutationChain : IMutationFunction
    {
        /// <summary>
        /// Gets or sets the genetic sequence length.
        /// </summary>
        public int SequenceLength { get; set; } = 0;

        /// <summary>
        /// Gets the mutation chain.
        /// </summary>
        public ICollection<IMutationFunction> Mutations { get; protected set; }

        /// <summary>
        /// Initializes a new mutation chain for chaining.
        /// </summary>
        /// <param name="mutations">Existing mutators.</param>
        /// <param name="sequenceLength">Length of the sequence.</param>
        internal MutationChain(IEnumerable<IMutationFunction> mutations, int sequenceLength)
        {
            this.SequenceLength = sequenceLength;
            this.Mutations = new List<IMutationFunction>(mutations);
        }

        /// <summary>
        /// Initializes.
        /// </summary>
        public MutationChain(int sequenceLength)
        {
            this.Mutations = new List<IMutationFunction>();
            this.SequenceLength = sequenceLength;
        }

        /// <summary>
        /// Adds a mutation function to the chain.
        /// <para>The mutator is required to handle sequence ranges.</para>
        /// </summary>
        /// <param name="mutator">Mutation function.</param>
        /// <returns></returns>
        public MutationChain Add(IMutationFunction mutator)
        {
            var mutations = new List<IMutationFunction>(this.Mutations)
            {
                mutator
            };

            return new MutationChain(mutations, this.SequenceLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fnInitializer"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public MutationChain Add<T>(Func<T> fnInitializer, int index) where T : MutationBase
        {
            if (index >= this.SequenceLength)
                throw new ArgumentOutOfRangeException(nameof(index), 
                    "Index for gene mutation was greater than the sequence length.");

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Index was less than zero");

            T mutator = fnInitializer();
            mutator.N = index;

            MutationChain result = this.Add(mutator);
            return result;
        }

        /// <summary>
        /// Adds a mutation function to the chain at a random gene location.
        /// </summary>
        /// <param name="fnInitializer">Mutation function.</param>
        /// <param name="copies">Number of copies, at random positions, to append to the chain.</param>
        /// <returns>MutationChain.</returns>
        public MutationChain AddRandom<T>(Func<T> fnInitializer, int copies = 1) where T : MutationBase
        {
            MutationChain chain = this;

            for (int i = 0; i < copies; i++)
            {
                chain = chain.Add(fnInitializer, Sampling.GetUniform(0, this.SequenceLength - 1));
            }

            return chain;
        }

        /// <summary>
        /// Initializes a new chain using a sequence length as reference for chaining.
        /// </summary>
        /// <param name="sequenceLength">Length of the genetic sequence.</param>
        /// <returns>MutationChain.</returns>
        public static MutationChain New(int sequenceLength)
        {
            return new MutationChain(sequenceLength);
        }

        /// <summary>
        /// Mutates the chromosome using the current chain.
        /// </summary>
        /// <param name="chromosome">Chromosome to mutate.</param>
        /// <returns>IChromosome.</returns>
        public IChromosome Mutate(IChromosome chromosome)
        {
            var clone = chromosome.Clone();

            foreach (var mutator in this.Mutations)
            {
                clone = mutator.Mutate(clone);
            }

            return clone;
        }
    }
}
