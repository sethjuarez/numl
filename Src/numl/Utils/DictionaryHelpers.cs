// file:	Utils\DictionaryHelpers.cs
//
// summary:	Implements the dictionary helpers class
using System;
using System.Collections.Generic;

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
			List<V> foundList;
			if (!dictionary.TryGetValue(key, out foundList))
			{
				foundList = new List<V> { value };
				dictionary.Add(key, foundList);
			}

			if (!foundList.Contains(value))
				foundList.Add(value);
		}

        /// <summary>A Dictionary&lt;K,V&gt; extension method that adds an or update.</summary>
        /// <tparam name="K">Generic type parameter.</tparam>
        /// <tparam name="V">Generic type parameter.</tparam>
        /// <param name="dictionary">The dictionary to act on.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddOrUpdate<K, V>(this Dictionary<K, V> dictionary, K key, Func<V, V> value)
		{
			V foundValue;
			if (dictionary.TryGetValue(key, out foundValue))
				dictionary[key] = value(foundValue);
			else
                dictionary.Add(key, value(default(V)));
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
			V foundValue;
			if (dictionary.TryGetValue(key, out foundValue))
                dictionary[key] = value(foundValue);
            else
                dictionary.Add(key, value(seed));
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
			Dictionary<K, V> foundDictionary;
            if (!dictionary.TryGetValue(key1, out foundDictionary))
            {
				foundDictionary = new Dictionary<K, V> { { key2, value } };
				dictionary.Add(key1, foundDictionary);
			}
            
			if (foundDictionary.ContainsKey(key2))
				foundDictionary[key2] = value;
			else
				foundDictionary.Add(key2, value);
		}

        /// <summary>
        /// Returns the value for the specified key or a default value if it doesn't exist.
        /// </summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <typeparam name="V">Value type.</typeparam>
        /// <param name="dictionary">Current dictionary.</param>
        /// <param name="key">Key to retrieve the value for.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dictionary, K key, V defaultValue)
        {
            V value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}
