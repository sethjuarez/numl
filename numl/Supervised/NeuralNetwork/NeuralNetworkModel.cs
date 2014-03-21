using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using System.Xml;

namespace numl.Supervised.NeuralNetwork
{
    public class NeuralNetworkModel : Model
    {
        public Network Network { get; set; }
        public override double Predict(Vector y)
        {
            Network.Forward(y);
            return Network.Out[0].Output;
        }

        public override void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
