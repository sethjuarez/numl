using System;
using numl.Model;
using System.Linq;
using numl.Math.Information;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.DecisionTree
{
    public class DecisionTreeGenerator : Generator
    {
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
        public DecisionTreeGenerator(Descriptor descriptor)
        {
            Depth = 5;
            Width = 2;
            Descriptor = descriptor;
            ImpurityType = typeof(Entropy);
            Hint = double.Epsilon;
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

            Descriptor = descriptor;
            Depth = depth;
            Width = width;
            ImpurityType = impurityType ?? typeof(Entropy);
            Hint = hint;
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
}
