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
    }
}
