using numl.Data;
using numl.Model;
using numl.Supervised;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
