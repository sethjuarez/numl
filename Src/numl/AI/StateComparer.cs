using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI
{
    /// <summary>
    /// Class StateComparer.
    /// </summary>
    public class StateComparer : IEqualityComparer<IState>
    {

        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Equals(IState x, IState y)
        {
            return x.IsEqualTo(y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(IState obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Returns an int indicating whether the <paramref name="y"/> is less than, equal to or greater than <paramref name="x"/>.
        /// </summary>
        /// <param name="x">First state.</param>
        /// <param name="y">Second state to compare.</param>
        /// <returns>Int.</returns>
        public static int Compare(IState x, IState y)
        {
            return Comparer<int>.Default.Compare(x.Id, y.Id);
        }
    }
}
