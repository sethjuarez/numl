using System;
using numl.Model;
using System.Linq;
using NUnit.Framework;
using numl.Supervised.KNN;
using System.Collections.Generic;
using numl.Tests.Data;

namespace numl.Tests.SupervisedTests
{
    [TestFixture, Category("Supervised")]
    public class KNNTests : BaseSupervised
    {
        [Test]
        public void Tennis_Tests()
        {
            TennisPrediction(new KNNGenerator());
        }

        [Test]
        public void House_Tests()
        {
            HousePrediction(new KNNGenerator());
        }

        [Test]
        public void Iris_Tests()
        {
            IrisPrediction(new KNNGenerator());
        }

        [Test]
        public void Tennis_Learner_Tests()
        {
            TennisLearnerPrediction(new KNNGenerator());
        }

        [Test]
        public void House_Learner_Tests()
        {
            HouseLearnerPrediction(new KNNGenerator());
        }

        [Test]
        public void Iris_Learner_Tests()
        {
            IrisLearnerPrediction(new KNNGenerator());
        }
    }
}
