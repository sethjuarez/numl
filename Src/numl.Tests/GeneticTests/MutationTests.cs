using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using numl.Genetic;
using numl.Genetic.Functions.Mutation;
using numl.Math.LinearAlgebra;
using numl.Utils;

using MATH = System.Math;

namespace numl.Tests.GeneticTests
{
    [Trait("Category", "Genetic")]
    public class MutationTests : BaseGenetic
    {
        

        [Fact]
        public void Test_Mutation_Chain_Build()
        {
            MutationChain chain = new MutationChain(5);

            var flip = new FlipMutation();
            var random = new RandomMutation();
            var gaussian = new GaussianMutation();

            var chain1 = chain.Add(() => flip, 1);
            var chain2 = chain1.Add(random);
            var chain3 = chain2.Add(() => gaussian, 4);
            
            Assert.DoesNotContain(flip, chain.Mutations);
            Assert.Contains(flip, chain1.Mutations);

            Assert.DoesNotContain(random, chain1.Mutations);
            Assert.Contains(random, chain2.Mutations);

            Assert.DoesNotContain(gaussian, chain2.Mutations);
            Assert.Contains(gaussian, chain3.Mutations);
        }

        [Fact]
        public void Test_Mutation_Chain_Fluent()
        {
            var flip = new FlipMutation();
            var random = new RandomMutation();
            var gaussian = new GaussianMutation();

            var chain = MutationChain.New(10)
                .Add(() => flip, 0)
                .Add(() => random, 4)
                .Add(() => gaussian, 6);

            Assert.Equal(flip, chain.Mutations.ElementAt(0));
            Assert.Equal(random, chain.Mutations.ElementAt(1));
            Assert.Equal(gaussian, chain.Mutations.ElementAt(2));
            
            Assert.True(chain.Mutations.Count == 3);
        }

        [Fact]
        public void Test_Mutation_Guassian()
        {
            int count = 0;

            Func<double, double, double, double, double> stderr =
                (double v, double mu, double s, double n) =>
                {
                    return (v - mu) / s;
                };

            while (count < 10)
            {
                int length = Math.Probability.Sampling.GetUniform(10, 100);
                int position = Math.Probability.Sampling.GetUniform(0, length - 1);

                var gaussian1 = new GaussianMutation() { N = position };
                // compound
                this.Test_Mutation(() => gaussian1, false, length, (p1, p2) =>
                {
                    double z = stderr(p2.Sequence[position], p1.Sequence.Mean(), p1.Sequence.StdDev(), length);
                    bool result = (MATH.Abs(z / MATH.Sqrt(length)) < 0.5);

                    return result;
                }, (i) => Math.Probability.Sampling.GetNormal());

                var gaussian2 = new GaussianMutation() { Compound = false, N = 5 };
                // replacement
                this.Test_Mutation(() => gaussian2, false, length, (p1, p2) =>
                {
                    double z = stderr(p2.Sequence[position], p2.Sequence.Mean(), p2.Sequence.StdDev(), length);
                    bool result = (MATH.Abs(z) < 3.1);

                    return result;
                }, (i) => Math.Probability.Sampling.GetNormal());

                count++;
            }
        }

        [Fact]
        public void Test_Mutation_Flip()
        {
            int count = 0;
            int length = 50;

            while (count < 10)
            {
                int position = Math.Probability.Sampling.GetUniform(0, length - 1);

                var flip = new FlipMutation() { N = position };
                // flip
                this.Test_Mutation(() => flip, true, length, (p1, p2) =>
                {
                    bool result = p1.Sequence.IsBinary() && p2.Sequence.IsBinary();
                    result = result && (p1.Sequence[flip.N] != p2.Sequence[flip.N]);

                    double sum = MATH.Abs((p1.Sequence - p2.Sequence).Sum());
                    result = result && (sum == 1);

                    return result;
                });

                count++;
            }
        }
    }
}
