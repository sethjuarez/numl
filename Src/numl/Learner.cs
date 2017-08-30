// file:	Learner.cs
//
// summary:	Implements the learner class
using System;
using numl.Utils;
using System.Linq;
using numl.Supervised;
using numl.Math.Probability;
using System.Threading.Tasks;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;


namespace numl
{
    /// <summary>
    /// Primary class for running model generators. It is designed to abstract the separation of
    /// training and test sets as well as best model selection.
    /// </summary>
    public static class Learner
    {
        /// <summary>Static constructor.</summary>
        static Learner()
        {
            Sampling.SetSeedFromSystemTime();
        }
        /// <summary>Retrieve best model (or model with the highest accuracy)</summary>
        /// <param name="models">List of models.</param>
        /// <param name="metric">Scoring metric to use for model selection.</param>
        /// <returns>Best Model.</returns>
        public static LearningModel Best(this IEnumerable<LearningModel> models, ScoringMetric metric = ScoringMetric.Accuracy)
        {
            return models.OrderByDescending(
                m => {
                    switch (metric)
                    {
                        case ScoringMetric.Accuracy: return m.Accuracy;
                        case ScoringMetric.FScore: return m.Score.FScore;
                        case ScoringMetric.Precision: return m.Score.Precision;
                        case ScoringMetric.Recall: return m.Score.Recall;
                        case ScoringMetric.RMSE: return m.Score.RMSE;
                        case ScoringMetric.NormRMSE: return m.Score.NormRMSE;
                        case ScoringMetric.AUC: return m.Score.AUC;
                        case ScoringMetric.Fallout: return m.Score.Fallout;
                        case ScoringMetric.Specificity: return m.Score.Specificity;
                        default: return m.Accuracy;
                    }
                }).FirstOrDefault();
        }
        /// <summary>
        /// Trains an arbitrary number of models on the provided examples by creating a separation of
        /// data based on training percentage. Each generator is rerun a predetermined amount of times.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="examples">Source data.</param>
        /// <param name="trainingPercentage">Data split percentage.</param>
        /// <param name="repeat">Number of repetitions per generator.</param>
        /// <param name="generators">Model generators used.</param>
        /// <returns>Best models for each generator.</returns>
        public static LearningModel[] Learn(IEnumerable<object> examples, double trainingPercentage, int repeat, params IGenerator[] generators)
        {
            if (generators.Length == 0)
                throw new InvalidOperationException("Need to have at least one generator!");

            // set up models
            var models = new LearningModel[generators.Length];

            for (int i = 0; i < generators.Length; i++)
                models[i] = Learn(examples, trainingPercentage, repeat, generators[i]);

            return models;
        }

