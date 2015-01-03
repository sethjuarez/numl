// file:	Supervised\NaiveBayes\NaiveBayesGenerator.cs
//
// summary:	Implements the naive bayes generator class
using System;
using numl.Model;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.NaiveBayes
{
    /// <summary>A naive bayes generator.</summary>
    public class NaiveBayesGenerator : Generator
    {
        /// <summary>Gets or sets the width.</summary>
        /// <value>The width.</value>
        public int Width { get; set; }
        /// <summary>Constructor.</summary>
        /// <param name="width">The width.</param>
        public NaiveBayesGenerator(int width)
        {
            Width = width;
        }
        /// <summary>Generate model based on a set of examples.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>Model.</returns>
        public override IModel Generate(Matrix x, Vector y)
        {
            if (Descriptor == null)
                throw new InvalidOperationException("Cannot build naive bayes model without type knowledge!");

            // create answer probabilities
            if (!Descriptor.Label.Discrete)
                throw new InvalidOperationException("Need to use regression for non-discrete labels!");

            // compute Y probabilities
            Statistic[] statistics = GetLabelStats(y);

            Measure root = new Measure
            {
                Discrete = true,
                Label = Descriptor.Label.Name,
                Probabilities = statistics
            };

            // collect feature ranges
            Measure[] features = GetBaseConditionals(x);

            // compute conditional counts
            for (int i = 0; i < y.Length; i++)
            {
                var stat = statistics.Where(s => s.X.Min == y[i]).First();
                if (stat.Conditionals == null)
                    stat.Conditionals = CloneMeasure(features);

                for (int j = 0; j < x.Cols; j++)
                {
                    var s = stat.Conditionals[j];
                    s.Increment(x[i, j]);
                }
            }

            // normalize into probabilities
            for (int i = 0; i < statistics.Length; i++)
            {
                var cond = statistics[i];
                for (int j = 0; j < cond.Conditionals.Length; j++)
                    cond.Conditionals[j].Normalize();
            }


            return new NaiveBayesModel { Descriptor = Descriptor, Root = root };
        }
        /// <summary>Gets label statistics.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An array of statistic.</returns>
        private Statistic[] GetLabelStats(Vector y)
        {
            var stats = y.Stats();
            Statistic[] statistics = new Statistic[stats.Rows];
            for (int i = 0; i < statistics.Length; i++)
            {
                double yVal = stats[i, 0];
                var s = Statistic.Make(Descriptor.Label.Convert(stats[i, 0]).ToString(), yVal);
                s.Count = (int)stats[i, 1];
                s.Probability = stats[i, 2];
                statistics[i] = s;
            }
            return statistics;
        }
        /// <summary>Gets base conditionals.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <returns>An array of measure.</returns>
        private Measure[] GetBaseConditionals(Matrix x)
        {
            Measure[] features = new Measure[x.Cols];
            for (int i = 0; i < features.Length; i++)
            {
                Property p = Descriptor.At(i);
                var f = new Measure
                {
                    Discrete = p.Discrete,
                    Label = Descriptor.ColumnAt(i),
                };

                IEnumerable<Statistic> fstats;
                if (f.Discrete)
                    fstats = x[i, VectorType.Col].Distinct().OrderBy(d => d)
                                                 .Select(d => Statistic.Make(p.Convert(d).ToString(), d, 1));
                else
                    fstats = x[i, VectorType.Col].Segment(Width)
                                                 .Select(d => Statistic.Make(f.Label, d, 1));

                f.Probabilities = fstats.ToArray();
                features[i] = f;
            }

            return features;
        }
        /// <summary>Clone measure.</summary>
        /// <param name="measures">The measures.</param>
        /// <returns>A Measure[].</returns>
        private Measure[] CloneMeasure(Measure[] measures)
        {
            var m = new Measure[measures.Length];
            for (int i = 0; i < m.Length; i++)
                m[i] = measures[i].Clone();
            return m;
        }
    }
}
