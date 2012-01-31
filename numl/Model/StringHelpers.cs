/*
 Copyright (c) 2012 Seth Juarez

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
*/

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace numl.Model
{
    public static class StringHelpers
    {
        /// <summary>
        /// Lazy list of available characters in a given string
        /// </summary>
        /// <param name="s">string</param>
        /// <param name="exclusions">characters to ignore</param>
        /// <returns>returns key value</returns>
        internal static IEnumerable<string> GetChars(string s, string[] exclusions = null)
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
                key = char.IsSymbol(a) || char.IsPunctuation(a) || char.IsSeparator(a) ? "#SYM#" : key;
                key = char.IsNumber(a) ? "#NUM#" : key;

                yield return key;
            }
        }

        /// <summary>
        /// Lazy list of available words in a string
        /// </summary>
        /// <param name="s">input string</param>
        /// <param name="separator">separator string</param>
        /// <param name="exclusions">excluded words</param>
        /// <returns>key words</returns>
        internal static IEnumerable<string> GetWords(string s, string separator, string[] exclusions = null)
        {
            s = s.Trim().ToUpperInvariant();

            foreach (string w in s.Split(separator.ToCharArray()))
            {
                string key = w.Trim();

                // kill inlined stuff that creates noise
                // (like punctuation etc.)
                key = key.Aggregate("",
                    (x, a) =>
                    {
                        if (char.IsSymbol(a) || char.IsPunctuation(a) || char.IsSeparator(a))
                            return x;
                        else
                            return x + a;
                    }
                );

                // null or whitespace
                if (string.IsNullOrWhiteSpace(key)) continue;

                // if stemming or anything of that nature is going to
                // happen, it should happen here. The exclusion dictionary
                // should also be modified to take into account the 
                // stemmed excluded terms

                // in excluded list
                if (exclusions != null && exclusions.Length > 0 && exclusions.Contains(key))
                    continue;

                // found a number! decimal pointed numbers should work since we
                // killed all of the punctuation!
                key = key.Where(c => char.IsNumber(c)).Count() == key.Length ? "#NUM#" : key;

                yield return key;
            }
        }

        internal static double[] GetWordCount(string item, StringProperty property)
        {
            double[] counts = new double[property.Dictionary.Length];
            var d = new Dictionary<string, int>();

            for (int i = 0; i < counts.Length; i++)
            {
                counts[i] = 0;
                // for quick index lookup
                d.Add(property.Dictionary[i], i);
            }

            IEnumerable<string> words = property.SplitType == StringSplitType.Character ?
                                                 GetChars(item) :
                                                 GetWords(item, property.Separator);

            // TODO: this is not too efficient. Perhaps reconsider how to do this
            foreach (var s in words)
                if (property.Dictionary.Contains(s))
                    counts[d[s]]++;

            return counts;
        }

        internal static int GetWordPosition(string item, StringProperty property)
        {
            if (property.Dictionary == null || property.Dictionary.Length == 0)
                throw new InvalidOperationException(string.Format("Cannot use StringProperty {0} with an empty property dictionary!", property.Name));

            item = item.Trim().ToUpperInvariant();
            for (int i = 0; i < property.Dictionary.Length; i++)
                if (property.Dictionary[i] == item)
                    return i;

            throw new InvalidOperationException(
                string.Format("\"{0}\" does not exist in the property dictionary for {1}", item, property.Name));
        }

        internal static Dictionary<string, double> BuildDataDictionary(IEnumerable<object> examples, StringProperty property)
        {
            Dictionary<string, double> d = new Dictionary<string, double>();

            // for holding string
            string s = string.Empty;

            foreach (object o in examples)
            {
                // get proper string
                s = (string)Convert.GetItem(o, property.Name);

                if (property.SplitType == StringSplitType.Character)
                {
                    foreach (string key in GetChars(s, property.Exclude))
                    {
                        if (d.ContainsKey(key))
                            d[key] += 1;
                        else
                            d.Add(key, 1);
                    }
                }
                else if (property.SplitType == StringSplitType.Word)
                {
                    foreach (string key in GetWords(s, property.Separator, property.Exclude))
                    {
                        if (d.ContainsKey(key))
                            d[key] += 1;
                        else
                            d.Add(key, 1);
                    }
                }
            }

            // remove words occurring only once !! NOT A GOOD
            // IDEA WHEN TRIMMING OUT THINGS...
            // var remove = d.Where(kv => kv.Value == 1).Select(kv => kv.Key).ToArray();
            // for (int i = 0; i < remove.Length; i++)
            //     d.Remove(remove[i]);


            // calculate relative term weight
            // why is this necessary?
            // perhaps statistic is interesting
            // for model purposes?
            var sum = d.Select(kv => kv.Value).Sum();
            foreach (var key in d.Select(kv => kv.Key).ToArray())
                d[key] /= sum;


            return d;
        }

        /// <summary>
        /// Populate StringProperty dictionary for given property
        /// </summary>
        /// <param name="property">StringProperty</param>
        /// <param name="examples">Examples</param>
        /// <returns>PropertyDescription with Populated Dictionary</returns>
        public static StringProperty BuildDictionary(this StringProperty property, IEnumerable<object> examples)
        {
            var d = BuildDataDictionary(examples, property);
            property.Dictionary = d.Keys.OrderBy(s => s).ToArray();
            return property;
        }

        /// <summary>
        /// Populate StringProperty dictionaries in Model Description
        /// </summary>
        /// <param name="desc">Model Description</param>
        /// <param name="examples">Examples</param>
        /// <returns>Same Description with Populated Dictionaries</returns>
        public static Description BuildDictionaries(this Description desc, IEnumerable<object> examples)
        {
            // build dictionaries for string properties
            foreach (var p in desc.Features.Where(p => p is StringProperty))
                (p as StringProperty).BuildDictionary(examples);

            // if it is a label description, build dictionaries for this as well
            if (desc is LabeledDescription && ((LabeledDescription)desc).Label is StringProperty)
                (((LabeledDescription)desc).Label as StringProperty).BuildDictionary(examples);

            return desc;
        }
    }
}
