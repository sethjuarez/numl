using System;
using numl.Data;
using numl.Model;
using numl.Utils;
using System.Linq;
using numl.Supervised;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Tests.Data;

namespace numl.Tests
{
    [TestFixture]
    public class DecisionTreeTests
    {
        [Test]
        public void House_DT_and_Prediction()
        {
            var data = House.GetData();

            var description = Descriptor.Create<House>();
            var generator = new DecisionTreeGenerator { Depth = 50 };
            var model = generator.Generate(description, data);

            House h = new House
            {
                District = District.Rural,
                HouseType = HouseType.Detached,
                Income = Income.High,
                PreviousCustomer = false
            };

            var prediction = model.Predict(h);
            Assert.IsTrue(prediction.Response);
        }

        [Test]
        public void Tennis_DT_and_Prediction()
        {
            var data = Tennis.GetData();
            var description = Descriptor.Create<Tennis>();
            var generator = new DecisionTreeGenerator(50);
            var model = generator.Generate(description, data);
        }

        [Test]
        public void Iris_DT_and_Prediction()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();
            var generator = new DecisionTreeGenerator(50);
            var model = generator.Generate(description, data);

            // should be Iris-Setosa
            Iris iris = new Iris
            {
                PetalWidth = 0.5m,
                PetalLength = 2.3m,
                SepalLength = 2.1m,
                SepalWidth = 2.1m
            };

            iris = model.Predict<Iris>(iris);
            Assert.AreEqual("Iris-setosa".Sanitize(), iris.Class);
        }

        [Test]
        public void Iris_DT_and_Prediction_with_Learner()
        {
            var data = Iris.Load();

            var generator = new DecisionTreeGenerator(50) 
            { 
                Descriptor = Descriptor.Create<Iris>(),
                Hint = 0
            };

            var lmodel = Learner.Learn(data, .80, 1000, generator);

            // should be Iris-Setosa
            Iris iris = new Iris
            {
                PetalWidth = 0.5m,
                PetalLength = 2.3m,
                SepalLength = 2.1m,
                SepalWidth = 2.1m
            };

            iris = lmodel.Model.Predict<Iris>(iris);
            Assert.AreEqual("Iris-setosa".Sanitize(), iris.Class);
        }

        [Test]
        public void ValueObject_Test_With_Yield_Enumerator()
        {
            var data = ValueObject.GetData();
            var generator = new DecisionTreeGenerator()
            {
                Descriptor = Descriptor.Create<ValueObject>()
            };

            var decisionTree = new DecisionTreeGenerator();
            var model = generator.Generate(data);

            var o = new ValueObject() { V1 = 1, V2 = 406 };
            var os = model.Predict<ValueObject>(o).R;
            Assert.AreEqual("l".Sanitize(), os);
        }
    }
}
