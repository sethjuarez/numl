using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI
{
    /// <summary>
    /// ActionComparer class.
    /// </summary>
    public class ActionComparer : IEqualityComparer<IAction>
    {

        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns><c>true</c> if is equal, <c>false</c> otherwise.</returns>
        public bool Equals(IAction x, IAction y)
        {
            return (x.Id == y.Id);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(IAction obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Returns an int indicating whether the <paramref name="y"/> is less than, equal to or greater than <paramref name="x"/>.
        /// </summary>
        /// <param name="x">First action.</param>
        /// <param name="y">Second action to compare.</param>
        /// <returns>Int.</returns>
        public static int Compare(IAction x, IAction y)
        {
            if (y == null) return -1;
            if (x.Id == y.Id) return 0;

            return (x.Id < y.Id ? -1 : 1);
        }
    }
}
