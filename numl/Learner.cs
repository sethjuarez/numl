using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Supervised;
using numl.Math;
using numl.Model;

namespace numl
{
    public class Learner
    {
        private static readonly Random r = new Random(DateTime.Now.Millisecond);
        public IGenerator[] Generators { get; private set; }
        public IModel[] Models { get; private set; }
        public Vector Accuracy { get; private set; }


        public Learner(params IGenerator[] generators)
        {
            Generators = generators;
        }

        public void Learn(Description description, Property label, IEnumerable<object> examples)
        {
            var total = examples.Count();
            
            var trainingCount = (int)System.Math.Ceiling(total * .8);

            // 20% for testing
            var testingSlice = GetTestPoints(total - trainingCount, total)
                                    .ToArray();
            // 80% for training
            var trainingSlice = GetTrainingPoints(testingSlice, total);


        }

        private IEnumerable<int> GetTestPoints(int testCount, int total)
        {
            List<int> taken = new List<int>(testCount);
            while (taken.Count < testCount)
            {
                int i = r.Next(total);
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
