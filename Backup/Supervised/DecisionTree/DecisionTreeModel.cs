// file:	Supervised\DecisionTree\DecisionTreeModel.cs
//
// summary:	Implements the decision tree model class
using System;
using numl.Model;
using System.Xml;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using numl.Utils;

namespace numl.Supervised.DecisionTree
{
    /// <summary>A data Model for the decision tree.</summary>
    public class DecisionTreeModel : Model
    {
        /// <summary>Gets or sets the tree.</summary>
        /// <value>The tree.</value>
        public Node Tree { get; set; }
        /// <summary>Gets or sets the hint.</summary>
        /// <value>The hint.</value>
        public double Hint { get; set; }

        /// <summary>Default constructor.</summary>
        public DecisionTreeModel()
        {
            // no hint
            Hint = double.Epsilon;
        }
        /// <summary>Predicts the given y coordinate.</summary>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        public override double Predict(Vector y)
        {
            return WalkNode(y, Tree);
        }
        /// <summary>Walk node.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="v">The Vector to process.</param>
        /// <param name="node">The node.</param>
        /// <returns>A double.</returns>
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
        /// <summary>Loads the given stream.</summary>
        /// <param name="stream">The stream to load.</param>
        /// <returns>An IModel.</returns>
        public override IModel Load(System.IO.Stream stream)
        {
            var model = base.Load(stream) as DecisionTreeModel;


            return model;
        }
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return PrintNode(Tree, "\t");
        }
        /// <summary>Print node.</summary>
        /// <param name="n">The Node to process.</param>
        /// <param name="pre">The pre.</param>
        /// <returns>A string.</returns>
        private string PrintNode(Node n, string pre)
        {
            if (n.IsLeaf)
                return String.Format("{0} +({1}, {2})\n", pre, n.Label, n.Value);
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
        /// <summary>Re link nodes.</summary>
        /// <param name="n">The Node to process.</param>
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
        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Hint = double.Parse(reader.GetAttribute("Hint"));
            reader.ReadStartElement();

            Descriptor = Xml.Read<Descriptor>(reader);
            Tree = Xml.Read<Node>(reader);

            // re-establish tree cycles and values
            ReLinkNodes(Tree);
        }
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Hint", Hint.ToString("r"));
            Xml.Write<Descriptor>(writer, Descriptor);
            Xml.Write<Node>(writer, Tree);
        }
    }
}
