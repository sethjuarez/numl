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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

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

    [XmlRoot("sp"), Serializable]
    public class StringProperty : Property
    {
        public StringProperty()
        {
            // set to default conventions
            SplitType = StringSplitType.Word;
            Separator = " ";
            Dictionary = new string[] { };
            Exclude = new string[] { };
            AsEnum = false;
            Type = typeof(string);
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
                if (AsEnum)
                    return 1;
                else
                    return Dictionary.Length;
            }
        }

        public override double[] ToArray(object o)
        {
            if (Dictionary == null || Dictionary.Length == 0)
                throw new InvalidOperationException(string.Format("{0} dictionaries do not exist.", Name));

            if (AsEnum)
                return new[] { Convert(o) };
            else
                return StringHelpers.GetWordCount(o.ToString(), this);
        }

        public override double Convert(object o)
        {
            if (Dictionary == null || Dictionary.Length == 0)
                throw new InvalidOperationException(string.Format("{0} dictionaries do not exist.", Name));

            if (AsEnum)
                return (double)StringHelpers.GetWordPosition(o.ToString(), this);
            else
                throw new InvalidCastException(string.Format("Cannot compress {0} to a single double", Name));
        }

        public override object Convert(double val)
        {
            if (AsEnum)
                return Dictionary[(int)val];
            else
                throw new InvalidCastException(string.Format("Cannot exand {0} to a string", val));
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

        // Serialization
        public override void WriteXml(XmlWriter writer)
        {
            // write inital bits
            base.WriteXml(writer);
            writer.WriteAttributeString("enum", AsEnum.ToString());
            writer.WriteAttributeString("split", SplitType.ToString());
            // write dictionaries
            writer.WriteStartElement("d");
            writer.WriteAttributeString("length", Dictionary.Length.ToString());
            for(int i = 0; i < Dictionary.Length; i++)
            {
                writer.WriteStartElement("i");
                writer.WriteValue(Dictionary[i]);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("e");
            writer.WriteAttributeString("length", Exclude.Length.ToString());
            for (int i = 0; i < Exclude.Length; i++)
            {
                writer.WriteStartElement("i");
                writer.WriteValue(Exclude[i]);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public override void ReadXml(XmlReader reader)
        {
            // read initial bits
            reader.MoveToContent();

            Name = reader.GetAttribute("name");
            string type = reader.GetAttribute("type");
            Type = R.FindType(type);
            Start = int.Parse(reader.GetAttribute("start"));

            AsEnum = bool.Parse(reader.GetAttribute("enum"));
            SplitType = (StringSplitType)Enum.Parse(typeof(StringSplitType), reader.GetAttribute("split"));
            reader.ReadStartElement();
            // read dictionaries
            int dlength = int.Parse(reader.GetAttribute("length"));
            reader.ReadStartElement("d");
            Dictionary = new string[dlength];
            for (int i = 0; i < Dictionary.Length; i++)
                Dictionary[i] = reader.ReadElementString("i");
            reader.ReadEndElement();

            int elength = int.Parse(reader.GetAttribute("length"));
            reader.ReadStartElement("e");
            Exclude = new string[elength];
            for (int i = 0; i < Exclude.Length; i++)
                Exclude[i] = reader.ReadElementString("i");
            reader.ReadEndElement();

            reader.ReadEndElement();
        }
    }
}
