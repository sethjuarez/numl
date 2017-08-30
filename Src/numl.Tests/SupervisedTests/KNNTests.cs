using System;
using numl.Model;
using System.Linq;
using Xunit;
using numl.Supervised.KNN;
using System.Collections.Generic;
using numl.Tests.Data;

namespace numl.Tests.SupervisedTests
{
    [Trait("Category", "Supervised")]
    public class KNNTests : BaseSupervised
    {
        [Fact]
        public void Tennis_Tests()
        {
            TennisPrediction(new KNNGenerator());
        }

        [Fact]
        public void House_Tests()
        {
            // need to run multiple times since
            // this model is a bit more sensitive
            LearnerPrediction<House>(
                new KNNGenerator(),
                House.GetData(),
                new House
                {
                    District = District.Rural,
                    HouseType = HouseType.Detached,
                    Income = Income.High,
                    PreviousCustomer = false
                },
                p => p.Response
            );
        }

        [Fact]
        public void Iris_Tests()
        {
            IrisPrediction(new KNNGenerator());
        }

        [Fact]
        public void Tennis_Learner_Tests()
        {
            TennisLearnerPrediction(new KNNGenerator());
        }
        
        [Fact]
        public void Iris_Learner_Tests()
        {
            IrisLearnerPrediction(new KNNGenerator());
        }
    }
}
