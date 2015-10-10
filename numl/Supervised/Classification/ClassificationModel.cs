using numl.Math.LinearAlgebra;
using numl.Supervised;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Supervised.Classification
{
    /// <summary>
    /// Generated Classification model.
    /// </summary>
    public class ClassificationModel : LearningModel
    {
        /// <summary>
        /// Gets or sets whether an item can belong to one or more classes.
        /// <para>For example: a song may take on one or more classes: Guitar, Drums and Vocals (i.e. not mutually exclusive) where as the genre is mutually exclusive.</para>
        /// </summary>
        public bool IsMultiClass { get; set; }

        /// <summary>
        /// Dictionary of individual specialist classifiers
        /// </summary>
        public Dictionary<object, IClassifier> Classifiers { get; set; }

        /// <summary>
        /// Instantiate a new ClassificationModel object.
        /// </summary>
        public ClassificationModel()
        {
        }

        /// <summary>
        /// Predict the given Label across all classifiers for the current object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Label"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public Label Predict<T, Label>(T o)
        {
            return (Label)this.Predict(o);
        }

        /// <summary>
        /// Predict all given Labels across all classifiers for the current object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Label"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public Label[] PredictClasses<T, Label>(T o)
        {
            return this.PredictClasses(o).Select(s => (Label)s).ToArray();
        }

        /// <summary>
        /// Predicts the given Label from the object.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public object Predict(object o)
        {
            Vector current = this.Generator.Descriptor.Convert(o, false).ToVector();

            var predictions = this.Classifiers.Select(s => new Tuple<object, double>(s.Key, s.Value.PredictRaw(current)))
                                             .OrderByDescending(or => or.Item2).ToArray();

            return predictions.FirstOrDefault().Item1;
        }

        /// <summary>
        /// Predict all given Labels across all classifiers for the current object.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public IEnumerable<object> PredictClasses(object o)
        {
            Vector current = this.Generator.Descriptor.ToVector(o);

            var prediction = this.Classifiers.Select(s => new Tuple<object, double>(s.Key, s.Value.PredictRaw(current)))
                                             .OrderByDescending(or => or.Item2);

            double sum = prediction.Sum(s => s.Item2);
            foreach (var predict in prediction)
            {
                if (predict.Item2 >= 0.5)
                    yield return predict.Item1;
            }
        }
    }
}
