using System;
using System.IO;
using numl.Utils;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace numl.Model
{
    public enum StringSplitType
    {
        /// <summary>
        /// Split string into corresponding characters
        /// </summary>
        Character,
        /// <summary>
        /// Split string into corresponding words
        /// </summary>
        Word
    }

    public class StringProperty : Property
    {
        public StringProperty()
            : base()
        {
            // set to default conventions
            SplitType = StringSplitType.Word;
            Separator = " ";
            Dictionary = new string[] { };
            Exclude = new string[] { };
            AsEnum = false;
            Type = typeof(string);
            Discrete = true;
        }

        public string Separator { get; set; }
        public StringSplitType SplitType { get; set; }
        public string[] Dictionary { get; set; }
        public string[] Exclude { get; set; }
        public bool AsEnum { get; set; }

        public override int Length
        {
            get
            {
                return AsEnum ? 1 : Dictionary.Length;
            }
        }

        public override IEnumerable<string> GetColumns()
        {
            if (AsEnum)
                yield return Name;
            else
                foreach (var s in Dictionary)
                    yield return s;
        }

        public override void PreProcess(IEnumerable<object> examples)
        {
            var q = from s in examples
                    select FastReflection.Get(s, Name).ToString();

            if (AsEnum)
                Dictionary = StringHelpers.BuildEnumDictionary(q).Select(kv => kv.Key).ToArray();
            else
            {
                switch (SplitType)
                {
                    case StringSplitType.Character:
                        Dictionary = StringHelpers.BuildCharDictionary(q, Exclude).Select(kv => kv.Key).ToArray();
                        break;
                    case StringSplitType.Word:
                        Dictionary = StringHelpers.BuildWordDictionary(q, Separator, Exclude).Select(kv => kv.Key).ToArray();
                        break;
                }
            }
        }

        public override object Convert(double val)
        {
            if (AsEnum)
                return Dictionary[(int)val];
            else
                return val.ToString();
        }

        public override IEnumerable<double> Convert(object o)
        {
            // check for valid dictionary
            if (Dictionary == null || Dictionary.Length == 0)
                throw new InvalidOperationException(string.Format("{0} dictionaries do not exist.", Name));

            // sanitize string
            string s = "";
            if (o == null || string.IsNullOrEmpty(o.ToString()) || string.IsNullOrWhiteSpace(o.ToString()))
                s = StringHelpers.EMPTY_STRING;
            else
                s = o.ToString();

            // returns single number
            if (AsEnum)
                yield return (double)StringHelpers.GetWordPosition(s, Dictionary);
            // returns list
            else
                foreach (double val in StringHelpers.GetWordCount(s, this))
                    yield return val;
        }

        public void ImportExclusions(string file)
        {
            // add exclusions
            if (!string.IsNullOrEmpty(file) && !string.IsNullOrWhiteSpace(file) && File.Exists(file))
            {
                Regex regex;
                if (SplitType == StringSplitType.Word)
                    regex = new Regex(@"\w+", RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                else
                    regex = new Regex(@"\w", RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase);

                List<string> exclusionList = new List<string>();
                using (StreamReader sr = new StreamReader(file))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var match = regex.Match(line);
                        // found something not already in list...
                        if (match.Success && !exclusionList.Contains(match.Value.Trim().ToUpperInvariant()))
                            exclusionList.Add(match.Value.Trim().ToUpperInvariant());
                    }
                }

                Exclude = exclusionList.OrderBy(s => s).ToArray();
            }
            else
                Exclude = new string[] { };
        }
    }
}
