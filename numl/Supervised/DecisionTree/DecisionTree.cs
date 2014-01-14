﻿using System;
using numl.Math;
using numl.Model;
using System.Linq;
using numl.Math.Information;
using numl.Math.LinearAlgebra;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using System.Xml;

namespace numl.Supervised.DecisionTree
{
    public class DecisionTreeGenerator : Generator
    {
        public int Depth { get; set; }
        public int Width { get; set; }
        public double Hint { get; set; }
        public bool AllowColumnReuse { get; set; }
        public Type ImpurityType { get; set; }

        private Impurity _impurity;
        public Impurity Impurity
        {
            get
            {
                if (_impurity == null)
                    _impurity = (Impurity)Activator.CreateInstance(ImpurityType);
                return _impurity;
            }
        }
        public DecisionTreeGenerator(Descriptor descriptor)
        {
            Depth = 5;
            Width = 2;
            Descriptor = descriptor;
            ImpurityType = typeof(Entropy);
            Hint = double.Epsilon;
            AllowColumnReuse = true;
        }

        public DecisionTreeGenerator(
            int depth = 5,
            int width = 2,
            Descriptor descriptor = null,
            Type impurityType = null,
            double hint = double.Epsilon,
            bool allowColumnReuse = true)
        {
            if (width < 2)
                throw new InvalidOperationException("Cannot set dt tree width to less than 2!");

            Descriptor = descriptor;
            Depth = depth;
            Width = width;
            ImpurityType = impurityType ?? typeof(Entropy);
            Hint = hint;
            AllowColumnReuse = allowColumnReuse;
        }

        public void SetHint(object o)
        {
            Hint = Descriptor.Label.Convert(o).First();
        }

        public override IModel Generate(Matrix x, Vector y)
        {
            if (Descriptor == null)
                throw new InvalidOperationException("Cannot build decision tree without type knowledge!");

            var n = BuildTree(x, y, Depth, new List<int>(x.Cols));

            return new DecisionTreeModel
            {
                Descriptor = Descriptor,
                Tree = n,
                Hint = Hint
            };
        }

        private Node BuildTree(Matrix x, Vector y, int depth, List<int> used)
        {
            if (depth < 0)
                return BuildLeafNode(y.Mode());

            var tuple = GetBestSplit(x, y, used);
            var col = tuple.Item1;
            var gain = tuple.Item2;
            var measure = tuple.Item3;

            // uh oh, need to return something?
            // a weird node of some sort...
            // but just in case...
            if (col == -1)
                return BuildLeafNode(y.Mode());

            used.Add(col);

            Node node = new Node
            {
                Column = col,
                Gain = gain,
                IsLeaf = false,
                Name = Descriptor.ColumnAt(col)
            };

            // populate edges
            List<Edge> edges = new List<Edge>(measure.Segments.Length);
            for (int i = 0; i < measure.Segments.Length; i++)
            {
                // working set
                var segment = measure.Segments[i];
                var edge = new Edge()
                {
                    Parent = node,
                    Discrete = measure.Discrete,
                    Min = segment.Min,
                    Max = segment.Max
                };

                IEnumerable<int> slice;

                if (edge.Discrete)
                {
                    // get discrete label
                    edge.Label = Descriptor.At(col).Convert(segment.Min).ToString();
                    // do value check for matrix slicing
                    slice = x.Indices(v => v[col] == segment.Min);
                }
                else
                {
                    // get range label
                    edge.Label = string.Format("{0} ≤ x < {1}", segment.Min, segment.Max);
                    // do range check for matrix slicing
                    slice = x.Indices(v => v[col] >= segment.Min && v[col] < segment.Max);
                }

                // something to look at?
                // if this number is 0 then this edge 
                // leads to a dead end - the edge will 
                // not be built
                if (slice.Count() > 0)
                {
                    Vector ySlice = y.Slice(slice);
                    // only one answer, set leaf
                    if (ySlice.Distinct().Count() == 1)
                        edge.Child = BuildLeafNode(ySlice[0]);
                    // otherwise continue to build tree
                    else
                        edge.Child = BuildTree(x.Slice(slice), ySlice, depth - 1, used);

                    edges.Add(edge);
                }
            }

            // might check if there are no edges
            // if this is the case should convert
            // node to leaf and bail
            var egs = edges.ToArray();
            // problem, need to convert
            // parent to terminal node
            // with mode
            if (egs.Length <= 1)
                node = BuildLeafNode(y.Mode());
            else
                node.Edges = egs;

            //Returning this node and moving back up the tree
            //It possible to reused this column in other branches if desired.
            if(AllowColumnReuse)
                used.Remove(col);
            return node;
        }

        private Tuple<int, double, Impurity> GetBestSplit(Matrix x, Vector y, List<int> used)
        {
            double bestGain = -1;
            int bestFeature = -1;

            Impurity bestMeasure = null;
            for (int i = 0; i < x.Cols; i++)
            {
                // already used?
                if (used.Contains(i)) continue;

                double gain = 0;
                Impurity measure = (Impurity)Activator.CreateInstance(ImpurityType);
                // get appropriate column vector
                var feature = x.Col(i);
                // get appropriate feature at index i
                // (important on because of multivalued
                // cols)
                var property = Descriptor.At(i);
                // if discrete, calculate full relative gain
                if (property.Discrete)
                    gain = measure.RelativeGain(y, feature);
                // otherwise segment based on width
                else
                    gain = measure.SegmentedRelativeGain(y, feature, Width);

                // best one?
                if (gain > bestGain)
                {
                    bestGain = gain;
                    bestFeature = i;
                    bestMeasure = measure;
                }
            }

            return new Tuple<int, double, Impurity>(bestFeature, bestGain, bestMeasure);
        }

        private Node BuildLeafNode(double val)
        {
            // build leaf node
            return new Node { IsLeaf = true, Value = val, Edges = null, Label = Descriptor.Label.Convert(val) };
        }
    }

    [Serializable]
    public class DecisionTreeModel : Model, IXmlSerializable
    {
        [XmlElement]
        public Node Tree { get; set; }
        [XmlAttribute]
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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Hint = double.Parse(reader.GetAttribute("Hint"));
            reader.ReadStartElement();

            XmlSerializer dserializer = new XmlSerializer(typeof(Descriptor));
            Descriptor = (Descriptor)dserializer.Deserialize(reader);
            reader.Read();

            XmlSerializer serializer = new XmlSerializer(typeof(Node));
            Tree = (Node)serializer.Deserialize(reader);
            // re-establish tree cycles and values
            ReLinkNodes(Tree);
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

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Hint", Hint.ToString("r"));
            XmlSerializer dserializer = new XmlSerializer(typeof(Descriptor));
            dserializer.Serialize(writer, Descriptor);

            XmlSerializer serializer = new XmlSerializer(Tree.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(writer, Tree, ns);
        }
    }


   

}
