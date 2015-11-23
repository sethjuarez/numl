using numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Scoring
{
    /// <summary>
    /// Contains scoring statistics for a given model.
    /// </summary>
    public class Score
    {
        private double _TotalAccuracy = double.NaN;

        /// <summary>
        /// Gets or sets the total test accuracy as defined by the normalized distribution over true vs negative cases.
        /// </summary>
        public double Accuracy
        {
            get
            {
                if (double.IsNaN(this._TotalAccuracy))
                {
                    this._TotalAccuracy = 
                        ((double)this.TruePositives + (double)this.TrueNegatives) / 
                        ((double)this.TruePositives + (double)this.TrueNegatives + (double)this.FalsePositives + (double)this.FalseNegatives);
                }
                return this._TotalAccuracy;
            }
            set
            {
                this._TotalAccuracy = value;
            }
        }

        /// <summary>
        /// Gets or sets the total number of positive examples.
        /// </summary>
        public int TotalPositives { get; set; }

        /// <summary>
        /// Gets or sets the number of correctly scored positive examples.
        /// </summary>
        public int TruePositives { get; set; }

        /// <summary>
        /// Gets or sets the number of incorrectly scored positive examples.
        /// </summary>
        public int FalsePositives { get; set; }

        /// <summary>
        /// Gets or sets the total number of negative examples.
        /// </summary>
        public int TotalNegatives { get; set; }

        /// <summary>
        /// Gets or sets the number of correctly scored negative examples.
        /// </summary>
        public int TrueNegatives { get; set; }

        /// <summary>
        /// Gets or sets the number of incorrectly scored negative examples.
        /// </summary>
        public int FalseNegatives { get; set; }

        /// <summary>
        /// Gets the Precision of the model.
        /// <para>A higher precision indicates the model has a higher prediction confidence.  Also known as the positive predictive value (PPV).</para>
        /// </summary>
        public double Precision
        {
            get
            {
                return (double)this.TruePositives / ((double)this.TruePositives + (double)this.FalsePositives);
            }
        }

        /// <summary>
        /// Gets the Recall of the model.
        /// <para>A higher recall indicates the model has scored better on reducing false negative predictions.  Also known as the sensitivity.</para>
        /// </summary>
        public double Recall
        {
            get
            {
                return (double)this.TruePositives / ((double)this.TruePositives + (double)this.FalseNegatives);
            }
        }

        /// <summary>
        /// Returns the F Score of the model.
        /// <para>The F Score determines the tradeoff between higher prediction confidence and reducing false negative predictions. (Higher is better).</para>
        /// </summary>
        public double FScore
        {
            get
            {
                return 2.0 * ((this.Precision * this.Recall) / (this.Precision + this.Recall));
            }
        }

        /// <summary>
        /// Initializes a new Score object.
        /// </summary>
        public Score()
        {
            this.FalseNegatives = this.FalsePositives = this.TotalNegatives = 
                this.TotalPositives = this.TruePositives = this.TrueNegatives = 0;
        }

        /// <summary>
        /// Scores a set of predictions against the actual values.
        /// </summary>
        /// <param name="predictions">Predicted values.</param>
        /// <param name="actual">Actual values.</param>
        /// <returns></returns>
        public static Score ScorePredictions(Vector predictions, Vector actual)
        {
            // TODO: This may not be computing correctly.
            var score = new numl.Scoring.Score()
            {
                TotalPositives = actual.Where(w => w == 1d).Count(),
                TotalNegatives = actual.Where(w => (w == 0d || w == -1d)).Count(),
                TruePositives = actual.Where((i, idx) => i == 1d && i == predictions[idx]).Count(),
                FalsePositives = actual.Where((i, idx) => (i == -1d || i == 0d) && predictions[idx] == 1d).Count(),
                TrueNegatives = actual.Where((i, idx) => (i == -1d || i == 0d) && i == predictions[idx]).Count(),
                FalseNegatives = actual.Where((i, idx) => i == 1d && (predictions[idx] == 0d || predictions[idx] == -1d)).Count()
            };

            // if the labels are continuous values then calculate accuracy manually
            if (!actual.IsBinary())
            {
                score._TotalAccuracy = (predictions.Where((d, idx) => d == actual[idx]).Count() / predictions.Length);
            }

            return score;
        }
    }
}
