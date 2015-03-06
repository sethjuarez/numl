// file:	Supervised\DecisionTree\Node.cs
//
// summary:	Implements the node class
using System;
using numl.Math;
using System.Xml.Serialization;

namespace numl.Supervised.DecisionTree
{
    /// <summary>A node.</summary>
    [XmlRoot("Node")]
    public class Node
    {
        /// <summary>Default constructor.</summary>
        public Node() {  }
        /// <summary>if is a leaf.</summary>
        /// <value>true if this object is leaf, false if not.</value>
        [XmlAttribute("Leaf")]
        public bool IsLeaf { get; set; }
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        [XmlAttribute("Value")]
        public double Value { get; set; }
        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [XmlIgnore]
        public object Label { get; set; }
        /// <summary>Gets or sets the column.</summary>
        /// <value>The column.</value>
        [XmlAttribute("Column")]
        public int Column { get; set; }
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        [XmlAttribute("Name")]
        public string Name { get; set; }
        /// <summary>Gets or sets the gain.</summary>
        /// <value>The gain.</value>
        [XmlAttribute("Gain")]
        public double Gain { get; set; }
        /// <summary>Gets or sets the edges.</summary>
        /// <value>The edges.</value>
        [XmlArray("Edges")]
        public Edge[] Edges { get; set; }
    }

    /// <summary>An edge.</summary>
    [XmlRoot("Edge")]
    public class Edge
    {
        /// <summary>Default constructor.</summary>
        public Edge() { }
        /// <summary>Gets or sets the minimum.</summary>
        /// <value>The minimum value.</value>
        [XmlAttribute("Min")]
        public double Min { get; set; }
        /// <summary>Gets or sets the maximum.</summary>
        /// <value>The maximum value.</value>
        [XmlAttribute("Max")]
        public double Max { get; set; }
        /// <summary>Gets or sets a value indicating whether the discrete.</summary>
        /// <value>true if discrete, false if not.</value>
        [XmlAttribute("Discrete")]
        public bool Discrete { get; set; }
        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [XmlAttribute("Label")]
        public string Label { get; set; }
        /// <summary>Gets or sets the parent.</summary>
        /// <value>The parent.</value>
        [XmlIgnore]
        public Node Parent { get; set; }
        /// <summary>Gets or sets the child.</summary>
        /// <value>The child.</value>
        [XmlElement("Child")]
        public Node Child { get; set; }
    }
}
