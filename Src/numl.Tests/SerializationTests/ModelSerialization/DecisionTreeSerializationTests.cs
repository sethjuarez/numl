using System;
using numl.Model;
using System.Linq;
using numl.Tests.Data;
using Xunit;
using System.Collections.Generic;
using numl.Supervised.DecisionTree;

namespace numl.Tests.SerializationTests.ModelSerialization
{
    [Trait("Category", "Serialization")]
    public class DecisionTreeSerializationTests : BaseSerialization
    {
        [Fact]
        public void Save_And_Load_HouseDT()
        {
            var data = House.GetData();

            var description = Descriptor.Create<House>();
            var generator = new DecisionTreeGenerator { Depth = 50 };
            var model = generator.Generate(description, data) as DecisionTreeModel;

            Serialize(model);
            var lmodel = Deserialize<DecisionTreeModel>();

            Assert.Equal(model.Descriptor, lmodel.Descriptor);
            Assert.Equal(model.Hint, lmodel.Hint);
            Assert.Equal(model.Tree, lmodel.Tree);
        }

        [Fact]
        public void Save_And_Load_Iris_DT()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();
            var generator = new DecisionTreeGenerator(50);
            var model = generator.Generate(description, data) as DecisionTreeModel;

            Serialize(model);
            var lmodel = Deserialize<DecisionTreeModel>();
            Assert.Equal(model.Descriptor, lmodel.Descriptor);
            Assert.Equal(model.Hint, lmodel.Hint);
            Assert.Equal(model.Tree, lmodel.Tree);
        }
    }
}
