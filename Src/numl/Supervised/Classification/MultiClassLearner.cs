using numl.Math.LinearAlgebra;
using numl.Math.Probability;
using numl.Model;
using numl.Supervised;
using numl.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using numl.Supervised.Classification;
using System.Threading.Tasks;

using System.Threading;

namespace numl
{
    /// <summary>
    /// Primary class for running classification models. It is designed to abstract the separation of
    /// training and test sets as well as select best result across all classes.
    /// </summary>
    public static class MultiClassLearner
    {
        static MultiClassLearner()
        {
            Sampling.SetSeedFromSystemTime();
        }

        /// <summary>
        /// Returns a Vector of positive and negative labels in 1 - 0 form.
        /// </summary>
        /// <param name="examples">Object examples.</param>
        /// <param name="descriptor">Descriptor.</param>
        /// <param name="truthLabel">The truth label's value (see <see cref="LabelAttribute"/>).</param>
        /// <returns></returns>
        public static Vector ChangeClassLabels(object[] examples, Descriptor descriptor, object truthLabel)
        {
            Vector y = new Vector(examples.Length);

            for (int i = 0; i < y.Length; i++)
            {
                y[i] = (descriptor.GetValue(examples.ElementAt(i), descriptor.Label).Equals(truthLabel) ? 1.0 : 0.0);
            }

            return y;
        }

        /// <summary>
        /// Generates and returns a new Tuple of objects: IClassifier, Score and object state
        /// </summary>
        /// <param name="generator">Generator to use for the model.</param>
        /// <param name="truthExamples">True examples.</param>
        /// <param name="falseExamples">False examples.</param>
        /// <param name="truthLabel">Truth label object.</param>
        /// <param name="trainingPct">Training percentage.</param>
        /// <param name="state">Object state</param>
        /// <returns></returns>
        private static Tuple<IClassifier, Score, object> GenerateModel(IGenerator generator, object[] truthExamples, object[] falseExamples, 
                                                                                object truthLabel, double trainingPct, object state = null)
        {
            Descriptor descriptor = generator.Descriptor;

            object[] examples = truthExamples.Union(falseExamples).Shuffle().ToArray(); // changed from .Shuffle()

            int total = examples.Count();

            int trainingCount = (int)System.Math.Floor((double)total * trainingPct);

            //// 100 - trainingPercentage for testing
            int[] testingSlice = Learner.GetTestPoints(total - trainingCount, total).ToArray();
            int[] trainingSlice = Learner.GetTrainingPoints(testingSlice, total).ToArray();

            var training = generator.Descriptor.Convert(examples.Slice(trainingSlice).ToArray(), true).ToExamples();

            // convert label to 1's and 0's
            Vector y = MultiClassLearner.ChangeClassLabels(examples.ToArray(), descriptor, truthLabel);

            IModel model = generator.Generate(training.X, y.Slice(trainingSlice));

            Score score = new Score();

            if (testingSlice.Count() > 0)
            {
                object[] testExamples = examples.Slice(testingSlice).ToArray();
                var testing = generator.Descriptor.Convert(testExamples, true).ToExamples();
                
                Vector y_pred = new Vector(testExamples.Length);

                // make sure labels are 1 / 0 based
                Vector y_test = MultiClassLearner.ChangeClassLabels(testExamples.ToArray(), descriptor, truthLabel);

                for (int i = 0; i < testExamples.Length; i++)
                {
                    double result = model.Predict(testing.X[i, VectorType.Row]);

                    y_pred[i] = result;
                }

                score = Score.ScorePredictions(y_pred, y_test);
            }
            return new Tuple<IClassifier, Score, object>((IClassifier)model, score, state);
        }

