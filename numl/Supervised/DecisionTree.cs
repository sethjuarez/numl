using System;
using numl.Math;
using numl.Model;
using System.Linq;
using numl.Math.Information;
using System.Xml.Serialization;
using System.Collections.Generic;
using numl.Math.LinearAlgebra;

namespace numl.Supervised
{
    public class DecisionTreeGenerator : Generator
    {
        private Random _random;
        public int Depth { get; set; }
        public int Width { get; set; }
        public double Hint { get; set; }
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

        public DecisionTreeGenerator(
            int depth = 5,
            int width = 2,
            Descriptor descriptor = null,
            Type impurityType = null,
            double hint = double.Epsilon)
        {
            if (width < 2)
                throw new InvalidOperationException("Cannot set dt tree width to less than 2!");

            _random = new Random(DateTime.Now.Millisecond);
            Descriptor = descriptor;
            Depth = depth;
            Width = width;
            ImpurityType = impurityType ?? typeof(Entropy);
            Hint = hint;
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
            // reached depth limit or all labels are the same
            if (depth < 0 || y.Distinct().Count() == 1)
                return new Node { IsLeaf = true, Label = y.Mode() };

            var tuple = GetBestSplit(x, y, depth, used);

            var col = tuple.Item1;
            var gain = tuple.Item2;
            var measure = tuple.Item3;

            // uh oh, need to return something?
            // a weird node of some sort...
            // but just in case...
            if (col == -1)
                return new Node { IsLeaf = true, Label = y.Mode() };

            used.Add(col);
            Node n = new Node()
            {
                Gain = gain,
                Children = new Node[measure.Segments.Length],
                Segmented = measure.Segmented,
                IsLeaf = false,
                Column = col,
                Segments = measure.Segments
            };
            
            // create children
            for (int i = 0; i < n.Children.Length; i++)
            {
                IEnumerable<int> slice;

                // continuous - range check
                if (measure.Segmented)
                    slice = x.Indices(v => v[col] >= n.Segments[i].Min &&
                                           v[col] < n.Segments[i].Max);
                // discrete - value check
                else
                    slice = x.Indices(v => v[col] == n.Segments[i].Min);

                // recursion
                n.Children[i] = BuildTree(x.Slice(slice), y.Slice(slice), depth - 1, used);
            }

            return n;
        }


        private Tuple<int, double, Impurity> GetBestSplit(Matrix x, Vector y, int depth, List<int> used)
        {
            double bestGain = -1;
            int bestFeature = -1;

            Impurity bestMeasure = null;
            for (int i = 0; i < x.Cols; i++)
            {
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
                if (gain > bestGain && !used.Contains(i))
                {
                    bestGain = gain;
                    bestFeature = i;
                    bestMeasure = measure;
                }
            }

            return new Tuple<int, double, Impurity>(bestFeature, bestGain, bestMeasure);
        }
    }

    [XmlRoot("dt")]
    public class DecisionTreeModel : Model
    {
        [XmlElement("tree")]
        public Node Tree { get; set; }
        [XmlAttribute("hint")]
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
                return node.Label;

            // Get the index of the feature for this node.
            var col = node.Column;
            if (col == -1)
                throw new InvalidOperationException("Invalid Feature encountered during node walk!");

            Range[] segments = node.Segments;

            for (int i = 0; i < segments.Length; i++)
            {
                // continuous - range check
                if (node.Segmented && v[col] >= segments[i].Min && v[col] < segments[i].Max)
                    return WalkNode(v, node.Children[i]);
                // discrete - value check
                if (!node.Segmented && v[col] == segments[i].Min)
                    return WalkNode(v, node.Children[i]);
            }

            if (Hint != double.Epsilon)
                return Hint;
            else
                throw new InvalidOperationException(String.Format("Unable to match split value {0} for feature {1}[2]", v[col], Descriptor.At(col), col));

        }
    }
}
