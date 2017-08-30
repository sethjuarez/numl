using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using numl.Math.Probability;
using numl.Math.LinearAlgebra;

namespace numl.Math.Filters
{
    /// <summary>
    /// A Proportional Sampling filter, otherwise known as Roulette Wheel sampling.
    /// </summary>
    public class ProportionalFilter : IFilter
    {
        /// <summary>
        /// Samples objects according to their probability using the roulette wheel sampling method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects">Objects to sample.</param>
        /// <param name="zeta">Vector of probabilities for each object.</param>
        /// <returns></returns>
        public IEnumerable<T> Filter<T>(IEnumerable<T> objects, Vector zeta)
        {           
            int N = objects.Count();
            int index = Sampling.GetUniform(max: N);

            double beta = 0d;
            double max = 0d;

            var norm = zeta.Normalize();

            foreach (var obj in objects)
            {
                beta += Sampling.GetUniform() * 2.0 * max;
                while (beta > norm.ElementAt(index))
                {
                    beta -= norm.ElementAt(index);
                    index = (index + 1) % N;
                }

                yield return objects.ElementAt(index);
            }
        }
    }
}
