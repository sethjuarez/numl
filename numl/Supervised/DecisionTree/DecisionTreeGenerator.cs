// file:	Supervised\DecisionTree\DecisionTreeGenerator.cs
//
// summary:	Implements the decision tree generator class
using System;
using numl.Model;
using System.Linq;
using numl.Math.Information;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Supervised.DecisionTree
{
    /// <summary>A decision tree generator.</summary>
    public class DecisionTreeGenerator : Generator
    {
        /// <summary>Gets or sets the depth.</summary>
        /// <value>The depth.</value>
        public int Depth { get; set; }
        /// <summary>Gets or sets the width.</summary>
        /// <value>The width.</value>
        public int Width { get; set; }
        /// <summary>Gets or sets the hint.</summary>
        /// <value>The hint.</value>
        public double Hint { get; set; }
        /// <summary>Gets or sets the type of the impurity.</summary>
        /// <value>The type of the impurity.</value>
        public Type ImpurityType { get; set; }

        /// <summary>The impurity.</summary>
        private Impurity _impurity;
        /// <summary>Gets the impurity.</summary>
        /// <value>The impurity.</value>
        public Impurity Impurity
        {
            get
            {
                if (_impurity == null)
                    _impurity = (Impurity)Activator.CreateInstance(ImpurityType);
                return _impurity;
            }
        }
        /// <summary>Constructor.</summary>
        /// <param name="descriptor">the descriptor.</param>
        public DecisionTreeGenerator(Descriptor descriptor)
        {
            Depth = 5;
            Width = 2;
            Descriptor = descriptor;
            ImpurityType = typeof(Entropy);
            Hint = double.Epsilon;
        }
        /// <summary>Constructor.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="depth">(Optional) The depth.</param>
        /// <param name="width">(Optional) the width.</param>
        /// <param name="descriptor">(Optional) the descriptor.</param>
        /// <param name="impurityType">(Optional) type of the impurity.</param>
        /// <param name="hint">(Optional) the hint.</param>
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
        /// <summary>Sets a hint.</summary>
        /// <param name="o">The object to process.</param>
        public void SetHint(object o)
        {
            Hint = Descriptor.Label.Convert(o).First();
        }
        /// <summary>Generates.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An IModel.</returns>
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
        /// <summary>Builds a tree.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="used">The used.</param>
        /// <returns>A Node.</returns>
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
        /// <summary>Gets best split.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <param name="used">The used.</param>
        /// <returns>The best split.</returns>
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
        /// <summary>Builds leaf node.</summary>
        /// <param name="val">The value.</param>
        /// <returns>A Node.</returns>
        private Node BuildLeafNode(double val)
        {
            // build leaf node
            return new Node { IsLeaf = true, Value = val, Edges = null, Label = Descriptor.Label.Convert(val) };
        }
    }
}
