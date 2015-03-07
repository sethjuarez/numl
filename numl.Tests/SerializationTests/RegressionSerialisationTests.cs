using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using numl.Supervised;
using numl.Supervised.Regression;
using numl.Model;

namespace numl.Tests.SerializationTests
{
    [TestFixture, Category("Serialization")]
    public class RegressionSerialisationTests : BaseSerialization
    {
        [Test]
        public void Linear_Regression_Save_And_Load()
        {
            var rnd = new Random();
            Func<double, double, double> func = (l, r) => l + 2 * r;

            LinearRegressionModel model;

            var data = new List<ModelItem>();
            for (var i = 0; i < 100; i++)
            {
                var left = rnd.NextDouble(0, 50000);
                var right = rnd.NextDouble(0, 50000);
                var result = func(left, right);
                data.Add(new ModelItem { LeftOperand = left, RightOperand = right, Result = result });
            }
            var d = Descriptor.Create<ModelItem>();
            var g = new LinearRegressionGenerator { Descriptor = d };
            var learningModel = Learner.Learn(data, .80, 1000, g);
            model = (LinearRegressionModel)learningModel.Model;

            Serialize(model);

            var loadedModel = Deserialize<LinearRegressionModel>();

            Assert.AreEqual(model.Theta, loadedModel.Theta);
        }

        //[Test]
        //public void Logistic_Regression_Save_And_Load()
        //{
            

        //    Serialize(p);

        //    var po = Deserialize<LogisticRegressionModel>();

        //    Assert.AreEqual(p.Name, po.Name);
        //    Assert.AreEqual(p.Type, po.Type);
        //    Assert.AreEqual(p.Discrete, po.Discrete);
        //    Assert.AreEqual(p.Start, po.Start);
        //}


    }

    public class ModelItem
    {
        [Feature]
        public double LeftOperand { get; set; }
        [Feature]
        public double RightOperand { get; set; }
        [Label]
        public double Result { get; set; }
    }

    public static class RandomExtensions
    {
        public static double NextDouble(
            this Random random,
            double minValue,
            double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}
