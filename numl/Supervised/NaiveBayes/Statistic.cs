using numl.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Supervised.NaiveBayes
{
    public class Statistic
    {
        public string Label { get; set; }
        public Range X { get; set; }
        public double Probability { get; set; }
        public bool Discrete { get; set; }
        public Statistic[] Conditional { get; set; }

        public override string ToString()
        {
            return string.Format("P({0}) = {1} [{2}]", Label, Probability, Discrete ? X.Min.ToString() : X.ToString());
        }
    }
}
