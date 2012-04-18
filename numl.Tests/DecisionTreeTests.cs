using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using numl.Model;
using numl.Data;
using numl.Supervised;

namespace numl.Tests
{
    [TestFixture]
    public class DecisionTreeTests
    {
        [Test]
        public void Test_Basic_DT_Prediction()
        {
            var data = House.GetData();
            var description = (LabeledDescription)Description.Create<House>();
            var generator = new DecisionTreeGenerator { Depth = 50 };
            var model = generator.Generate(description, data);

            House h = new House { District = District.Rural, 
                                    HouseType = HouseType.Detached, 
                                    Income = Income.High, 
                                    PreviousCustomer = false };
            var prediction = model.Predict(h);
            Assert.IsTrue(prediction.Response);
        }
    }
}
