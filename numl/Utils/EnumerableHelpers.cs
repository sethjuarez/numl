using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Utils
{
    /// <summary>
    /// Extension methods for IEnumerable collections
    /// </summary>
    public static class EnumerableHelpers
    {
        /// <summary>
        /// Performs the specified action and returns the result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="source">Source array</param>
        /// <param name="fnTransform">Function to apply to each element</param>
        /// <returns></returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> source, Func<T, R> fnTransform)
        {
            foreach (var t in source)
            {
                yield return fnTransform(t);
            }
        }

        /// <summary>
        /// Gets the index of the specified item in the source array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Source array</param>
        /// <param name="item">Object to test for.</param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T item)
        {
            int index = -1;

            for (int i = 0; i < source.Count(); i++)
            {
                if (source.ElementAt(i).Equals(item))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// Randomly shuffles the indexing of the source array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            Random random = new Random(42);

            return source.OrderBy(o => random.NextDouble());
        }

        /// <summary>
        /// Returns the slice of objects using the specified index arrays.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> source, IEnumerable<int> indices)
        {
            foreach (var index in indices)
                yield return source.ElementAt(index);
        }

        /// <summary>
        /// Calculates the slope of the source array and returns True if the values are increasing.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsAscending(this IEnumerable<double> source)
        {
            return (source.ElementAt(0) - source.ElementAt(source.Count() - 1)) < 0;
        }

        /// <summary>
        /// Calculates the standard deviation on the source collection
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="fnPropSelector"></param>
        /// <param name="isSamplePopulation"></param>
        /// <returns></returns>
        public static double StandardDeviation<TSource>(this IEnumerable<TSource> source, Func<TSource, double> fnPropSelector, bool isSamplePopulation = false)
        {
            return System.Math.Sqrt(EnumerableHelpers.Variance(source, fnPropSelector, isSamplePopulation));
        }

        /// <summary>
        /// Calculates the variance on the source collection
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="fnPropSelector"></param>
        /// <param name="isSamplePopulation"></param>
        /// <returns></returns>
        public static double Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, double> fnPropSelector, bool isSamplePopulation = false)
        {
            return source.Select(s => System.Math.Pow(fnPropSelector(s) - source.Average(fnPropSelector), 2)).Sum() / (isSamplePopulation ? source.Count() - 1 : source.Count());
        }
    }
}
