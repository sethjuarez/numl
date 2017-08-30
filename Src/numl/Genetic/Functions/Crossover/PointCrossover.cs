using numl.Math.Probability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Genetic.Functions.Crossover
{
    /// <summary>
    /// PointCrossover function.
    /// </summary>
    public class PointCrossover : ICrossoverFunction
    {
        private bool _Initialized = false;

        /// <summary>
        /// Gets or sets the points of crossover between chromosomes, i.e. an index array in the sequence.
        /// <para>If the array sequence is zeros of n length, random points will be chosen and used in subsequent crossovers.</para>
        /// </summary>
        public int[] Points { get; set; }

        /// <summary>
        /// Gets or sets the probability of selecting the alternating parent between segments (default is 1.0).
        /// </summary>
        public double Probability { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets whether the segmentation points are dynamically updated during subsequent crossovers.
        /// </summary>
        public bool IsDynamic { get; set; } = false;

        /// <summary>
        /// Initializes a new point crossover function.
        /// </summary>
        /// <param name="points">Number of segmentation points between parent chromosomes.</param>
        public PointCrossover(int points = 1)
        {
            this.Points = new int[points];
        }

        /// <summary>
        /// Initializes the segmentation points of the crossover.
        /// </summary>
        /// <param name="length">Number of segmentation points.</param>
        /// <param name="reset">(Optional) Recreates the points if already set.</param>
        public void InitPoints(int length, bool reset = false)
        {
            if (reset || !this._Initialized)
            {
                int min = 0;
                for (int i = 1; i <= this.Points.Length; i++)
                {
                    int point = Sampling.GetUniform(min, (i * (length / this.Points.Length)));
                    min = point + 1;

                    this.Points[i - 1] = point;
                }

                this._Initialized = true;
            }
        }

        /// <summary>
        /// Performs crossover using point selection.
        /// </summary>
        /// <param name="chromosome1">First parent.</param>
        /// <param name="chromosome2">Second parent.</param>
        /// <returns></returns>
        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            this.InitPoints(chromosome1.Sequence.Length, this.IsDynamic);

            IChromosome parent = chromosome1;
            IChromosome clone = parent.Clone();

            int split = 0;
            for (int i = 0; i < chromosome1.Sequence.Length; i++)
            {
                if (i == this.Points[split])
                {
                    // striated genes
                    double p = Sampling.GetUniform();

                    if (p <= this.Probability) 
                        parent = (parent == chromosome1 ? chromosome2 : chromosome1);

                    if (split < this.Points.Length - 1)
                        split++;
                }
                // do crossover
                clone.Sequence[i] = parent.Sequence[i];
            }

            return clone;
        }
    }
}
