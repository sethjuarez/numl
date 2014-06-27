using System;
using System.Linq;
using numl.Math.Functions;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml;
using numl.Utils;

namespace numl.Supervised.NeuralNetwork
{
    [XmlRoot("Node"), Serializable]
    public class Node : IXmlSerializable
    {
        public Node()
        {
            // assume bias node unless
            // otherwise told through
            // links
            Output = 1d;
            Input = 1d;
            Delta = 0d;
            Label = String.Empty;
            Out = new List<Edge>();
            In = new List<Edge>();
            Id = Guid.NewGuid().ToString();
        }

        public double Output { get; set; }
        public double Input { get; set; }
        public double Delta { get; set; }
        public string Label { get; set; }
        public string Id { get; private set; }
        public List<Edge> Out { get; set; }
        public List<Edge> In { get; set; }
        public IFunction Activation { get; set; }

        public double Evaluate()
        {
            if (In.Count > 0)
            {
                Input = In.Select(e => e.Weight * e.Source.Evaluate()).Sum();
                Output = Activation.Compute(Input);
            }

            return Output;
        }

        public double Error(double t)
        {
            // output node
            if (Out.Count == 0)
                Delta = Output - t;
            else // internal nodes
            {
                var hp = Activation.Derivative(Input);
                Delta = hp * Out.Select(e => e.Weight * e.Target.Error(t)).Sum();
            }

            return Delta;
        }

        public void Update(double learningRate)
        {
            foreach (Edge edge in In)
            {
                // for output nodes, the derivative is the Delta
                edge.Weight = learningRate * Delta * edge.Source.Output;
                edge.Source.Update(learningRate);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1} | {2})", Label, Input, Output);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Id = reader.GetAttribute("Id");
            Label = reader.GetAttribute("Label");
            Input = double.Parse(reader.GetAttribute("Input"));
            Output = double.Parse(reader.GetAttribute("Output"));
            Delta = double.Parse(reader.GetAttribute("Delta"));

            var activation = Ject.FindType(reader.GetAttribute("Activation"));
            Activation = (IFunction)Activator.CreateInstance(activation);
            In = new List<Edge>();
            Out = new List<Edge>();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Id", Id);
            writer.WriteAttributeString("Label", Label);
            writer.WriteAttributeString("Input", Input.ToString("r"));
            writer.WriteAttributeString("Output", Output.ToString("r"));
            writer.WriteAttributeString("Delta", Delta.ToString("r"));
            writer.WriteAttributeString("Activation", Activation.GetType().Name);
        }
    }
}