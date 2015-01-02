using System;
using numl.Model;
using System.Linq;
using numl.Tests.Data;
using NUnit.Framework;
using System.Collections.Generic;
using numl.Supervised.DecisionTree;

namespace numl.Tests.SerializationTests
{
    [TestFixture, Category("Serialization")]
    public class DecisionTreeSerializationTests : BaseSerialization
    {
        private static void AreEqual(Node n1, Node n2, bool ignoreParent)
        {
            Assert.AreEqual(n1.IsLeaf, n2.IsLeaf);
            Assert.AreEqual(n1.Value, n2.Value);
            Assert.AreEqual(n1.Label, n2.Label);
            Assert.AreEqual(n1.Column, n2.Column);
            Assert.AreEqual(n1.Name, n2.Name);
            if (n1.Edges == null) Assert.IsNull(n2.Edges);
            else
            {
                Assert.AreEqual(n1.Edges.Length, n2.Edges.Length);

                // since we are not ignoring parent,
                // no need to check edges since these
                // are checked when ignoring parents
                if (ignoreParent) return;
                for (int i = 0; i < n1.Edges.Length; i++)
                {
                    var n1e = n1.Edges[i];
                    var n2e = n2.Edges[i];
                    Assert.AreEqual(n1e.Min, n2e.Min);
                    Assert.AreEqual(n1e.Max, n2e.Max);
                    Assert.AreEqual(n1e.Discrete, n2e.Discrete);
                    Assert.AreEqual(n1e.Label, n2e.Label);

                    if (!ignoreParent)
                        AreEqual(n1e.Parent, n2e.Parent, true);

                    AreEqual(n1e.Child, n2e.Child, true);
                }
            }
        }

        [Test]
        public void Save_And_Load_HouseDT()
        {
            var data = House.GetData();

            var description = Descriptor.Create<House>();
            var generator = new DecisionTreeGenerator { Depth = 50 };
            var model = generator.Generate(description, data) as DecisionTreeModel;

            Serialize(model);

            var lmodel = Deserialize<DecisionTreeModel>();
            Assert.AreEqual(model.Hint, lmodel.Hint);
            AreEqual(model.Tree, lmodel.Tree, false);
        }

        [Test]
        public void Save_And_Load_Iris_DT()
        {
            var data = Iris.Load();
            var description = Descriptor.Create<Iris>();
            var generator = new DecisionTreeGenerator(50);
            var model = generator.Generate(description, data) as DecisionTreeModel;

            Serialize(model);
            var lmodel = Deserialize<DecisionTreeModel>();
            Assert.AreEqual(model.Hint, lmodel.Hint);
            AreEqual(model.Tree, lmodel.Tree, false);
        }
    }
}
