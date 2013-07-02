using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Utils;
using numl.Math.LinearAlgebra;
using numl.Model;

namespace numl.Supervised.NaiveBayes
{
    public class NaiveBayesGenerator : Generator
    {
        public int Width { get; set; }

        public override IModel Generate(Matrix x, Vector y)
        {
            if (Descriptor == null)
                throw new InvalidOperationException("Cannot build naive bayes model without type knowledge!");

            var lablCounts = new Dictionary<double, double>();
            var condCounts = new Dictionary<Tuple<double, double>, double>();
            // generate counts
            for (int i = 0; i < x.Rows; i++)
            {
                lablCounts.AddOrUpdate(y[i], v => v + 1, 0);
                for (int j = 0; j < x.Cols; j++)
                    condCounts.AddOrUpdate(Tuple.Create(y[i], x[i, j]), v => v + 1, 0);
            }
            

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
