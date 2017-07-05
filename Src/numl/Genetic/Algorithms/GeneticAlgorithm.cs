using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using numl.Math.Probability;
using numl.Math.LinearAlgebra;

using numl.Genetic.Metrics;
using numl.Genetic.Functions.Mutation;
using numl.Genetic.Functions.Crossover;
using numl.Genetic.Functions.Selection;
using numl.Genetic.Functions.Pairing;

namespace numl.Genetic.Algorithms
{
    /// <summary>
    /// GeneticAlgorithm class.
    /// </summary>
    public class GeneticAlgorithm : IGeneticAlgorithm
    {
        /// <summary>
        /// Gets or sets the rate of mutation during evolution.
        /// </summary>
        public double MutationRate { get; set; } = 0.15;
        
        /// <summary>
        /// Gets or sets the Mutation Function applied in subsequent generations.
        /// </summary>
        public IMutationFunction Mutation { get; set; }

        /// <summary>
        /// Gets or sets the rate of crossover applied during evolution.
        /// </summary>
        public double CrossoverRate { get; set; } = 0.8;

        /// <summary>
        /// Gets or sets the Crossover Function used in performing genetic crossover during evolution.
        /// </summary>
        public ICrossoverFunction Crossover { get; set; }

        /// <summary>
        /// Gets or sets the rate of elitist chromosomes carried over through subsequent generations. 
        /// </summary>
        public double ElitistRate { get; set; } = 0.05;

        /// <summary>
        /// Gets or sets the Selection Function used in filtering the chromosomes to survive through to the next generation.
        /// </summary>
        public ISelectionFunction Selection { get; set; }

        /// <summary>
        /// Gets or sets the Pairing function used to select pairs of chromosomes for performing crossover.
        /// </summary>
        public IPairingFunction Pairing { get; set; }

        /// <summary>
        /// Initializes a default Genetic Algorith.
        /// </summary>
        public GeneticAlgorithm()
        {
            this.Mutation = new GaussianMutation();
            this.Crossover = new PointCrossover(3);
            this.Selection = new RouletteSelection();
            this.Pairing = new TournamentPairing();
        }

        /// <summary>
        /// Initializes a new instance of a GeneticAlgorithm.
        /// </summary>
        /// <param name="mutationFunction">Mutation function to apply during evolution.</param>
        /// <param name="crossoverFunction">Crossover Function used during evolution.</param>
        /// <param name="selectionFunction">Selection Function used in chromosome selection.</param>
        /// <param name="pairingFunction">Pairing function used to select pairs of chromosomes during crossover.</param>
        public GeneticAlgorithm(IMutationFunction mutationFunction, ICrossoverFunction crossoverFunction, 
                                ISelectionFunction selectionFunction, IPairingFunction pairingFunction)
        {
            this.Mutation = mutationFunction;
            this.Crossover = crossoverFunction;
            this.Selection = selectionFunction;
            this.Pairing = pairingFunction;
        }

        /// <summary>
        /// Evolves using a standard genetic algorithm.
        /// </summary>
        /// <param name="chromosomes">Chromosomes to undergo evolution.</param>
        /// <param name="fitnessMetric">Fitness metric to use for evaluation.</param>
        /// <param name="fitnessMode">Fitness mode for evaluating performance.</param>
        public virtual IEnumerable<IChromosome> Evolve(IEnumerable<IChromosome> chromosomes, IFitnessMetric fitnessMetric, FitnessMode fitnessMode)
        {
            int count = chromosomes.Count();
            var candidates = new List<IChromosome>();

            int elites = (int)System.Math.Ceiling(chromosomes.Count() * this.ElitistRate);
            var topCandidates = (chromosomes.OrderByDescending(o => o.Weight).Take(elites));

            foreach (var pair in this.Pairing.Pair(chromosomes))
            {
                (IChromosome c1, IChromosome c2) = pair;
                
                IChromosome candidate = null;

                double rand = Sampling.GetUniform();
                if (rand < this.CrossoverRate)
                {
                    candidate = this.Crossover.Crossover(c1, c2);
                }

                double selector = Sampling.GetUniform();
                candidate = candidate ?? (selector <= 0.5 ? c1 : c2);

                if (rand < this.MutationRate)
                {
                    candidate = this.Mutation.Mutate(candidate.Clone());
                }

                candidates.Add(candidate);
            }

            foreach (var elite in topCandidates)
            {
                candidates.Add(elite);
            }

            var result = this.Selection.Select(candidates, fitnessMode).Take(count);

            return result;
        }
    }
}
