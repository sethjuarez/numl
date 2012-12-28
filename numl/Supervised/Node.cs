using System;
using numl.Math;
using System.Xml.Serialization;

namespace numl.Supervised
{
    public class Node
    {
        // if is a leaf
        public bool IsLeaf { get; set; }
        public double Value { get; set; }
        public object Label { get; set; }

        public int Column { get; set; }
        public string Name { get; set; }
        public double Gain { get; set; }

        public Edge[] Edges { get; set; }
    }

    public class Edge
    {
        
        public double Min { get; set; }
        public double Max { get; set; }
        public bool Discrete { get; set; }

        public string Label { get; set; }
        
        public Node Parent { get; set; }
        public Node Child { get; set; }
    }
}
