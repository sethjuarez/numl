using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using numl.Supervised.NaiveBayes;

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
    }
}
