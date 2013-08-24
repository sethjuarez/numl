using System;
using numl.Utils;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Model;
using numl.Math;
using System.Drawing;
using System.Collections;

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


            var stats = y.Stats();
            Statistic[] statistics = new Statistic[stats.Rows];
            for (int i = 0; i < statistics.Length; i++)
            {
                double yVal = stats[i, 0];
                var s = new Statistic
                {
                    Discrete = true,
                    X = new Range { Min = yVal, Max = yVal },
                    Label = Descriptor.Label.Convert(stats[i, 0]).ToString(),
                    Probability = stats[i, 2]
                };


                var conditionals = x.Slice(y.Indices(d => d == yVal), VectorType.Row)
                                    .Stats(VectorType.Row);
                

                statistics[i] = s;
            }




            // create conditional probabilities
            //FeatureStatistic[] statistics = new FeatureStatistic[x.Cols];
            //for (int i = 0; i < x.Cols; i++)
            //{
            //    var p = Descriptor.At(i);
            //    // setup (all have a count of 1 to start for smoothing)
            //    FeatureStatistic cp = new FeatureStatistic(
            //                                        x[i, VectorType.Col],
            //                                        y,
            //                                        p.Discrete ? 0 : Width);
            //    statistics[i] = cp;
            //}
            return new NaiveBayesModel { Descriptor = Descriptor };
        }
    }

    public class NaiveBayesModel : Model
    {
        public FeatureStatistic[] Probabilities { get; set; }

        public override double Predict(Vector y)
        {
            Vector probs = Vector.Zeros(Probabilities.Length);
            for (int i = 0; i < probs.Length; i++)
            {

            }
            

            return 0;
        }
    }
}
