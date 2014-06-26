using System;
using System.Xml;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Model;
using numl.Utils;

namespace numl.Supervised.Perceptron
{
    public class PerceptronModel : Model
    {
        public Vector W { get; set; }
        public double B { get; set; }
        public bool Normalized { get; set; }

        public override double Predict(Vector y)
        {
            if (Normalized)
                y = y / y.Norm();

            return W.Dot(y) + B;
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("B", B.ToString("r"));
            writer.WriteAttributeString("Normalized", Normalized.ToString());

            Xml.Write<Descriptor>(writer, Descriptor);
            Xml.Write<Vector>(writer, W);
        }

        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            B = double.Parse(reader.GetAttribute("B"));
            Normalized = bool.Parse(reader.GetAttribute("Normalized"));
            reader.ReadStartElement();

            Descriptor = Xml.Read<Descriptor>(reader);
            W = Xml.Read<Vector>(reader);
        }
    }
}
