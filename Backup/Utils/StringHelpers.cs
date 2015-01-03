// file:	Utils\StringHelpers.cs
//
// summary:	Implements the string helpers class
using System;
using System.Linq;
using System.Collections.Generic;
using numl.Model;

namespace numl.Utils
{
    /// <summary>A string helpers.</summary>
    public static class StringHelpers
    {
        /// <summary>The empty string.</summary>
        public const string EMPTY_STRING = "#EMPTY#";
        /// <summary>Number of strings.</summary>
        public const string NUMBER_STRING = "#NUM#";
        /// <summary>The symbol string.</summary>
        public const string SYMBOL_STRING = "#SYM#";
        /// <summary>A string extension method that sanitizes.</summary>
        /// <param name="s">string.</param>
        /// <param name="checkNumber">(Optional) true to check number.</param>
        /// <returns>A string.</returns>
        public static string Sanitize(this string s, bool checkNumber = true)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                return EMPTY_STRING;

            s = s.Trim().ToUpperInvariant();
            string item = s.Trim();

            // kill inlined stuff that creates noise
            // (like punctuation etc.)
            item = item.Aggregate("",
                (x, a) =>
                {
                    if (char.IsSymbol(a) || char.IsPunctuation(a) || char.IsSeparator(a))
                        return x;
                    else
                        return x + a;
                }
            );

            // since we killed everything
            // and it is still empty, it
            // must be a symbol
            if (string.IsNullOrEmpty(item))
                return SYMBOL_STRING;

            // number check
            if (checkNumber)
            {
                double check = 0;
                if (double.TryParse(item, out check)) return NUMBER_STRING;
            }

            // return item
            return item;
        }
        /// <summary>Lazy list of available characters in a given string.</summary>
        /// <param name="s">string.</param>
        /// <param name="exclusions">(Optional) characters to ignore.</param>
        /// <returns>returns key value.</returns>
        public static IEnumerable<string> GetChars(string s, string[] exclusions = null)
        {
            s = s.Trim().ToUpperInvariant();

            foreach (char a in s.ToCharArray())
            {
                string key = a.ToString();

                // ignore whitespace (should maybe set as option? I think it's noise)
                if (string.IsNullOrWhiteSpace(key)) continue;

                // ignore excluded items
                if (exclusions != null && exclusions.Length > 0 && exclusions.Contains(key))
                    continue;

                // make numbers and symbols a single feature
                // I think it is noise....
                key = char.IsSymbol(a) || char.IsPunctuation(a) || char.IsSeparator(a) ? SYMBOL_STRING : key;
                key = char.IsNumber(a) ? NUMBER_STRING : key;

                yield return key;
            }
        }
        /// <summary>Lazy list of available words in a string.</summary>
        /// <param name="s">input string.</param>
        /// <param name="separator">(Optional) separator string.</param>
        /// <param name="exclusions">(Optional) excluded words.</param>
        /// <returns>key words.</returns>
        public static IEnumerable<string> GetWords(string s, string separator = " ", string[] exclusions = null)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                yield return EMPTY_STRING;
            else
            {
                s = s.Trim().ToUpperInvariant();

                foreach (string w in s.Split(separator.ToCharArray()))
                {
                    string key = Sanitize(w);

                    // if stemming or anything of that nature is going to
                    // happen, it should happen here. The exclusion dictionary
                    // should also be modified to take into account the 
                    // stemmed excluded terms

                    // in excluded list
                    if (exclusions != null && exclusions.Length > 0 && exclusions.Contains(key))
                        continue;

                    yield return key;
                }
            }
        }
        /// <summary>Gets word count.</summary>
        /// <param name="item">The item.</param>
        /// <param name="property">The property.</param>
        /// <returns>An array of double.</returns>
        public static double[] GetWordCount(string item, StringProperty property)
        {
            double[] counts = new double[property.Dictionary.Length];
            var d = new Dictionary<string, int>();

            for (int i = 0; i < counts.Length; i++)
            {
                counts[i] = 0;
                // for quick index lookup
                d.Add(property.Dictionary[i], i);
            }

            // get list of words (or chars) from source
            IEnumerable<string> words = property.SplitType == StringSplitType.Character ?
                                                 GetChars(item) :
                                                 GetWords(item, property.Separator);

            // TODO: this is not too efficient. Perhaps reconsider how to do this
            foreach (var s in words)
                if (property.Dictionary.Contains(s))
                    counts[d[s]]++;

            return counts;
        }
        /// <summary>Gets word position.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="item">The item.</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="checkNumber">(Optional) true to check number.</param>
        /// <returns>The word position.</returns>
        public static int GetWordPosition(string item, string[] dictionary, bool checkNumber = true)
        {
            //string[] dictionary = property.Dictionary;
            if (dictionary == null || dictionary.Length == 0)
                throw new InvalidOperationException("Cannot get word position with an empty dictionary");

            item = Sanitize(item, checkNumber);

            // is this the smartest thing?
            for (int i = 0; i < dictionary.Length; i++)
                if (dictionary[i] == item)
                    return i;

            throw new InvalidOperationException(
                string.Format("\"{0}\" does not exist in the property dictionary", item));
        }
        /// <summary>Builds character dictionary.</summary>
        /// <param name="examples">The examples.</param>
        /// <param name="exclusion">(Optional) the exclusion.</param>
        /// <returns>A Dictionary&lt;string,double&gt;</returns>
        public static Dictionary<string, double> BuildCharDictionary(IEnumerable<string> examples, string[] exclusion = null)
        {
            Dictionary<string, double> d = new Dictionary<string, double>();

            foreach (string o in examples)
            {
                foreach (string key in GetChars(o, exclusion))
                {
                    if (d.ContainsKey(key))
                        d[key] += 1;
                    else
                        d.Add(key, 1);
                }
            }

            return d;
        }
        /// <summary>Builds enum dictionary.</summary>
        /// <param name="examples">The examples.</param>
        /// <returns>A Dictionary&lt;string,double&gt;</returns>
        public static Dictionary<string, double> BuildEnumDictionary(IEnumerable<string> examples)
        {
            // TODO: Really need to consider this as an enum builder
            Dictionary<string, double> d = new Dictionary<string, double>();

            // for holding string
            string s = string.Empty;

            foreach (string o in examples)
            {
                s = o.Trim().ToUpperInvariant();

                // kill inlined stuff that creates noise
                // (like punctuation etc.)
                s = s.Aggregate("",
                    (x, a) =>
                    {
                        if (char.IsSymbol(a) || char.IsPunctuation(a) || char.IsSeparator(a))
                            return x;
                        else
                            return x + a;
                    }
                );

                // null or whitespace
                if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                    s = EMPTY_STRING;

                if (d.ContainsKey(s))
                    d[s] += 1;
                else
                    d.Add(s, 1);
            }

            return d;
        }
        /// <summary>Builds word dictionary.</summary>
        /// <param name="examples">The examples.</param>
        /// <param name="separator">(Optional) separator string.</param>
        /// <param name="exclusion">(Optional) the exclusion.</param>
        /// <returns>A Dictionary&lt;string,double&gt;</returns>
        public static Dictionary<string, double> BuildWordDictionary(IEnumerable<string> examples, string separator = " ", string[] exclusion = null)
        {
            Dictionary<string, double> d = new Dictionary<string, double>();

            foreach (string s in examples)
                foreach (string key in GetWords(s, separator, exclusion))
                    if (d.ContainsKey(key))
                        d[key] += 1;
                    else
                        d.Add(key, 1);

            return d;
        }
    }
}
