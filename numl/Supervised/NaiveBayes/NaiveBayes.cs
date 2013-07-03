using System;
using numl.Utils;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Model;
using numl.Math;
using System.Drawing;

namespace numl.Supervised.NaiveBayes
{
    public class Pair
    {
        public Range X { get; set; }
        public double Y { get; set; }
    }

    public class FeatureStatistic
    {
        public bool Discrete { get; set; }

        public FeatureStatistic(Vector x, Vector y, int width = 0)
        {
            Discrete = width <= 0;

            if (width <= 0)
                Initialize(x.Select(d => Range.Make(d, d)), y.Distinct());
            else
                Initialize(x.Segment(width), y.Distinct());

            Count(x, y);

        }


        private void Initialize(IEnumerable<Range> x, IEnumerable<double> y)
        {
            _conditional = new Dictionary<double, Dictionary<Range, double>>();
            _statistic = new Dictionary<double, double>();
            foreach (double item in y)
            {
                _conditional[item] = new Dictionary<Range, double>();
                _statistic[item] = 0;
                foreach (Range range in x.OrderBy(d => d.Min))
                {
                    _conditional[item][range] = 1;
                    _statistic[item]++;    
                }
            }
        }

        private void Count(Vector x, Vector y)
        {
            if (x.Length != y.Length)
                throw new InvalidOperationException();

            for (int i = 0; i < x.Length; i++)
            {
                this[x[i], y[i]]++;
                _statistic[y[i]]++;
            }
        }


        private Dictionary<double, Dictionary<Range, double>> _conditional;
        private Dictionary<double, double> _statistic;


        public double this[double x, double y]
        {
            //TODO: FINISH HERE!!
            get
            {
                var cond = _conditional[y];
                foreach (var i in cond)
                    if (Discrete && i.Key.Min == x)
                        return i.Value;
                    else if (i.Key.Test(x))
                        return i.Value;

                throw new IndexOutOfRangeException();
                
            }
            set
            {
                var cond = _conditional[y];
                
            }
        }

    }

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


            // create conditional probabilities
            for (int i = 0; i < x.Cols; i++)
            {
                var p = Descriptor.At(i);
                // setup (all have a count of 1 to start for smoothing)
                FeatureStatistic cp = new FeatureStatistic(
                                                    x[i, VectorType.Col], 
                                                    y, 
                                                    p.Discrete ? 0 : Width);


            }



            //var lablCounts = new Dictionary<double, double>();
            //var condCounts = new Dictionary<Tuple<double, double>, double>();
            //// generate counts
            //for (int i = 0; i < x.Rows; i++)
            //{
            //    lablCounts.AddOrUpdate(y[i], v => v + 1, 0);
            //    for (int j = 0; j < x.Cols; j++)
            //        condCounts.AddOrUpdate(Tuple.Create(y[i], x[i, j]), v => v + 1, 0);
            //}

            return new NaiveBayesModel();
        }
    }

    public class NaiveBayesModel : Model
    {
        public override double Predict(Vector y)
        {

            return 0;
        }
    }
}
