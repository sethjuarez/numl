using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.AI.Collections
{
    /// <summary>
    /// A sorted data table.
    /// </summary>
    /// <typeparam name="TKey1">Parent key type.</typeparam>
    /// <typeparam name="TKey2">Child key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    public class SortedTable<TKey1, TKey2, TValue>
    {
        private readonly SortedDictionary<TKey1, SortedDictionary<TKey2, TValue>> _Table;

        /// <summary>
        /// Gets or sets the default table value.
        /// </summary>
        public TValue DefaultValue { get; set; }

        /// <summary>
        /// Returns all the keys in the current collection.
        /// </summary>
        public IEnumerable<TKey1> Keys
        {
            get { return this._Table.Keys; }
        }

        /// <summary>
        /// Gets or sets the value for the key-key pair.  Returns the default value if one is not found at the specified location.
        /// </summary>
        /// <param name="key1">Parent key.</param>
        /// <param name="key2">Child key.</param>
        /// <returns><typeparamref name="TValue"/>.</returns>
        public TValue this[TKey1 key1, TKey2 key2]
        {
            get
            {
                if (key1 == null || key2 == null || !this._Table.ContainsKey(key1) || _Table[key1] == null)
                    return this.DefaultValue;
                return this._Table[key1][key2];
            }
            set
            {
                if (this._Table[key1] == null)
                    this._Table[key1] = new SortedDictionary<TKey2, TValue>();

                this._Table[key1][key2] = value;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SortedTable()
        {
            this._Table = new SortedDictionary<TKey1, SortedDictionary<TKey2, TValue>>();
            this.DefaultValue = default(TValue);
        }

        /// <summary>
        /// Returns all associated child keys for the parent key.
        /// </summary>
        /// <param name="key">Parent key.</param>
        /// <returns>IEnumerable&lt;<typeparamref name="TValue"/>&gt;</returns>
        public IEnumerable<TKey2> GetKeys(TKey1 key)
        {
            return this._Table[key]?.Select(s => s.Key);
        }

        /// <summary>
        /// Returns all child key-value pairs for the specified parent key.
        /// </summary>
        /// <param name="key">Parent key.</param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<TKey2, TValue>> GetPairs(TKey1 key)
        {
            return this._Table[key];
        }

        /// <summary>
        /// Returns all values for the given parent key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<TValue> GetValues(TKey1 key)
        {
            return this._Table[key]?.Select(s => s.Value);
        }

        /// <summary>
        /// Adds the specified parent key to the collection.
        /// </summary>
        /// <param name="key">Parent key to add.</param>
        public void AddKey(TKey1 key)
        {
            if (!this._Table.ContainsKey(key))
                this._Table.Add(key, new SortedDictionary<TKey2, TValue>());
        }

        /// <summary>
        /// Adds or updates the paired value in the collection.
        /// </summary>
        /// <param name="key">Parent key.</param>
        /// <param name="childKey">Child key.</param>
        /// <param name="value">Value to add.</param>
        public void AddOrUpdate(TKey1 key, TKey2 childKey, TValue value)
        {
            if (!this._Table.ContainsKey(key))
                this._Table.Add(key, new SortedDictionary<TKey2, TValue>());

            if (!this._Table[key].ContainsKey(childKey))
                this._Table[key].Add(childKey, value);
            else
                this._Table[key][childKey] = value;
        }

        /// <summary>
        /// Returns <c>True</c> if the specified key exists in the collection, otherwise <c>False</c>.
        /// </summary>
        /// <param name="key">Parent key.</param>
        /// <returns>Boolean.</returns>
        public bool ContainsKey(TKey1 key)
        {
            return this._Table.ContainsKey(key);
        }

        /// <summary>
        /// Returns <c>True</c> if the specified keys exist in the collection, otherwise <c>False</c>.
        /// </summary>
        /// <param name="key">Parent key.</param>
        /// <param name="childKey">Child key.</param>
        /// <returns>Boolean.</returns>
        public bool ContainsKey(TKey1 key, TKey2 childKey)
        {
            return this.ContainsKey(key) && this._Table[key].ContainsKey(childKey);
        }

        /// <summary>
        /// Removes all child elements, including the parent, by the specified parent key.
        /// </summary>
        /// <param name="key">Parent key.</param>
        public void Remove(TKey1 key)
        {
            this._Table.Remove(key);
        }

        /// <summary>
        /// Removes only the value at the specified location.
        /// </summary>
        /// <param name="key">Parent key.</param>
        /// <param name="childKey">Child key.</param>
        public void Remove(TKey1 key, TKey2 childKey)
        {
            if (this._Table.ContainsKey(key))
                this._Table[key].Remove(childKey);
        }
    }
}
