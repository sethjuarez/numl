// file:	Supervised\NaiveBayes\Statistic.cs
//
// summary:	Implements the statistic class
using numl.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Serialization;

namespace numl.Supervised.NaiveBayes
{
    /// <summary>A statistic.</summary>
    [XmlRoot("Statistic")]
    public class Statistic
    {
        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [XmlAttribute("Label")]
        public string Label { get; set; }
        /// <summary>Gets or sets a value indicating whether the discrete.</summary>
        /// <value>true if discrete, false if not.</value>
        [XmlAttribute("Discrete")]
        public bool Discrete { get; set; }
        /// <summary>Gets or sets the number of. </summary>
        /// <value>The count.</value>
        [XmlAttribute("Count")]
        public int Count { get; set; }
        /// <summary>Gets or sets the x coordinate.</summary>
        /// <value>The x coordinate.</value>
        [XmlElement("Range")]
        public Range X { get; set; }
        /// <summary>Gets or sets the probability.</summary>
        /// <value>The probability.</value>
        [XmlAttribute("Probability")]
        public double Probability { get; set; }
        /// <summary>Gets or sets the conditionals.</summary>
        /// <value>The conditionals.</value>
        [XmlArray("Conditionals")]
        public Measure[] Conditionals { get; set; }
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("P({0}) = {1} [{2}, {3}]", Label, Probability, Count, Discrete ? X.Min.ToString() : X.ToString());
        }
        /// <summary>Makes a deep copy of this object.</summary>
        /// <returns>A copy of this object.</returns>
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
        /// <summary>Makes.</summary>
        /// <param name="label">The label.</param>
        /// <param name="x">The Range to process.</param>
        /// <param name="count">(Optional) number of.</param>
        /// <returns>A Statistic.</returns>
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
        /// <summary>Makes.</summary>
        /// <param name="label">The label.</param>
        /// <param name="val">The value.</param>
        /// <param name="count">(Optional) number of.</param>
        /// <returns>A Statistic.</returns>
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