        /// <summary>
        /// Trains a single model based on a generator a predefined number of times with the provided
        /// examples and data split and selects the best (or most accurate) model.
        /// </summary>
        /// <param name="examples">Source data.</param>
        /// <param name="trainingPercentage">Data split percentage.</param>
        /// <param name="repeat">Number of repetitions per generator.</param>
        /// <param name="generator">Model generator used.</param>
        /// <returns>Best model for provided generator.</returns>
        public static LearningModel Learn(IEnumerable<object> examples, double trainingPercentage, int repeat, IGenerator generator)
        {
            // count only once
            var total = examples.Count();
            var descriptor = generator.Descriptor;
            var (x, y) = descriptor.Convert(examples).ToExamples();

            var models = new IModel[repeat];
            //var accuracy = Vector.Zeros(repeat);
            var scores = new Score[repeat];

            if (trainingPercentage > 1.0) trainingPercentage /= 100.0;

            // safe for parallelization
            // read-only references to the data model
            // and update indices independently
            for (int i = 0; i < models.Length; i++)
            {
                var t = GenerateModel(generator, x, y, examples, trainingPercentage, total);
                models[i] = t.Model;
                scores[i] = t.Score;
            }

            int idx = scores.Select(s => s.RMSE).MinIndex();

            // sanity check, for convergence failures
            if (idx < 0 && trainingPercentage < 1d) throw new Exception("All models failed to initialize properly");
            else if (idx < 0) idx = 0;

            return new LearningModel { Generator = generator, Model = models[idx], Score = scores[idx] };
        }
        /// <summary>Generates a model.</summary>
        /// <param name="generator">Model generator used.</param>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <param name="examples">Source data.</param>
        /// <param name="trainingPct">The training pct.</param>
        /// <param name="total">Number of Examples</param>
        /// <returns>The model.</returns>
        private static LearningModel GenerateModel(IGenerator generator, Matrix x, Vector y, IEnumerable<object> examples, double trainingPct, int total)
        {
            var descriptor = generator.Descriptor;
            //var total = examples.Count();
            var trainingCount = (int)System.Math.Floor(total * trainingPct);

            // 100 - trainingPercentage for testing
            var testingSlice = GetTestPoints(total - trainingCount, total).ToArray();

            // trainingPercentage for training
            var trainingSlice = GetTrainingPoints(testingSlice, total).ToArray();

            // training
            var x_t = x.Slice(trainingSlice);
            var y_t = y.Slice(trainingSlice);

            // generate model
            var model = generator.Generate(x_t, y_t);
            model.Descriptor = descriptor;

            Score score = new Score();

            if (testingSlice.Count() > 0)
            {
                // testing            
                object[] test = GetTestExamples(testingSlice, examples);
                Vector y_pred = new Vector(test.Length);
                Vector y_test = descriptor.ToExamples(test).Y;

                bool isBinary = y_test.IsBinary();
                if (isBinary)
                    y_test = y_test.ToBinary(f => f == 1d, 1.0, 0.0);

                for (int j = 0; j < test.Length; j++)
                {
                    // items under test
                    object o = test[j];

                    // make prediction
                    var features = descriptor.Convert(o, false).ToVector();
                    // --- temp changes ---
                    double val = model.Predict(features);
                    var pred = descriptor.Label.Convert(val);

                    var truth = Ject.Get(o, descriptor.Label.Name);

                    if (truth.Equals(pred))
                        y_pred[j] = y_test[j];
                    else
                        y_pred[j] = (isBinary ? (y_test[j] >= 1d ? 0d : 1d) : val);
                }

                // score predictions
                score = Score.ScorePredictions(y_pred, y_test);
            }

            return new LearningModel { Generator = generator, Model = model, Score = score };
        }

        /// <summary>Gets test examples.</summary>
        /// <param name="slice">The slice.</param>
        /// <param name="examples">Source data.</param>
        /// <returns>An array of object.</returns>
        internal static object[] GetTestExamples(IEnumerable<int> slice, IEnumerable<object> examples)
        {
            return examples
                    .Where((o, i) => slice.Contains(i))
                    .ToArray();
        }
        /// <summary>Gets the test points in this collection.</summary>
        /// <param name="testCount">Number of tests.</param>
        /// <param name="total">Number of.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the test points in this collection.
        /// </returns>
        internal static IEnumerable<int> GetTestPoints(int testCount, int total)
        {
            List<int> taken = new List<int>(testCount);
            while (taken.Count < testCount)
            {
                int i = Sampling.GetUniform(total);
                if (!taken.Contains(i) && i >= 0 && i < total)
                {
                    taken.Add(i);
                    yield return i;
                }
            }
        }
        /// <summary>Gets the training points in this collection.</summary>
        /// <param name="testPoints">The test points.</param>
        /// <param name="total">Number of.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the training points in this
        /// collection.
        /// </returns>
        internal static IEnumerable<int> GetTrainingPoints(IEnumerable<int> testPoints, int total)
        {
            for (int i = 0; i < total; i++)
                if (!testPoints.Contains(i))
                    yield return i;
        }
    }
}
