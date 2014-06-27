using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using numl.Math.Probability;
using numl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace numl.Supervised.NeuralNetwork
{
    [XmlRoot("Edge")]
    public class Edge : IXmlSerializable
    {
        public Edge()
        {
            // random initialization
            // R. D. Reed and R. J. Marks II, "Neural Smithing: 
            // Supervised Learning in Feedforward Artificial 
            // Neural Networks", Mit Press, 1999. pg 57
            // selecting values from range [-a,+a] where 0.1 < a < 2
            Weight = (double)Sampling.GetUniform(1, 20) / 10d;
            if (Sampling.GetUniform() < .5)
                Weight *= -1;
        }

        public Node Source { get; set; }
        internal string SourceId { get; set; }
        public Node Target { get; set; }
        internal string TargetId { get; set; }
        public double Weight { get; set; }

        public static Edge Create(Node source, Node target)
        {
            Edge e = new Edge { Source = source, Target = target };
            source.Out.Add(e);
            target.In.Add(e);
            return e;
        }

        public override string ToString()
        {
            return string.Format("{0} ---- {1} ----> {2}", Source, Weight, Target);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            SourceId = reader.GetAttribute("Source");
            TargetId = reader.GetAttribute("Target");
            Weight = double.Parse(reader.GetAttribute("Weight"));

        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Source", Source.Id);
            writer.WriteAttributeString("Target", Target.Id);
            writer.WriteAttributeString("Weight", Weight.ToString("r"));
        }
    }
}
