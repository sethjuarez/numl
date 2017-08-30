using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Utils;
using numl.Math;

namespace numl.Supervised
{
    /// <summary>
    /// Contains scoring statistics for a given model.
    /// </summary>
    public class Score
    {
        private double _totalAccuracy = double.NaN;
        private bool _IsBinary = true;

        /// <summary>
        /// Gets or sets the total number of scored examples.
        /// </summary>
        public int Examples { get; set; }

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
        /// Gets or sets the sum of squared errors of the predictions.
        /// </summary>
        public double SSE { get; set; }

        /// <summary>
        /// Gets or sets the mean squared error of the predictions.
        /// </summary>
        public double MSE { get; set; }

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
        /// Gets or sets the cross entropy loss.
        /// </summary>
        public double CrossEntropy { get; set; }

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
        /// Computes the Mean Squared Error of the given inputs.
        /// </summary>
        /// <param name="y1">Predicted values.</param>
        /// <param name="y2">Actual values.</param>
        /// <returns>Double.</returns>
        public static double ComputeSSE(Vector y1, Vector y2)
        {
            return ((y1 - y2) * (y1 - y2)).Sum();
        }

        /// <summary>
        /// Computes the Mean Squared Error of the given inputs.
        /// </summary>
        /// <param name="y1">Predicted values.</param>
        /// <param name="y2">Actual values.</param>
        /// <returns>Double.</returns>
        public static double ComputeMSE(Vector y1, Vector y2)
        {
            return (1.0 / y1.Length) * ComputeSSE(y1, y2);
        }

        /// <summary>
        /// Computes the Root Mean Squared Error for the given inputs.
        /// </summary>
        /// <param name="y1">Predicted values.</param>
        /// <param name="y2">Actual values.</param>
        /// <returns>Double.</returns>
        public static double ComputeRMSE(Vector y1, Vector y2)
        {
            return System.Math.Sqrt(Score.ComputeMSE(y1, y2));
        }

        /// <summary>
        /// Computes the Coefficient of Variation of the Root Mean Squared Error for the given inputs.
        /// </summary>
        /// <param name="y1">Predicted values.</param>
        /// <param name="y2">Actual values.</param>
        /// <returns>Double.</returns>
        public static double ComputeCoefRMSE(Vector y1, Vector y2)
        {
            return numl.Supervised.Score.ComputeRMSE(y1, y2) / y1.Mean();
        }

        /// <summary>
        /// Computes the Normalized Root Mean Squared Error for the given inputs.
        /// </summary>
        /// <param name="y1">Predicted values.</param>
        /// <param name="y2">Actual values.</param>
        /// <returns>Double.</returns>
        public static double ComputeNormRMSE(Vector y1, Vector y2)
        {
            return numl.Supervised.Score.ComputeRMSE(y1, y2) / (y1.Max() - y1.Min());
        }

        /// <summary>
        /// Computes the Mean Absolute Error for the given inputs.
        /// </summary>
        /// <param name="y1">Predicted values.</param>
        /// <param name="y2">Actual values.</param>
        /// <returns></returns>
        public static double ComputeMeanError(Vector y1, Vector y2)
        {
            return (((y1 - y2) * (y1 - y2)).Sqrt().Mean());
        }

        /// <summary>
        /// Computes the Cross Entropy Loss for the given inputs.
        /// </summary>
        /// <param name="y1">Predicted values.</param>
        /// <param name="y2">Actual values.</param>
        /// <returns></returns>
        public static double ComputeCrossEntropy(Vector y1, Vector y2)
        {
            return -(y2 * Vector.Log(y1 + Defaults.Epsilon)).Sum();
        }

        #endregion

