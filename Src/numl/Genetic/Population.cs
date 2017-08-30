using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using numl.AI.Collections;
using numl.Math.LinearAlgebra;
using numl.Math.Probability;
using numl.Genetic.Algorithms;
using numl.Genetic.Functions.Crossover;
using numl.Genetic.Functions.Mutation;
using numl.Genetic.Functions.Pairing;
using numl.Genetic.Functions.Selection;
using numl.Genetic.Metrics;
using numl.Utils;

namespace numl.Genetic
{
    /// <summary>
    /// Population object.
    /// </summary>
    public class Population : IEnumerable<IChromosome>
    {

        /// <summary>
        /// Gets the chromosomes in the current pool.
        /// </summary>
        private NSortedList<IChromosome> _Chromosomes;

        /// <summary>
        /// Gets or sets the Fitness mode for evaluating chromosome performance.
        /// </summary>
        public FitnessMode FitnessMode
        {
            get
            {
                return (this._Chromosomes.Reverse ? FitnessMode.Maximize : FitnessMode.Minimize);
            }
            set
            {
                this._Chromosomes.Reverse = (value == FitnessMode.Maximize ? true : false);
            }
        }

        /// <summary>
        /// Gets or sets the Fitness Metric used in evolution.
        /// </summary>
        public IFitnessMetric FitnessMetric { get; set; }

        /// <summary>
        /// Gets the estimated generation of the current population.
        /// </summary>
        public double Generation
        {
            get
            {
                return this.Average(s => s.Generation);
            }
        }

        /// <summary>
        /// Determines whether the population size can expand or contract across generations.
        /// </summary>
        public bool IsDynamic { get; set; } = false;

        /// <summary>
        /// Gets the best chromosome from the pool.
        /// </summary>
        public IChromosome Best
        {
            get
            {
                return this._Chromosomes.First();
            }
        }

        /// <summary>
        /// Gets the worst performing chromosome from the pool.
        /// </summary>
        public IChromosome Worst
        {
            get
            {
                return this._Chromosomes.Last();
            }
        }

        /// <summary>
        /// Gets the mean fitness value for the population.
        /// </summary>
        public double MeanFitness
        {
            get
            {
                return this.Average(m => m.Fitness);
            }
        }

        /// <summary>
        /// Gets the accumulated fitness for the population.
        /// </summary>
        public double Cost
        {
            get
            {
                return this.Sum(s => s.Fitness);
            }
        }

        /// <summary>
        /// Gets the oldest generation in the population.
        /// </summary>
        public int MinGeneration
        {
            get
            {
                return this._Chromosomes.Min(m => m.Generation);
            }
        }

        /// <summary>
        /// Gets the youngest generation in the population.
        /// </summary>
        public int MaxGeneration
        {
            get
            {
                return this._Chromosomes.Max(m => m.Generation);
            }
        }

        /// <summary>
        /// Returns the number of chromosomes in the population.
        /// </summary>
        public int Count => this._Chromosomes.Count;

        /// <summary>
        /// Returns the chromosome at the specified index.
        /// </summary>
        /// <param name="index">Index of the item to retrieve.</param>
        /// <returns>IChromosome.</returns>
        public IChromosome this[int index]
        {
            get
            {
                return this._Chromosomes[index];
            }
        }

        protected Population(IFitnessMetric fitnessMetric, FitnessMode fitnessMode)
        {
            this.FitnessMetric = fitnessMetric;
            this.FitnessMode = fitnessMode;
        }

        /// <summary>
        /// Initializes a new Population using existing chromosomes.
        /// </summary>
        /// <param name="chromosomes">Chromosomes to use in the population.</param>
        /// <param name="fitnessMetric">Fitness function for evaluating chromosomes.</param>
        /// <param name="fitnessMode">Fitness mode.</param>
        public Population(IEnumerable<IChromosome> chromosomes, IFitnessMetric fitnessMetric, FitnessMode fitnessMode)
            : this(fitnessMetric, fitnessMode)
        {
            this.Apply(chromosomes);
        }

