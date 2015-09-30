// file:	Supervised\DecisionTree\Node.cs
//
// summary:	Implements the node class
using System;
using numl.Math;
using System.Xml.Serialization;

namespace numl.Supervised.DecisionTree
{
    /// <summary>A node.</summary>
    public class Node
    {
        /// <summary>Default constructor.</summary>
        public Node() {  }
        /// <summary>if is a leaf.</summary>
        /// <value>true if this object is leaf, false if not.</value>
        public bool IsLeaf { get; set; }
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public double Value { get; set; }
        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        public object Label { get; set; }
        /// <summary>Gets or sets the column.</summary>
        /// <value>The column.</value>
        public int Column { get; set; }
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>Gets or sets the gain.</summary>
        /// <value>The gain.</value>
        public double Gain { get; set; }
        /// <summary>Gets or sets the edges.</summary>
        /// <value>The edges.</value>
        public Edge[] Edges { get; set; }
    }

    /// <summary>An edge.</summary>
    public class Edge
    {
        /// <summary>Default constructor.</summary>
        public Edge() { }
        /// <summary>Gets or sets the minimum.</summary>
        /// <value>The minimum value.</value>
        public double Min { get; set; }
        /// <summary>Gets or sets the maximum.</summary>
        /// <value>The maximum value.</value>
        public double Max { get; set; }
        /// <summary>Gets or sets a value indicating whether the discrete.</summary>
        /// <value>true if discrete, false if not.</value>
        public bool Discrete { get; set; }
        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        public string Label { get; set; }
        /// <summary>Gets or sets the parent.</summary>
        /// <value>The parent.</value>
        public Node Parent { get; set; }
        /// <summary>Gets or sets the child.</summary>
        /// <value>The child.</value>
        public Node Child { get; set; }
    }
}
