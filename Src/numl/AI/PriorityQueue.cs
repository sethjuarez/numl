using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI
{
    class PriorityQueue<P, V>
    {
        private SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();
        private int _count = 0;
        /// <summary>
        /// Enqueues the specified priority.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="value">The value.</param>
        public void Enqueue(P priority, V value)
        {
            Queue<V> q;
            if (!list.TryGetValue(priority, out q))
            {
                q = new Queue<V>();
                list.Add(priority, q);
            }
            q.Enqueue(value);
            _count++;
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns>V.</returns>
        public V Dequeue()
        {
            // will throw if there isn’t any first element!
            var pair = list.First();
            var v = pair.Value.Dequeue();
            if (pair.Value.Count == 0) // nothing left of the top priority.
                list.Remove(pair.Key);
            _count--;
            return v;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return !list.Any(); }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                return _count;
            }
        }
    }
}
