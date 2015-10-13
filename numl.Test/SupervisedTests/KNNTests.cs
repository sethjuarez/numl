using System;
using numl.Model;
using System.Linq;

using numl.Supervised.KNN;
using System.Collections.Generic;
using numl.Tests.Data;

namespace numl.Tests.SupervisedTests
{
    [TestClass, TestCategory("Supervised")]
    public class KNNTests : BaseSupervised
    {
        [TestMethod]
        public void Tennis_Tests()
        {
            TennisPrediction(new KNNGenerator());
        }

        [TestMethod]
        public void House_Tests()
        {
            HousePrediction(new KNNGenerator());
        }

        [TestMethod]
        public void Iris_Tests()
        {
            IrisPrediction(new KNNGenerator());
        }

        [TestMethod]
        public void Tennis_Learner_Tests()
        {
            TennisLearnerPrediction(new KNNGenerator());
        }

        [TestMethod]
        public void House_Learner_Tests()
        {
            HouseLearnerPrediction(new KNNGenerator());
        }

        [TestMethod]
        public void Iris_Learner_Tests()
        {
            IrisLearnerPrediction(new KNNGenerator());
        }
    }
}
