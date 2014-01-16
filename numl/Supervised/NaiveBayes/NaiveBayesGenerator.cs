using System;
using numl.Model;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.NaiveBayes
{
    public class NaiveBayesGenerator : Generator
    {
        public int Width { get; set; }

        public NaiveBayesGenerator(int width)
        {
            Width = width;
        }

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

        private Measure[] CloneMeasure(Measure[] measures)
        {
            var m = new Measure[measures.Length];
            for (int i = 0; i < m.Length; i++)
                m[i] = measures[i].Clone();
            return m;
        }
    }
}
