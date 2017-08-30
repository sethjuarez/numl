// file:	Supervised\NaiveBayes\NaiveBayesModel.cs
//
// summary:	Implements the naive bayes model class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Data;

namespace numl.Supervised.NaiveBayes
{
    /// <summary>A data Model for the naive bayes.</summary>
    public class NaiveBayesModel : Model
    {
        /// <summary>Gets or sets the root.</summary>
        /// <value>The root.</value>
        public Measure Root { get; set; }

        /// <summary>Predicts the given o.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
        public override double Predict(Vector y)
        {
            this.Preprocess(y);

            if (Root == null || Descriptor == null)
                throw new InvalidOperationException("Invalid Model - Missing information");

            Vector lp = Vector.Zeros(Root.Probabilities.Length);
            for (int i = 0; i < Root.Probabilities.Length; i++)
            {
                Statistic stat = Root.Probabilities[i];
                lp[i] = System.Math.Log(stat.Probability);
                for (int j = 0; j < y.Length; j++)
                {
                    Measure conditional = stat.Conditionals[j];
                    var p = conditional.GetStatisticFor(y[j]);
                    // check for missing range, assign bad probability
                    lp[i] += System.Math.Log(p == null ? 10e-10 : p.Probability);
                }
            }
            var idx = lp.MaxIndex();
            return Root.Probabilities[idx].X.Min;
        }
    }
}
