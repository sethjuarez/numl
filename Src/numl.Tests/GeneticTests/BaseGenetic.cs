using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

using numl.Math.Probability;
using numl.Math.LinearAlgebra;
using numl.Genetic;
using numl.Genetic.Functions.Crossover;
using numl.Genetic.Functions.Mutation;

namespace numl.Tests.GeneticTests
{
    public abstract class BaseGenetic
    {

        public void Test_Mutation<T>(Func<T> factory, bool binary, int length, Func<IChromosome, IChromosome, bool> fnTest,
                                     Func<int, double> fnSequenceInitializer = null) 
                                     where T : IMutationFunction
        {
            var s = (fnSequenceInitializer == null ? 
                        Vector.Create(length, i => (binary ? (i % 2 == 0 ? 1 : 0) : (i + 1))) 
                        : Vector.Create(length, fnSequenceInitializer));

            var parent = new Chromosome()
            {
                Sequence = s.Copy()
            };

            var mutator = factory();

            var clone = mutator.Mutate(parent);

            bool result = fnTest(parent, clone);

            Assert.True(result);
        }

        public void Test_Crossover<T>(Func<T> factory, int length, Func<IChromosome, IChromosome, IChromosome, bool> fnTest) 
                                      where T : ICrossoverFunction
        {
            ICrossoverFunction crossover = factory();

            var s1 = Vector.Create(length, i => (i + 1) * 100);
            var parent1 = new Chromosome()
            {
                Sequence = s1.Copy()
            };

            var s2 = Vector.Create(length, i => (i + 1));
            var parent2 = new Chromosome()
            {
                Sequence = s2.Copy()
            };

            var child = crossover.Crossover(parent1, parent2);

            bool result = fnTest(parent1, parent2, child);

            Assert.True(result);
        }
    }
}
