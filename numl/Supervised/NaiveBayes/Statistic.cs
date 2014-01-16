using numl.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Serialization;

namespace numl.Supervised.NaiveBayes
{
    [XmlRoot("Statistic")]
    public class Statistic
    {
        [XmlAttribute("Label")]
        public string Label { get; set; }

        [XmlAttribute("Discrete")]
        public bool Discrete { get; set; }

        [XmlAttribute("Count")]
        public int Count { get; set; }

        [XmlElement("Range")]
        public Range X { get; set; }

        [XmlAttribute("Probability")]
        public double Probability { get; set; }

        [XmlArray("Conditionals")]
        public Measure[] Conditionals { get; set; }

        public override string ToString()
        {
            return string.Format("P({0}) = {1} [{2}, {3}]", Label, Probability, Count, Discrete ? X.Min.ToString() : X.ToString());
        }

        public Statistic Clone()
        {
            var s = new Statistic
            {
                Label = Label,
                Discrete = Discrete,
                Count = Count,
                X = X,
                Probability = Probability
            };

            if (Conditionals != null && Conditionals.Length > 0)
            {
                s.Conditionals = new Measure[Conditionals.Length];
                for (int i = 0; i < s.Conditionals.Length; i++)
                    s.Conditionals[i] = Conditionals[i].Clone();
            }


            return s;
        }

        public static Statistic Make(string label, Range x, int count = 0)
        {
            return new Statistic
            {
                Label = label,
                Discrete = false,
                Count = count,
                X = x
            };
        }

        public static Statistic Make(string label, double val, int count = 0)
        {
            return new Statistic
            {
                Label = label,
                Discrete = true,
                Count = count,
                X = Range.Make(val)
            };
        }
    }
}
