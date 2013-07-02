using System;
using numl.Utils;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Model;
using numl.Math;

namespace numl.Supervised.NaiveBayes
{
    public class Pair
    {
        public bool Discrete { get; set; }
        public Range Range { get; set; }
        public double Value { get; set; }
    }

    public class ConditionalProbability
    {
        public ConditionalProbability(IEnumerable<Range> x, IEnumerable<double> y, bool discrete)
        {
            _counts = new Dictionary<Pair, int>(x.Count() * y.Count());
            foreach (Range range in x)
            {
                foreach (double item in y)
                {
                    
                }
            }
        }


        private Dictionary<Pair, int> _counts;


        public double this[double x, double y]
        {
            get
            {
                return 0;
            }
            set
            {

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

            var labels = new Dictionary<double, double>();
            for (int i = 0; i < y.Length; i++)
            {
                if (!labels.ContainsKey(y[i]))
                    labels[i] = 1;
                else labels[i]++;
            }

            // create conditional probabilities
            for (int i = 0; i < x.Cols; i++)
            {
                var p = Descriptor.At(i);
                // setup (all have a count of 1 to start for smoothing)


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
