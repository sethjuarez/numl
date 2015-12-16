// file:	Supervised\DecisionTree\Node.cs
//
// summary:	Implements the node class
using System;
using numl.Math;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace numl.Supervised.DecisionTree
{
    /// <summary>A node.</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Node
    {
        /// <summary>Default constructor.</summary>
        public Node() { }
        /// <summary>if is a leaf.</summary>
        /// <value>true if this object is leaf, false if not.</value>
        [JsonProperty]
        public bool IsLeaf { get; set; }
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        [JsonProperty]
        public double Value { get; set; }
        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [JsonProperty]
        public object Label { get; set; }
        /// <summary>Gets or sets the column.</summary>
        /// <value>The column.</value>
        [JsonProperty]
        public int Column { get; set; }
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        [JsonProperty]
        public string Name { get; set; }
        /// <summary>Gets or sets the gain.</summary>
        /// <value>The gain.</value>
        [JsonProperty]
        public double Gain { get; set; }
        /// <summary>Gets or sets the edges.</summary>
        /// <value>The edges.</value>
        [JsonProperty]
        public Edge[] Edges { get; set; }

        //[OnDeserialized]
        //internal void OnDeserializedMethod(StreamingContext context)
        //{
        //    if (Edges != null)
        //        foreach (var e in Edges)
        //            e.Parent = this;
        //}
    }

    /// <summary>An edge.</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Edge
    {
        /// <summary>Default constructor.</summary>
        public Edge() { }
        /// <summary>Gets or sets the minimum.</summary>
        /// <value>The minimum value.</value>
        [JsonProperty]
        public double Min { get; set; }
        /// <summary>Gets or sets the maximum.</summary>
        /// <value>The maximum value.</value>
        [JsonProperty]
        public double Max { get; set; }
        /// <summary>Gets or sets a value indicating whether the discrete.</summary>
        /// <value>true if discrete, false if not.</value>
        [JsonProperty]
        public bool Discrete { get; set; }
        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [JsonProperty]
        public string Label { get; set; }
        /// <summary>Gets or sets the parent.</summary>
        /// <value>The parent.</value>
        public Node Parent { get; set; }
        /// <summary>Gets or sets the child.</summary>
        /// <value>The child.</value>
        [JsonProperty]
        public Node Child { get; set; }
    }
}
