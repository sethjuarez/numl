// file:	Model\StringProperty.cs
//
// summary:	Implements the string property class
using System;
using System.IO;
using numl.Utils;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace numl.Model
{
    /// <summary>Enumeration describing how to split a string property.</summary>
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

    /// <summary>Represents a string property.</summary>
    [XmlRoot("StringProperty"), Serializable]
    public class StringProperty : Property
    {
        /// <summary>Default constructor.</summary>
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
        /// <summary>How to separate words (defaults to a space)</summary>
        /// <value>The separator.</value>
        public string Separator { get; set; }
        /// <summary>How to split text.</summary>
        /// <value>The type of the split.</value>
        public StringSplitType SplitType { get; set; }
        /// <summary>generated dictionary (using bag of words model)</summary>
        /// <value>The dictionary.</value>
        public string[] Dictionary { get; set; }
        /// <summary>Exclusion set (stopword removal)</summary>
        /// <value>The exclude.</value>
        public string[] Exclude { get; set; }
        /// <summary>Treat as enumeration.</summary>
        /// <value>true if as enum, false if not.</value>
        public bool AsEnum { get; set; }
        /// <summary>Expansion length (total distinct words)</summary>
        /// <value>The length.</value>
        public override int Length
        {
            get
            {
                return AsEnum ? 1 : Dictionary.Length;
            }
        }
        /// <summary>Expansion column names.</summary>
        /// <returns>List of distinct words and positions.</returns>
        public override IEnumerable<string> GetColumns()
        {
            if (AsEnum)
                yield return Name;
            else
                foreach (var s in Dictionary)
                    yield return s;
        }
        /// <summary>Preprocess data set to create dictionary.</summary>
        /// <param name="examples">.</param>
        public override void PreProcess(IEnumerable<object> examples)
        {
            var q = from s in examples
                    select Ject.Get(s, Name).ToString();

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
        /// <summary>Convert from number to string.</summary>
        /// <param name="val">Number.</param>
        /// <returns>String.</returns>
        public override object Convert(double val)
        {
            if (AsEnum)
                return Dictionary[(int)val];
            else
                return val.ToString();
        }
        /// <summary>Convert string to list of numbers.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="o">in string.</param>
        /// <returns>lazy list of numbers.</returns>
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
                yield return (double)StringHelpers.GetWordPosition(s, Dictionary, false);
            // returns list
            else
                foreach (double val in StringHelpers.GetWordCount(s, this))
                    yield return val;
        }
        /// <summary>import exclusion list from file.</summary>
        /// <param name="file">.</param>
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
        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);
            Separator = reader.GetAttribute("Separator");
            SplitType = (StringSplitType)Enum.Parse(typeof(StringSplitType), reader.GetAttribute("SplitType"));
            AsEnum = bool.Parse(reader.GetAttribute("AsEnum"));

            reader.ReadStartElement();

            Dictionary = new string[int.Parse(reader.GetAttribute("Length"))];
            reader.ReadStartElement("Dictionary");
            for (int i = 0; i < Dictionary.Length; i++)
                Dictionary[i] = reader.ReadElementString("item");
            reader.ReadEndElement();

            Exclude = new string[int.Parse(reader.GetAttribute("Length"))];
            reader.ReadStartElement("Exclude");
            for (int i = 0; i < Exclude.Length; i++)
                Exclude[i] = reader.ReadElementString("item");
        }
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteAttributeString("Separator", Separator);
            writer.WriteAttributeString("SplitType", SplitType.ToString());
            writer.WriteAttributeString("AsEnum", AsEnum.ToString());

            writer.WriteStartElement("Dictionary");
            writer.WriteAttributeString("Length", Dictionary.Length.ToString());
            for (int i = 0; i < Dictionary.Length; i++)
            {
                writer.WriteStartElement("item");
                writer.WriteValue(Dictionary[i]);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Exclude");
            writer.WriteAttributeString("Length", Exclude.Length.ToString());
            for (int i = 0; i < Exclude.Length; i++)
            {
                writer.WriteStartElement("item");
                writer.WriteValue(Exclude[i]);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