        /// <summary>
        /// Returns a string representation of the current Score object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            return $"Score:\n[" +
                    $"\n\tNo# of Predictions:\t{Examples}" +
                    "\n\t---------------------------------" +
                    $"\n\tAccuracy:\t\t{System.Math.Round(Accuracy, 6)}" +
                    $"\n\tMAE:\t\t\t{System.Math.Round(MeanAbsError, 6)}" +
                    $"\n\tSSE:\t\t\t{System.Math.Round(SSE, 6)}" +
                    $"\n\tMSE:\t\t\t{System.Math.Round(MSE, 6)}" +
                    $"\n\tRMSE:\t\t\t{System.Math.Round(RMSE, 6)}" +
                    "\n" +
                    $"\n\tCoef RMSE:\t\t{System.Math.Round(CoefRMSE, 6)}" +
                    $"\n\tNorm RMSE:\t\t{System.Math.Round(NormRMSE, 6)}" +
                    "\n" +
                    (this._IsBinary ? 
                      ( $"\n\tAUC:\t\t\t{System.Math.Round(AUC, 6)}" +
                        "\n\t---------------------------------" +
                        $"\n\tTrue Positives:\t{TruePositives}" +
                        $"\n\tFalse Positives:\t{FalsePositives}" +
                        $"\n\tTrue Negatives:\t{TrueNegatives}" +
                        $"\n\tFalse Negatives:\t{FalseNegatives}" +
                        "\n\t---------------------------------" +
                        $"\n\tPrecision:\t\t{System.Math.Round(Precision, 6)}" +
                        $"\n\tRecall:\t\t{System.Math.Round(Recall, 6)}" +
                        $"\n\tSpecificity:\t\t{System.Math.Round(Specificity, 6)}" +
                        $"\n\tFallout:\t\t{System.Math.Round(Fallout, 6)}" +
                        $"\n\tF-Score:\t\t{System.Math.Round(FScore, 6)}" +
                        $"\n\tCross-Entropy:\t\t{System.Math.Round(CrossEntropy, 6)}")
                       
                        : string.Empty +
                    "\n]");
        }

        /// <summary>
        /// Scores a set of predictions against the actual values.
        /// </summary>
        /// <param name="predictions">Predicted values.</param>
        /// <param name="actual">Actual values.</param>
        /// <param name="truthLabel">(Optional) the truth label in the <paramref name="actual"/> vector.</param>
        /// <param name="falseLabel">(Optional) the false label in the <paramref name="actual"/> vector.</param>
        /// <returns></returns>
        public static Score ScorePredictions(Vector predictions, Vector actual, 
                                            double truthLabel = Ject.DefaultTruthValue, double falseLabel = Ject.DefaultFalseValue)
        {
            var score = new numl.Supervised.Score()
            {
                TotalPositives = actual.Where(w => w == truthLabel).Count(),
                TotalNegatives = actual.Where(w => (w == falseLabel || w != truthLabel)).Count(),

                TruePositives = actual.Where((i, idx) => i == truthLabel && i == predictions[idx]).Count(),
                FalsePositives = actual.Where((i, idx) => (i == falseLabel || i != truthLabel) && predictions[idx] == truthLabel).Count(),

                TrueNegatives = actual.Where((i, idx) => (i == falseLabel || i != truthLabel) && predictions[idx] != truthLabel).Count(),
                FalseNegatives = actual.Where((i, idx) => i == truthLabel && (predictions[idx] == falseLabel || predictions[idx] != truthLabel)).Count(),

                Examples = predictions.Length
            };

            score._IsBinary = actual.IsBinary();

            // if the labels are continuous values then calculate accuracy manually
            if (!score._IsBinary)
            {
                score._totalAccuracy = (predictions.Where((d, idx) => d == actual[idx]).Count() / predictions.Length);
            }

            score.RMSE = Score.ComputeRMSE(predictions, actual);
            score.CoefRMSE = Score.ComputeCoefRMSE(predictions, actual);
            score.NormRMSE = Score.ComputeRMSE(predictions, actual);
            score.MeanAbsError = Score.ComputeMeanError(predictions, actual);
            score.SSE = Score.ComputeSSE(predictions, actual);
            score.MSE = Score.ComputeMSE(predictions, actual);
            score.CrossEntropy = Score.ComputeCrossEntropy(predictions, actual);

            return score;
        }

        /// <summary>
        /// Combines and averages metrics across all the given scores.
        /// </summary>
        /// <param name="scores">Scores.</param>
        /// <returns></returns>
        public static Score CombineScores(params Score[] scores)
        {
            if (scores == null) return null;

            Score result = new Score();

            result.Accuracy = scores.Average(s => s.Accuracy);
            result.CoefRMSE = scores.Average(s => s.CoefRMSE);
            result.Examples = scores.Sum(s => s.Examples);

            result.MSE = scores.Sum(s => s.MSE);
            
            result.MeanAbsError = scores.Average(s => s.MeanAbsError);
            result.NormRMSE = scores.Average(s => s.NormRMSE);
            result.RMSE = scores.Average(s => s.RMSE);

            result.TotalNegatives = scores.Sum(s => s.TotalNegatives);
            result.TotalPositives = scores.Sum(s => s.TotalPositives);

            result.TrueNegatives = scores.Sum(s => s.TrueNegatives);
            result.TruePositives = scores.Sum(s => s.TruePositives);
            result.FalseNegatives = scores.Sum(s => s.FalseNegatives);
            result.FalsePositives = scores.Sum(s => s.FalsePositives);

            return result;
        }
    }
}