        /// <summary>
        /// Generate a multi-class classification model using a specialist classifier for each class label.
        /// </summary>
        /// <param name="generator">The generator to use for each individual classifier.</param>
        /// <param name="examples">Training examples of any number of classes</param>
        /// <param name="trainingPercentage">Percentage of training examples to use, i.e. 70% = 0.7</param>
        /// <param name="mixingPercentage">Percentage to mix positive and negative exmaples, i.e. 50% will add an additional 50% of 
        ///   <paramref name="trainingPercentage"/> of negative examples into each classifier when training.</param>
        /// <param name="isMultiClass">Determines whether each class is mutually inclusive. 
        ///   <para>For example: If True, each class takes on a number of classes and does not necessarily belong to one specific class.</para>
        ///   <para>The ouput would then be a number of predicted classes for a single prediction.  E.g. A song would be True as it may belong to classes: vocals, rock as well as bass.</para>
        /// </param>
        /// <returns></returns>
        public static ClassificationModel Learn(IGenerator generator, IEnumerable<object> examples, double trainingPercentage, double mixingPercentage = 0.5, bool isMultiClass = true)
        {
            Descriptor descriptor = generator.Descriptor;

            trainingPercentage = (trainingPercentage > 1.0 ? trainingPercentage / 100 : trainingPercentage);
            mixingPercentage = (mixingPercentage > 1.0 ? mixingPercentage / 100 : mixingPercentage);

            var classGroups = examples.Select(s => new
                                                {
                                                    Label = generator.Descriptor.GetValue(s, descriptor.Label),
                                                    Item = s
                                                })
                                       .GroupBy(g => g.Label)
                                       .ToDictionary(k => k.Key, v => v.Select(s => s.Item).ToArray());

            int classes = classGroups.Count();

            Dictionary<object, IClassifier> models = null;

            Score finalScore = new Score();

            if (classes > 2)
            {
                models = new Dictionary<object, IClassifier>(classes);

                Task<Tuple<IClassifier, Score, object>>[] learningTasks = new Task<Tuple<IClassifier, Score, object>>[classes];

                for (int y = 0; y < classes; y++)
                {
                    models.Add(classGroups.ElementAt(y).Key, null);

                    int mix = (int)System.Math.Ceiling(((classGroups.ElementAt(y).Value.Count() * trainingPercentage) * mixingPercentage) / classes);
                    object label = classGroups.ElementAt(y).Key;
                    object[] truthExamples = classGroups.ElementAt(y).Value;
                    object[] falseExamples = classGroups.Where(w => w.Key != classGroups.Keys.ElementAt(y))
                                                        .SelectMany(s => s.Value.Take(mix).ToArray())
                                                        .ToArray();

                    learningTasks[y] = Task.Factory.StartNew(
                            () => MultiClassLearner.GenerateModel(generator, truthExamples, falseExamples, label, trainingPercentage, label)
                        );
                }

                Task.WaitAll(learningTasks);

                Score[] scores = new Score[learningTasks.Count()];

                for (int c = 0; c < learningTasks.Count(); c++)
                {
                    models[learningTasks[c].Result.Item3] = learningTasks[c].Result.Item1;
                    scores[c] = learningTasks[c].Result.Item2;
                }

                finalScore = Score.CombineScores(scores);
            }
            else
            {
                // fallback to single classifier for two class classification

                var dataset = descriptor.Convert(examples, true).ToExamples();
                var positives = examples.Slice(dataset.Y.Indices(f => f == 1d)).ToArray();
                var negatives = examples.Slice(dataset.Y.Indices(w => w != 1d)).ToArray();

                var label = generator.Descriptor.GetValue(positives.First(), descriptor.Label);

                var model = MultiClassLearner.GenerateModel(generator, positives, negatives, label, trainingPercentage, label);
                finalScore = model.Item2;

                models = new Dictionary<object, IClassifier>() { { label, model.Item1 } };
            }

            ClassificationModel classificationModel = new ClassificationModel()
            {
                Generator = generator,
                Classifiers = models,
                IsMultiClass = isMultiClass,
                Score = finalScore
            };

            return classificationModel;
        }
    }
}
