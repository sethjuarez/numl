using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.AI
{
    /// <summary>
    /// Class EnumerableExtensions.
    /// </summary>
    public static class EnumerableExtensions
    {
        private static Random _random = new Random(DateTime.Now.Millisecond);
        /// <summary>
        /// Rands the specified items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns>T.</returns>
        public static T Rand<T>(this IEnumerable<T> items)
        {
            var count = items.Count();
            var d = _random.Next(count);
            return items.ElementAt(d);
        }
    }
}