        /// <summary>
        /// Initializes a new Population using a factory and the specified fitness.
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="poolSize">Size of the pool to create.</param>
        /// <param name="fnInitializer">Initialization function to be called for each new chromosome.</param>
        /// <param name="fitnessMetric">Fitness function for evaluating chromosomes.</param>
        /// <param name="fitnessMode">Fitness mode.</param>
        public Population(int poolSize, Func<IChromosome> fnInitializer, IFitnessMetric fitnessMetric, FitnessMode fitnessMode)
            : this(fitnessMetric, fitnessMode)
        {
            var chromosomes = new IChromosome[poolSize];

            // init pool
            for (int i = 0; i < poolSize; i++)
            {
                var clone = fnInitializer();
                clone.Fitness = this.FitnessMetric.Fitness(clone);
                clone.Generation = 0;

                chromosomes[i] = clone;
            }

            this.Apply(chromosomes);
        }

        /// <summary>
        /// Initializes the genetic pool using the specified seed and mutation functions.
        /// </summary>
        /// <param name="poolSize">Size of the pool to create.</param>
        /// <param name="seed">Starting seed value.</param>
        /// <param name="seedMutator">Mutator used during initialization of the pool.</param>
        /// <param name="fitnessMetric">Fitness function for evaluating chromosomes.</param>
        /// <param name="fitnessMode">Fitness mode.</param>
        public Population(int poolSize, IChromosome seed, IMutationFunction seedMutator, IFitnessMetric fitnessMetric, FitnessMode fitnessMode)
            : this(poolSize, () => seedMutator.Mutate(seed), fitnessMetric, fitnessMode)
        { }

        /// <summary>
        /// Applies the pool to the current population object.
        /// </summary>
        /// <param name="pool"></param>
        protected void Apply(IEnumerable<IChromosome> pool)
        {
            this._Chromosomes.Clear();

            Vector weights = this._Chromosomes.Select(s => s.Fitness)
                                             .ToVector()
                                             .Normalize();

            // increase weights according to fitness mode
            if (this.FitnessMode == FitnessMode.Minimize)
            {
                weights = (1.0 - weights).Pow(2);
                weights = weights / weights.Sum();
            }

            for (int i = 0; i < pool.Count(); i++)
            {
                var chromosome = pool.ElementAt(i);
                chromosome.Weight = weights[i];

                this._Chromosomes.Add(chromosome);
            }
        }

        /// <summary>
        /// Runs one generation of genetic evolution on the population using the supplied solver.
        /// </summary>
        /// <param name="algorithm">Genetic algorithm to use during evolution.</param>
        public virtual void Evolve(IGeneticAlgorithm algorithm)
        {
            var nextGeneration = algorithm.Evolve(this._Chromosomes, this.FitnessMetric, this.FitnessMode);
            
            int size = (this.IsDynamic ? nextGeneration.Count() : this._Chromosomes.Count);
            var newGeneration = new IChromosome[size];

            for (int i = 0; i < size; i++)
            {
                IChromosome current;

                if (i >= nextGeneration.Count() && !this.IsDynamic)
                {
                    current = this.Random();
                }
                else
                {
                    current = nextGeneration.ElementAt(i);
                }

                newGeneration[i] = current;
            }

            this.Apply(newGeneration);
        }

        /// <summary>
        /// Merges a population with the current one.
        /// </summary>
        /// <param name="population">Population to merge with.</param>
        /// <param name="newPoolSize">Size of the pool to create, including the current pool and merging pool (defaults to current pool size).</param>
        /// <param name="balance">Balance rate of the first population pool (percentage).  A higher rate will increase the selection rate of chromosomes in the first pool.</param>
        /// <param name="elites">Rate of elitist chromosomes in the resulting population from the previous pools (percentage).</param>
        /// <returns>Population.</returns>
        public virtual Population Merge(Population population, int newPoolSize, double balance = 0.5, double elites = 0.1)
        {
            if (balance > 1) balance /= 100;
            if (elites > 1) elites /= 100;

            int poolSize = (newPoolSize > 0 ? newPoolSize : this._Chromosomes.Count);

            int eliteCount = (int) System.Math.Ceiling(poolSize * elites);

            var population1 = this._Chromosomes;
            var population2 = population;

            var result = new List<IChromosome>(poolSize);

            result.AddRange(population1.Take(eliteCount));
            result.AddRange(population2.Take(eliteCount));

            poolSize = poolSize - eliteCount;

            for (int i = result.Count; i < poolSize; i++)
            {
                double rand = Sampling.GetUniform();

                var item = (rand < balance ? population1.Random() : population2.Random());
                result.Add(item);
            }

            return new Population(result, this.FitnessMetric, this.FitnessMode);
        }

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator for the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IChromosome> GetEnumerator()
        {
            return this._Chromosomes.GetEnumerator();
        }

        /// <summary>
        /// Returns the enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
