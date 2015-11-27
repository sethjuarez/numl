using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised
{
    /// <summary>
    /// Contains scoring statistics for a given model.
    /// </summary>
    public class Score
    {
        private double _totalAccuracy = double.NaN;

        /// <summary>
        /// Gets or sets the total test accuracy as defined by the normalized distribution over true vs negative cases.
        /// </summary>
        public double Accuracy
        {
            get
            {
                if (double.IsNaN(_totalAccuracy))
                {
                    _totalAccuracy = 
                        ((double)TruePositives + (double)TrueNegatives) / 
                        ((double)TruePositives + (double)TrueNegatives + (double)FalsePositives + (double)FalseNegatives);
                }
                return _totalAccuracy;
            }
            set
            {
                _totalAccuracy = value;
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
        /// Gets or sets the Root Mean Squared Error of the predictions.
        /// </summary>
        public double RMSE { get; set; }

        /// <summary>
        /// Gets or sets the Coefficient of Variation of the RMSE.
        /// </summary>
        public double CoefRMSE { get; set; }

        /// <summary>
        /// Gets or sets the normalised RMSE.
        /// </summary>
        public double NormRMSE { get; set; }

        /// <summary>
        /// Gets or sets the mean absolute error.
        /// </summary>
        public double MeanAbsError {  get; set; }

        /// <summary>
        /// Gets the Specificity of the model.
        /// <para>A higher value indicates the model has scored better at classifying negative examples, otherwise known as the True-Negative-Rate (TNR). </para>
        /// </summary>
        public double Specificity
        {
            get
            {
                return (double)TrueNegatives / ((double)TrueNegatives + (double)FalsePositives);
            }
        }

        /// <summary>
        /// Gets the Fallout value of the model.
        /// <para>A higher value indicates the model has decreased prediction accuracy, otherwise known as the False-Positive-Rate (FPR).</para>
        /// </summary>
        public double Fallout
        {
            get
            {
                return (double)FalsePositives / ((double)FalsePositives + (double)TrueNegatives);
            }
        }

        /// <summary>
        /// Gets the Precision of the model.
        /// <para>A higher precision indicates the model has a higher positive prediction confidence.  Also known as the Positive-Predictive-Value (PPV).</para>
        /// </summary>
        public double Precision
        {
            get
            {
                return (double)TruePositives / ((double)TruePositives + (double)FalsePositives);
            }
        }

        /// <summary>
        /// Gets the Recall of the model.
        /// <para>A higher recall indicates the model has scored better on reducing false negative predictions.  Also known as the Sensitivity or True-Positive-Rate (TPR).</para>
        /// </summary>
        public double Recall
        {
            get
            {
                return (double)TruePositives / ((double)TruePositives + (double)FalseNegatives);
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
                return 2.0 * ((Precision * Recall) / (Precision + Recall));
            }
        }

        /// <summary>
        /// Gets the Area Under the Curve value for the current fixed stationary point of the Precision / Recall curve.
        /// </summary>
        public double AUC
        {
            get
            {
                return (Precision * Recall);
            }
        }

        /// <summary>
        /// Initializes a new Score object.
        /// </summary>
        public Score()
        {
            FalseNegatives = FalsePositives = TotalNegatives = 
                TotalPositives = TruePositives = TrueNegatives = 0;
        }

        #region Static Methods

        /// <summary>
        /// Computes the Root Mean Squared Error for the given inputs.
        /// </summary>
        /// <param name="y1">Target values.</param>
        /// <param name="y2">Actual values.</param>
        /// <returns>Double.</returns>
        public static double ComputeRMSE(Vector y1, Vector y2)
        {
            return System.Math.Sqrt(((y1 - y2) * (y1 - y2)).Sum() / (double)y1.Length);
        }

        /// <summary>
        /// Computes the Coefficient of Variation of the Root Mean Squared Error for the given inputs.
        /// </summary>
        /// <param name="y1">Target values.</param>
        /// <param name="y2">Actual values.</param>
        /// <returns>Double.</returns>
        public static double ComputeCoefRMSE(Vector y1, Vector y2)
        {
            return numl.Supervised.Score.ComputeRMSE(y1, y2) / y1.Mean();
        }

        /// <summary>
        /// Computes the Normalized Root Mean Squared Error for the given inputs.
        /// </summary>
        /// <param name="y1">Target values.</param>
        /// <param name="y2">Actual values.</param>
        /// <returns>Double.</returns>
        public static double ComputeNormRMSE(Vector y1, Vector y2)
        {
            return numl.Supervised.Score.ComputeRMSE(y1, y2) / (y1.Max() - y1.Min());
        }

        /// <summary>
        /// Computes the Mean Absolute Error for the given inputs.
        /// </summary>
        /// <param name="y1">Target values.</param>
        /// <param name="y2">Actual values.</param>
        /// <returns></returns>
        public static double ComputeMeanError(Vector y1, Vector y2)
        {
            return (((y1 - y2) * (y1 - y2)).Sqrt().Mean());
        }

        #endregion

        /// <summary>
        /// Scores a set of predictions against the actual values.
        /// </summary>
        /// <param name="predictions">Predicted values.</param>
        /// <param name="actual">Actual values.</param>
        /// <returns></returns>
        public static numl.Supervised.Score ScorePredictions(Vector predictions, Vector actual)
        {
            // TODO: This may not be computing correctly.
            var score = new numl.Supervised.Score()
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
                score._totalAccuracy = (predictions.Where((d, idx) => d == actual[idx]).Count() / predictions.Length);
            }

            score.RMSE = Score.ComputeRMSE(predictions, actual);
            score.CoefRMSE = Score.ComputeCoefRMSE(predictions, actual);
            score.NormRMSE = Score.ComputeRMSE(predictions, actual);
            score.MeanAbsError = Score.ComputeMeanError(predictions, actual);

            return score;
        }
    }
}
