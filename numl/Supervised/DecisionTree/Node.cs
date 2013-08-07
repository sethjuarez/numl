using System;
using numl.Math;
using System.Xml.Serialization;

namespace numl.Supervised.DecisionTree
{
    [XmlRoot("Node")]
    public class Node
    {
        public Node() {  }

        // if is a leaf
        [XmlAttribute("Leaf")]
        public bool IsLeaf { get; set; }

        [XmlAttribute("Value")]
        public double Value { get; set; }

        [XmlIgnore]
        public object Label { get; set; }

        [XmlAttribute("Column")]
        public int Column { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Gain")]
        public double Gain { get; set; }

        [XmlArray("Edges")]
        public Edge[] Edges { get; set; }
    }

    [XmlRoot("Edge")]
    public class Edge
    {
        public Edge() { }

        [XmlAttribute("Min")]
        public double Min { get; set; }

        [XmlAttribute("Max")]
        public double Max { get; set; }

        [XmlAttribute("Discrete")]
        public bool Discrete { get; set; }

        [XmlAttribute("Label")]
        public string Label { get; set; }

        [XmlIgnore]
        public Node Parent { get; set; }

        [XmlElement("Child")]
        public Node Child { get; set; }
    }
}
