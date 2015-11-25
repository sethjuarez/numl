using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Supervised
{
    /// <summary>
    /// The Scoring metric to use for selecting models.
    /// </summary>
    public enum ScoringMetric
    {
        /// <summary>
        /// F1 Score (Precision vs Recall)
        /// </summary>
        FScore,
        /// <summary>
        /// Accuracy
        /// </summary>
        Accuracy,
        /// <summary>
        /// Root Mean Squared Error
        /// </summary>
        RMSE,
        /// <summary>
        /// Normalized Root Mean Squared Error
        /// </summary>
        NormRMSE,
        /// <summary>
        /// Precision or PPV (positive predictive value)
        /// </summary>
        Precision,
        /// <summary>
        /// Recall or Sensitivity
        /// </summary>
        Recall,
        /// <summary>
        /// Fallout or FPR (false positive rate)
        /// </summary>
        Fallout,
        /// <summary>
        /// Specificity or TNR (true negative rate value)
        /// </summary>
        Specificity,
        /// <summary>
        /// AUC of the PR curve
        /// </summary>
        AUC
    }
}
