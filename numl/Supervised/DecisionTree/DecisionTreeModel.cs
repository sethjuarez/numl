using System;
using numl.Model;
using System.Xml;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace numl.Supervised.DecisionTree
{
    public class DecisionTreeModel : Model
    {
        public Node Tree { get; set; }
        public double Hint { get; set; }

        public DecisionTreeModel()
        {
            // no hint
            Hint = double.Epsilon;
        }

        public override double Predict(Vector y)
        {
            return WalkNode(y, Tree);
        }

        private double WalkNode(Vector v, Node node)
        {
            if (node.IsLeaf)
                return node.Value;

            // Get the index of the feature for this node.
            var col = node.Column;
            if (col == -1)
                throw new InvalidOperationException("Invalid Feature encountered during node walk!");

            for (int i = 0; i < node.Edges.Length; i++)
            {
                Edge edge = node.Edges[i];
                if (edge.Discrete && v[col] == edge.Min)
                    return WalkNode(v, edge.Child);
                if (!edge.Discrete && v[col] >= edge.Min && v[col] < edge.Max)
                    return WalkNode(v, edge.Child);
            }

            if (Hint != double.Epsilon)
                return Hint;
            else
                throw new InvalidOperationException(String.Format("Unable to match split value {0} for feature {1}[2]\nConsider setting a Hint in order to avoid this error.", v[col], Descriptor.At(col), col));
        }

        public override IModel Load(System.IO.Stream stream)
        {
            var model = base.Load(stream) as DecisionTreeModel;


            return model;
        }

        public override string ToString()
        {
            return PrintNode(Tree, "\t");
        }

        private string PrintNode(Node n, string pre)
        {
            if (n.IsLeaf)
                return String.Format("{0} +({1}, {2:#.####})\n", pre, n.Label, n.Value);
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(String.Format("{0}[{1}, {2:0.0000}]", pre, n.Name, n.Gain));
                foreach (Edge edge in n.Edges)
                {
                    sb.AppendLine(String.Format("{0} |- {1}", pre, edge.Label));
                    sb.Append(PrintNode(edge.Child, String.Format("{0} |\t", pre)));
                }

                return sb.ToString();
            }
        }

        private void ReLinkNodes(Node n)
        {
            if (n.Edges != null)
            {
                foreach (Edge e in n.Edges)
                {
                    e.Parent = n;
                    if (e.Child.IsLeaf)
                        e.Child.Label = Descriptor.Label.Convert(e.Child.Value);
                    else
                        ReLinkNodes(e.Child);
                }
            }
        }

        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Hint = double.Parse(reader.GetAttribute("Hint"));
            reader.ReadStartElement();

            Descriptor = ReadXml<Descriptor>(reader);
            Tree = ReadXml<Node>(reader);

            // re-establish tree cycles and values
            ReLinkNodes(Tree);
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Hint", Hint.ToString("r"));
            WriteXml<Descriptor>(writer, Descriptor);
            WriteXml<Node>(writer, Tree);
        }
    }
}
