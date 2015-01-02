// file:	Supervised\NaiveBayes\Measure.cs
//
// summary:	Implements the measure class
using System;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace numl.Supervised.NaiveBayes
{
    /// <summary>A measure.</summary>
    [XmlRoot("Measure")]
    public class Measure
    {
        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [XmlAttribute("Label")]
        public string Label { get; set; }
        /// <summary>Gets or sets a value indicating whether the discrete.</summary>
        /// <value>true if discrete, false if not.</value>
        [XmlAttribute("Discrete")]
        public bool Discrete { get; set; }
        /// <summary>Gets or sets the probabilities.</summary>
        /// <value>The probabilities.</value>
        [XmlArray("Probabilities")]
        public Statistic[] Probabilities { get; set; }
        /// <summary>Increments.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="x">The x coordinate.</param>
        internal void Increment(double x)
        {
            var p = GetStatisticFor(x);
            if (p == null) throw new InvalidOperationException("Range not found!");
            p.Count++;
        }
        /// <summary>Gets a probability.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="x">The x coordinate.</param>
        /// <returns>The probability.</returns>
        internal double GetProbability(double x)
        {
            var p = GetStatisticFor(x);
            if (p == null) throw new InvalidOperationException("Range not found!");
            return p.Probability;
        }
        /// <summary>Gets statistic for.</summary>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is outside the required
        /// range.</exception>
        /// <param name="x">The x coordinate.</param>
        /// <returns>The statistic for.</returns>
        internal Statistic GetStatisticFor(double x)
        {
            if (Probabilities == null || Probabilities.Length == 0)
                throw new IndexOutOfRangeException("Invalid statistics");

            var p = Probabilities.Where(s => s.X.Test(x)).FirstOrDefault();

            return p;
        }

        /// <summary>Normalizes this object.</summary>
        internal void Normalize()
        {
            double total = Probabilities.Select(p => p.Count).Sum();
            for (int i = 0; i < Probabilities.Length; i++)
                Probabilities[i].Probability = (double)Probabilities[i].Count / total;
        }
        /// <summary>Makes a deep copy of this object.</summary>
        /// <returns>A copy of this object.</returns>
        public Measure Clone()
        {
            var m = new Measure
            {
                Label = Label,
                Discrete = Discrete
            };

            if (Probabilities != null && Probabilities.Length > 0)
            {
                m.Probabilities = new Statistic[Probabilities.Length];
                for (int i = 0; i < m.Probabilities.Length; i++)
                    m.Probabilities[i] = Probabilities[i].Clone();
            }

            return m;
        }
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0} [{1}]", Label, Discrete ? "Discrete" : "Continuous");
        }
        /// <summary>Tests if this object is considered equal to another.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the objects are considered equal, false if they are not.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Measure)) return false;
            var measure = obj as Measure;
            if (Label != measure.Label) return false;
            if (Discrete != measure.Discrete) return false;

            if (Probabilities == null && measure.Probabilities != null) return false;
            if (measure.Probabilities == null && Probabilities != null) return false;

            if (Probabilities != null)
            {
                if (Probabilities.Length != measure.Probabilities.Length) return false;
                for (int i = 0; i < Probabilities.Length; i++)
                    if (!Probabilities[i].Equals(measure.Probabilities[i]))
                        return false;
            }
            return true;
        }
        /// <summary>Calculates a hash code for this object.</summary>
        /// <returns>A hash code for this object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
