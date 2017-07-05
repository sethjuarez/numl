using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace numl.AI.Collections
{
    /// <summary>
    /// Represents a limited collection of objects that is maintained in sorted order and allows duplicate elements.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public class NSortedList<T> : IEnumerable<T>, ICollection<T>
    {
        private const int _defaultSize = 4;

        private IComparer<T> _Comparer;
        private int _Length = 0;
        private T[] _Items = new T[_defaultSize];
        private int _Limit = -1;
        private int _Version = 0;
        private bool _Reverse = false;

        /// <summary>
        /// Gets or sets the limit of the collection.  Once the limit has been reached, elements will be replaced 
        /// in the collection as determined by the sorting strategy.
        /// </summary>
        public int Limit
        {
            get
            {
                return this._Limit;
            }
            set
            {
                this._Limit = value;
                if (this._Length > this._Limit)
                {
                    this.Trim(true);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the collection is maintained in ascending or descending order.
        /// <para>Default is ascending order (false).</para>
        /// </summary>
        public bool Reverse
        {
            get
            {
                return this._Reverse;
            }
            set
            {
                if (value != this._Reverse)
                {
                    this._Reverse = value;

                    if (this._Reverse)
                    {
                        Array.Reverse(this._Items, 0, this._Length);
                    }
                    else
                    {
                        Array.Sort(this._Items, 0, this._Length, this._Comparer);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the initial capacity that the collection contains.
        /// </summary>
        public int Capacity
        {
            get
            {
                return this._Items.Length;
            }
            set
            {
                if (value != this._Items.Length)
                {
                    if (value < this._Length) throw new ArgumentOutOfRangeException(nameof(value));

                    if (value > 0)
                    {
                        var arr = new T[value];
                        if (this._Length > 0)
                        {
                            Array.Copy(this._Items, 0, arr, 0, this._Length);
                        }
                        this._Items = arr;

                        return;
                    }

                    this._Items = new T[_defaultSize];
                }
            }
        }

        /// <summary>
        /// Gets the comparer used for sorting.
        /// </summary>
        public IComparer<T> Comparer
        {
            get
            {
                return this._Comparer;
            }
        }

        /// <summary>
        /// Initializes a new NSortedList collection.
        /// </summary>
        public NSortedList()
        {
            this._Comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Initializes a new NSortedList collection with a determined limit.
        /// </summary>
        public NSortedList(int limit, bool reverse) : this()
        {
            this.Reverse = reverse;
            this.Limit = limit;
        }

        /// <summary>
        /// Initializes a new NSortedList collection using the specified comparer.
        /// </summary>
        /// <param name="comparer">Comparer to use.</param>
        public NSortedList(IComparer<T> comparer) : this()
        {
            this._Comparer = comparer;
        }

        /// <summary>
        /// Initializes a new NSortedList from the specified source collection.
        /// </summary>
        /// <param name="collection">Source collection to copy from.</param>
        public NSortedList(IEnumerable<T> collection) : this()
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            foreach (T obj in collection)
            {
                this.Add(obj);
            }
        }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public int Count => this._Length;

        /// <summary>
        /// (Ignored) Returns false, indicating the collection is not readonly.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets the minimum item in the collection.
        /// </summary>
        public T Min
        {
            get
            {
                return (this.Reverse ? this[this._Length - 1] : this[0]);
            }
        }

        /// <summary>
        /// Gets the maximum item in the collection.
        /// </summary>
        public T Max
        {
            get
            {
                return (this.Reverse ? this[0] : this[this._Length - 1]);
            }
        }

        /// <summary>
        /// Gets the value at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return this._Items[index];
            }
        }

        /// <summary>
        /// Searches for the index of the specified item in the collection.
        /// </summary>
        /// <param name="item">Item to locate the zero-based index in the collection.</param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            int index = this.GetHeuristic(item);
            if (index < 0)
            {
                return -1;
            }

            index = this.GetItemIndex(index, item);

            return index;
        }

        /// <summary>
        /// Retrieves the index heuristic.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private int GetHeuristic(T item)
        {
            IComparer<T> comparer = (this._Reverse ? new ReverseComparer(this._Comparer) : this._Comparer);

            int index = Array.BinarySearch<T>(this._Items, 0, this._Length, item, comparer);

            return index;
        }

        /// <summary>
        /// Returns the index of an item using the initial heuristic.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private int GetItemIndex(int offset, T item)
        {
            if (offset >= 0 && offset < this._Length - 1)
            {
                int index = offset;
                do
                {
                    index += 1;

                    if (this._Items[index].Equals(item))
                        break;
                }
                while (index < this._Length);

                return index;
            }

            return offset;
        }

        /// <summary>
        /// Trims and resizes the collection according to the specified limit.
        /// </summary>
        private void Trim(bool cycle)
        {
            if (this._Limit > 0)
            {
                do
                {
                    if (this._Length > this._Limit)
                    {
                        int index = (this._Length - 1);
                        this.RemoveAt(index);
                    }
                    else
                    {
                        break;
                    }
                }
                while (cycle);
            }
        }

        /// <summary>
        /// Trims excess elements.
        /// </summary>
        public void TrimExcess()
        {
            int length = (this._Limit > 0 ? this._Limit : (int)(((int)this._Items.Length) * 0.9));
            if (this._Length < length)
            {
                this.Capacity = this._Length;
            }
        }

        private void EnsureCapacity(int min)
        {
            int num = (this._Items.Length == 0 ? _defaultSize : this._Items.Length + _defaultSize);

            num = (System.Math.Min(2146435071, System.Math.Min(min, num)));

            this.Capacity = num;
        }

        /// <summary>
        /// Removes all elements from the collection. 
        /// </summary>
        public void Clear()
        {
            Array.Clear(this._Items, 0, this._Length);
            this._Length = 0;
            this._Version += 1;
        }
        
        /// <summary>
        /// Adds the element to the list.
        /// </summary>
        /// <param name="item">Item to add to the collection.</param>
        public void Add(T item)
        {
            int index = this.GetHeuristic(item);

            if (index < 0)
            {
                index = ~index;
            }

            this.InsertAt(index, item);
        }

        private void InsertAt(int index, T item)
        {
            if (this._Length == (int) this._Items.Length)
            {
                this.EnsureCapacity(this._Length + 1);
            }

            if (index < this._Length)
            {
                Array.Copy(this._Items, index, this._Items, index + 1, this._Length - index);
            }

            this._Items[index] = item;
            this._Length += 1;
            this._Version += 1;

            this.Trim(false);
        }

        /// <summary>
        /// Removes all instances of the element from the collection.
        /// </summary>
        /// <param name="item">Element to remove.</param>
        /// <returns>Boolean.</returns>
        public bool Remove(T element)
        {
            int index = this.GetHeuristic(element);

            if (index >= 0)
            {
                while (index > 0 && this[index - 1].Equals(element))
                    index -= 1;

                do
                {
                    this.RemoveAt(index);
                }
                while (this[index].Equals(element));
            }

            return (index >= 0);
        }

        private void RemoveAt(int index)
        {
            if (index < 0 || index > this._Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            this._Length -= 1;

            if (index < this._Length)
            {
                Array.Copy(this._Items, index + 1, this._Items, index, this._Length - index);
            }

            this._Items[this._Length] = default(T);
            this._Version += 1;
        }

        /// <summary>
        /// Determines whether the specified key/value pair exists in the collection.
        /// </summary>
        /// <param name="item">Item to find.</param>
        /// <returns>Boolean.</returns>
        public bool Contains(T item)
        {
            return (this.IndexOf(item) >= 0);
        }

        /// <summary>
        /// Copies the elements from the current collection into the destination array.
        /// </summary>
        /// <param name="array">Destination array.</param>
        /// <param name="arrayIndex">Starting index to insert from in the array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex > (int) array.Length)
                throw new ArgumentOutOfRangeException("Index out of range.", nameof(arrayIndex));
            if ((array.Length - arrayIndex) < this.Count)
                throw new ArgumentException("Destination array length was less than the collection.", nameof(array));

            for (int i = 0; i < this.Count; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        #region Enumerator

        /// <summary>
        /// Returns an enumerator over the collection.
        /// </summary>
        /// <returns>IEnumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator over the collection of key-value pairs.
        /// </summary>
        /// <returns>IEnumerator&lt;T&gt;.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new NSortedList<T>.Enumerator(this);
        }

        private struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private NSortedList<T> sortedList;
            private int index;
            private T value;

            int version;

            public T Current => this.value;

            object IEnumerator.Current => this.value;

            internal Enumerator(NSortedList<T> sortedList)
            {
                this.sortedList = sortedList;
                this.value = default(T);
                this.index = 0;
                this.version = sortedList._Version;
            }

            private void Validate()
            {
                if (this.version != sortedList._Version)
                    throw new InvalidOperationException("Enumeration expired.");
            }

            public void Dispose()
            {
                this.index = 0;
                this.value = default(T);
            }

            public bool MoveNext()
            {
                this.Validate();

                if (this.index >= this.sortedList.Count)
                {
                    this.index = this.sortedList.Count + 1;
                    this.value = default(T);
                    return false;
                }

                this.value = this.sortedList[this.index];
                this.index += 1;

                return true;
            }

            public void Reset()
            {
                this.Validate();

                this.index = 0;
                this.value = default(T);
            }
        }

        #endregion

        #region Comparator

        private class ReverseComparer : IComparer<T>
        {
            private IComparer<T> _Comparer;

            public ReverseComparer(IComparer<T> comparer)
            {
                this._Comparer = comparer;
            }
            public int Compare(T x, T y)
            {
                return (this._Comparer.Compare(y, x));
            }
        }

        #endregion
    }
}
