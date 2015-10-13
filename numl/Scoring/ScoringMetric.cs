using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Scoring
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
        /// Precision or PPV (positive predictive value)
        /// </summary>
        Precision,
        /// <summary>
        /// Recall or sensitivity
        /// </summary>
        Recall
    }
}
