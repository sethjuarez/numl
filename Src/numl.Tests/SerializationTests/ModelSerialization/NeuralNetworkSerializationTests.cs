using System;
using System.Linq;
using numl.Tests.Data;
using NUnit.Framework;
using numl.Tests.SupervisedTests;
using System.Collections.Generic;
using numl.Supervised.NeuralNetwork;

namespace numl.Tests.SerializationTests.ModelSerialization
{
    [TestFixture, Category("Serialization")]
    public class NeuralNetworkSerializationTests : BaseSerialization
    {
        public static void AreEqual(Network n1, Network n2)
        {
            // recursive test
            Assert.AreEqual(n1.In.Length, n2.In.Length);
            Assert.AreEqual(n1.Out.Length, n2.Out.Length);
            for (int i = 0; i < n1.In.Length; i++)
                AreEqual(n1.In[i], n2.In[i]);

            for (int i = 0; i < n1.Out.Length; i++)
                AreEqual(n1.Out[i], n2.Out[i]);
        }

        public static void AreEqual(Neuron n1, Neuron n2)
        {
            if (n1 != null && n2 != null)
            {
                Assert.AreEqual(n1.Input, n2.Input);
                Assert.AreEqual(n1.Output, n2.Output);
                Assert.AreEqual(n1.Label, n2.Label);
                Assert.AreEqual(n1.Id, n2.Id);
                Assert.AreEqual(n1.In.Count, n2.In.Count);
                Assert.AreEqual(n1.Out.Count, n2.Out.Count);

                for (int i = 0; i < n1.Out.Count; i++)
                    AreEqual(n1.Out[i], n2.Out[i]);
            }
            else
                Assert.Fail("Nodes do not match");
        }

        public static void AreEqual(Edge e1, Edge e2)
        {
            Assert.AreEqual(e1.Weight, e2.Weight);
            AreEqual(e1.Target, e2.Target);
        }

        [Test]
        public void Save_Model_Test()
        {

            Tennis t = new Tennis
            {
                Humidity = Humidity.Normal,
                Outlook = Outlook.Overcast,
                Temperature = Temperature.Cool,
                Windy = true
            };

            var model = (NeuralNetworkModel)BaseSupervised.Prediction<Tennis>(
                new NeuralNetworkGenerator(),
                Tennis.GetData(),
                t,
                p => p.Play
            );

            Serialize(model);
            var model2 = Deserialize<NeuralNetworkModel>();

            Assert.AreEqual(model.Descriptor, model2.Descriptor);
            AreEqual(model.Network, model2.Network);
        }

        [Test]
        public void Save_Network_Test()
        {
            Tennis t = new Tennis
            {
                Humidity = Humidity.Normal,
                Outlook = Outlook.Overcast,
                Temperature = Temperature.Cool,
                Windy = true
            };

            var model = (NeuralNetworkModel)BaseSupervised.Prediction<Tennis>(
                new NeuralNetworkGenerator(),
                Tennis.GetData(),
                t,
                p => p.Play
            );

            Serialize(model.Network);
            var network = Deserialize<Network>();
            AreEqual(model.Network, network);
        }
    }
}
