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
