// file:	Utils\DictionaryHelpers.cs
//
// summary:	Implements the dictionary helpers class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Utils
{
    /// <summary>A dictionary helpers.</summary>
    public static class DictionaryHelpers
    {
        /// <summary>A Dictionary&lt;K,V&gt; extension method that adds an or update.</summary>
        /// <tparam name="K">Generic type parameter.</tparam>
        /// <tparam name="V">Generic type parameter.</tparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddOrUpdate<K, V>(this Dictionary<K, List<V>> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                if (!dictionary[key].Contains(value))
                    dictionary[key].Add(value);
            }
            else
                dictionary[key] = new List<V> { value };
        }
        /// <summary>A Dictionary&lt;K,V&gt; extension method that adds an or update.</summary>
        /// <tparam name="K">Generic type parameter.</tparam>
        /// <tparam name="V">Generic type parameter.</tparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddOrUpdate<K, V>(this Dictionary<K, V> dictionary, K key, Func<V, V> value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value(dictionary[key]);
            else
                dictionary[key] = value(default(V));
        }
        /// <summary>A Dictionary&lt;K,V&gt; extension method that adds an or update.</summary>
        /// <tparam name="K">Generic type parameter.</tparam>
        /// <tparam name="V">Generic type parameter.</tparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="seed">The seed.</param>
        public static void AddOrUpdate<K, V>(this Dictionary<K, V> dictionary, K key, Func<V, V> value, V seed)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value(dictionary[key]);
            else
                dictionary[key] = value(seed);
        }

        /// <summary>
        /// Adds or updates the value at the specified location.
        /// </summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <typeparam name="V">Value type.</typeparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="key1">The parent key.</param>
        /// <param name="key2">The child key.</param>
        /// <param name="value">The value to add or update.</param>
        public static void AddOrUpdate<K, V>(this Dictionary<K, Dictionary<K, V>> dictionary, K key1, K key2, V value)
        {
            if (dictionary.ContainsKey(key1))
            {
                if (dictionary[key1].ContainsKey(key2))
                    dictionary[key1][key2] = value;
                else
                    dictionary[key1].Add(key2, value);
            }
            else
                dictionary.Add(key1, new Dictionary<K, V> { { key2, value } });
        }
    }
}
