using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Utils
{
    public static class DictionaryHelpers
    {
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

        public static void AddOrUpdate<K, V>(this Dictionary<K, V> dictionary, K key, Func<V, V> value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value(dictionary[key]);
            else
                dictionary[key] = value(default(V));
        }

        public static void AddOrUpdate<K, V>(this Dictionary<K, V> dictionary, K key, Func<V, V> value, V seed)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value(dictionary[key]);
            else
                dictionary[key] = value(seed);
        }
    }
}
