using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using numl.Data;
using numl.Model;
using numl.Supervised;

namespace numl.Tests
{
    [TestFixture]
    public class ModelSerializationTests
    {
        [Test]
        public void Serialize_DT_Test()
        {
            var data = House.GetData();
            var description = (LabeledDescription)Description.Create<House>();
            var generator = new DecisionTreeGenerator { Depth = 50 };
            var model = generator.Generate(description, data);
        }
    }
}
