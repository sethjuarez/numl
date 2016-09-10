using numl.Supervised;
using numl.Tests;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.ScoringTests
{
    [Trait("Category", "Scoring")]
    public class ScoreTests
    {
        private const double Delta = 0.001;

        [Theory]
        [InlineData(new[] { -1d, -1d, 1d, -1d, 1d, -1d, 1d, 1d, 1d, -1d },
                  new[] { -1d, 1d, 1d, 1d, 1d, -1d, 1d, -1d, 1d, -1d },
                  4, 3, 0.7, 0.8, 0.667, 0.533, 0.727)]
        [InlineData(new[] { 0d, 0d, 1d, 0d, 0d, 0d, 1d, 0d, 0d, 0d },
                  new[] { 0d, 0d, 1d, 0d, 0d, 0d, 1d, 1d, 0d, 0d },
                  2, 7, 0.9, 1.0, 0.667, 0.667, 0.8)]
        [InlineData(new[] { 0d, 1d, 1d, 0d, 1d, 0d, 1d, 0d, 0d, 1d },
                  new[] { 0d, 1d, 0d, 0d, 1d, 0d, 1d, 1d, 0d, 1d },
                  4, 4, 0.8, 0.8, 0.8, 0.64, 0.8)]
        public void Score_Discrete_All(double[] test, double[] actual, int truepos, int trueneg, double accuracy, double precision, double recall, double auc, double fScore)
        {
            var scores = Score.ScorePredictions(test, actual);

            Almost.Equal(truepos, scores.TruePositives, Delta, "True Positives");
            Almost.Equal(trueneg, scores.TrueNegatives, Delta, "True Negatives");
            Almost.Equal(accuracy, scores.Accuracy, Delta, "Accuracy");
            Almost.Equal(precision, scores.Precision, Delta, "Precision");
            Almost.Equal(recall, scores.Recall, Delta, "Recall");
            Almost.Equal(auc, scores.AUC, Delta, "AUC");
            Almost.Equal(fScore, scores.FScore, Delta, "FScore");
        }

        [Theory]
        [InlineData(new[] { 74.98, 79.26, 75.26, 80.31, 82.68, 82.66, 77.49, 77.67, 77.14, 82.23,
                          80.31, 79.95, 78.46, 76.61, 73.87, 90.31, 89.95, 80.33, 78.38 },
                  new[] { 74.978733, 79.261027, 75.259871, 80.303349, 82.682547, 82.662863, 77.49019, 77.669849, 77.139804, 82.230296,
                          80.309399, 79.949135, 78.448338, 76.608467, 73.87774, 90.310994, 89.949127, 80.338398, 78.379833 },
                  0.0041, 0.0025)]
        public void Score_Continuous_All(double[] test, double[] actual, double rmse, double mae)
        {
            var scores = Score.ScorePredictions(test, actual);

            Assert.True(scores.RMSE >= rmse);
            Assert.True(scores.MeanAbsError >= mae);
        }
    }
}
