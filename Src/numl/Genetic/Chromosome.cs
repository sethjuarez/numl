using System;
using System.Collections.Generic;
using System.Text;
using numl.Math.LinearAlgebra;

namespace numl.Genetic
{
    /// <summary>
    /// Chromosome.
    /// </summary>
    public class Chromosome : IChromosome
    {
        private static int _id = -1;

        /// <summary>
        /// Gets or sets the unique identifer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the weight of the chromosome.
        /// <para>The weight is the normalized fitness of the chromosome, i.e.
        /// a greater fit corresponds to a higher weight.</para>
        /// </summary>
        public double Weight { get; set; } = 0;

        /// <summary>
        /// Gets or sets the fitness value.
        /// </summary>
        public double Fitness { get; set; }

        /// <summary>
        /// Gets the generation of this chromosome.
        /// </summary>
        public int Generation { get; set; } = 0;

        /// <summary>
        /// Gets or sets the genetic sequence of this chromosome.
        /// </summary>
        public Vector Sequence { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Chromosome()
        {
            this.Id = (++_id);
        }

        /// <summary>
        /// Initializes a new Chromosome using a random starting sequence.
        /// </summary>
        /// <param name="size">Size of the sequence.</param>
        /// <param name="minVal">Minimum gene phenotype value, i.e. zero for binary chromosomes.</param>
        /// <param name="maxVal">Maximum gene phenotype value, i.e. one for binary chromosomes.</param>
        public Chromosome(int size, double minVal, double maxVal) : this()
        {
            this.Sequence = Vector.Rand(size, minVal, maxVal);
        }

        /// <summary>
        /// Returns a clone of this chromosome.
        /// </summary>
        /// <returns>IChromosome.</returns>
        public virtual IChromosome Clone()
        {
            return new Chromosome()
            {
                Weight = this.Weight,
                Fitness = this.Fitness,
                Sequence = this.Sequence.Copy(),
                Generation = this.Generation + 1
            };
        }

        /// <summary>
        /// Converts the current object to a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => 
            $"C:{this.Id} [Gen: {this.Generation}, Weight: {this.Weight}, Fitness: {this.Fitness}]";

        /// <summary>
        /// Compares the object with the current and returns a value indicating equality, 
        /// where 0 being equal, -1 less than <paramref name="obj"/> and 1 being greater than <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns>Int.</returns>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            
            double weight = ((Chromosome)obj).Weight;

            return this.Weight.CompareTo(weight);
        }
    }
}
