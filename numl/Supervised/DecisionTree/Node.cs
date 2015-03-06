// file:	Supervised\DecisionTree\Node.cs
//
// summary:	Implements the node class
using System;
using numl.Math;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace numl.Supervised.DecisionTree
{
    /// <summary>A node.</summary>
    [DataContract(Name = "Node"), XmlRoot("Node")]
    public class Node
    {
        /// <summary>Default constructor.</summary>
        public Node() {  }
        /// <summary>if is a leaf.</summary>
        /// <value>true if this object is leaf, false if not.</value>
        [DataMember, XmlAttribute("Leaf")]
        public bool IsLeaf { get; set; }
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        [DataMember, XmlAttribute("Value")]
        public double Value { get; set; }
        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [XmlIgnore]
        public object Label { get; set; }
        /// <summary>Gets or sets the column.</summary>
        /// <value>The column.</value>
        [DataMember, XmlAttribute("Column")]
        public int Column { get; set; }
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        [DataMember, XmlAttribute("Name")]
        public string Name { get; set; }
        /// <summary>Gets or sets the gain.</summary>
        /// <value>The gain.</value>
        [DataMember, XmlAttribute("Gain")]
        public double Gain { get; set; }
        /// <summary>Gets or sets the edges.</summary>
        /// <value>The edges.</value>
        [DataMember, XmlArray("Edges")]
        public Edge[] Edges { get; set; }
    }

    /// <summary>An edge.</summary>
    [DataContract(Name = "Edge"), XmlRoot("Edge")]
    public class Edge
    {
        /// <summary>Default constructor.</summary>
        public Edge() { }
        /// <summary>Gets or sets the minimum.</summary>
        /// <value>The minimum value.</value>
        [DataMember, XmlAttribute("Min")]
        public double Min { get; set; }
        /// <summary>Gets or sets the maximum.</summary>
        /// <value>The maximum value.</value>
        [DataMember, XmlAttribute("Max")]
        public double Max { get; set; }
        /// <summary>Gets or sets a value indicating whether the discrete.</summary>
        /// <value>true if discrete, false if not.</value>
        [DataMember, XmlAttribute("Discrete")]
        public bool Discrete { get; set; }
        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [DataMember, XmlAttribute("Label")]
        public string Label { get; set; }
        /// <summary>Gets or sets the parent.</summary>
        /// <value>The parent.</value>
        [XmlIgnore]
        public Node Parent { get; set; }
        /// <summary>Gets or sets the child.</summary>
        /// <value>The child.</value>
        [DataMember, XmlElement("Child")]
        public Node Child { get; set; }
    }
}
