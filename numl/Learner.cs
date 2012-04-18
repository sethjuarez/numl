using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Supervised;
using numl.Math;
using numl.Model;
using System.Threading.Tasks;
using numl.Math.Probability;
using System.Collections;

namespace numl
{
    public class Learner
    {
        public IGenerator[] Generators { get; private set; }
        public IModel[] Models { get; private set; }
        public Vector Accuracy { get; private set; }

        public virtual Tuple<IGenerator, IModel, double> this[int i]
        {
            get
            {
                return new Tuple<IGenerator, IModel, double>(Generators[i], Models[i], Accuracy[i]);
            }
        }

        public Learner(params IGenerator[] generators)
        {
            MLRandom.SetSeedFromSystemTime();
            Generators = generators;
        }

        /// <summary>
        /// Given the labeled description, this method creates models for
        /// each generator provided in the ctor using an 80/20 training/test
        /// slice
        /// </summary>
        /// <param name="description"></param>
        /// <param name="examples"></param>
        public void Learn(LabeledDescription description, IEnumerable<object> examples)
        {
            var total = examples.Count();
            
            var trainingCount = (int)System.Math.Ceiling(total * .8);

            // 20% for testing
            var testingSlice = GetTestPoints(total - trainingCount, total)
                                    .ToArray();
            // 80% for training
            var trainingSlice = GetTrainingPoints(testingSlice, total);

            var data = description.ToExamples(examples);

            Matrix x = data.Item1;
            Vector y = data.Item2;

            // training
            var x_t = x.Slice(trainingSlice, VectorType.Row);
            var y_t = y.Slice(trainingSlice);

            foreach (IGenerator generator in Generators)
                generator.Description = description;

            Models = new IModel[Generators.Length];

            // run in parallel since they all have 
            // read-only references to the data model
            // and update independently to different
            // spots
            Parallel.For(0, Models.Length, i =>
            {
                Models[i] = Generators[i].Generate(x_t, y_t);
                Models[i].Description = description;
            });

            // testing            
            object[] test = GetTestExamples(testingSlice, examples);
            Accuracy = Vector.Zeros(Models.Length);
            for (int i = 0; i < Models.Length; i++)
            {
                Accuracy[i] = 0;
                for (int j = 0; j < test.Length; j++)
                {
                    var truth = R.Get(test[j], description.Label.Name);

                    var features = description.ToVector(test[j]);
                    var p = Models[i].Predict(features);

                    // set prediction
                    Models[i].Predict(test[j]);

                    // get prediction
                    var pred = R.Get(test[j], description.Label.Name);

                    if (truth.Equals(pred))
                        Accuracy[i] += 1;

                    // put it back to the way it was
                    R.Set(test[j], description.Label.Name, truth);
                }

                Accuracy[i] /= test.Length;
            };
        }

        private object[] GetTestExamples(IEnumerable<int> slice, IEnumerable<object> examples)
        {
            return examples
                    .Where((o, i) => slice.Contains(i))
                    .ToArray();
        }

        private IEnumerable<int> GetTestPoints(int testCount, int total)
        {
            List<int> taken = new List<int>(testCount);
            while (taken.Count < testCount)
            {
                int i = MLRandom.GetUniform(total);
                if (!taken.Contains(i))
                {
                    taken.Add(i);
                    yield return i;
                }
            }
        }

        private IEnumerable<int> GetTrainingPoints(IEnumerable<int> testPoints, int total)
        {
            for (int i = 0; i < total; i++)
                if (!testPoints.Contains(i))
                    yield return i;
        }
    }
}
