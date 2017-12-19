using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using numl.Supervised.NaiveBayes;
using numl.Tests.Data;
using numl.Model;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class NaiveBayesTests : BaseSupervised
    {
        [Fact]
        public void Tennis_Tests()
        {
            TennisPrediction(new NaiveBayesGenerator(2));
        }

        [Fact]
        public void House_Tests()
        {
            HousePrediction(new NaiveBayesGenerator(2));
        }

        [Fact]
        public void Iris_Tests()
        {
            IrisPrediction(new NaiveBayesGenerator(2));
        }

        [Fact]
        public void Tennis_Learner_Tests()
        {
            TennisLearnerPrediction(new NaiveBayesGenerator(2));
        }

        [Fact]
        public void House_Learner_Tests()
        {
            HouseLearnerPrediction(new NaiveBayesGenerator(2));
        }

        [Fact]
        public void Iris_Learner_Tests()
        {
            IrisLearnerPrediction(new NaiveBayesGenerator(2));
        }

      [Fact]
      public void Tennis_Probabilities_Count_Tests()
      {
         var data = Tennis.GetData();
         var generator = new NaiveBayesGenerator(2);

         Probabilities_Count(generator, data);
      }

      [Fact]
      public void House_Probabilities_Count_Tests()
      {
         var data = House.GetData();
         var generator = new NaiveBayesGenerator(2);

         Probabilities_Count(generator, data);
      }

      [Fact]
       public void Iris_Probabilities_Count_Tests()
       {
          var data = Iris.Load();
          var generator = new NaiveBayesGenerator(2);

         Probabilities_Count(generator, data);
      }

      private static void Probabilities_Count<T>(NaiveBayesGenerator generator, IEnumerable<T> data)
          where T : class
      {
         generator.Descriptor = Descriptor.Create<T>();
         LearningModel model = Learner.Learn(data, 1.0, 1, generator);

         NaiveBayesModel innerModel = model.Model as NaiveBayesModel;

         foreach (Statistic rootProbability in innerModel.Root.Probabilities)
         {
            foreach(Measure conditional in rootProbability.Conditionals)
            {
               int totalCount = 0;

               foreach (Statistic measureProbability in conditional.Probabilities)
               {
                  Assert.True(measureProbability.Count <= rootProbability.Count);

                  totalCount += measureProbability.Count;
               }

               Assert.True(totalCount == rootProbability.Count);
            }
         }
      }
   }
}
