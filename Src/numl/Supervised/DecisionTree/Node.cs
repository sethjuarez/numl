// file:	Supervised\DecisionTree\Node.cs
//
// summary:	Implements the node class
using System;
using numl.Math;
using System.Runtime.Serialization;
using numl.Data;

namespace numl.Supervised.DecisionTree
{
    public class Node : IVertex
    {
        static int _id = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node() { Id = ++_id; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>if is a leaf.</summary>
        /// <value>true if this object is leaf, false if not.</value>
        public bool IsLeaf { get; set; }
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public double Value { get; set; }
        /// <summary>Gets or sets the column.</summary>
        /// <value>The column.</value>
        public int Column { get; set; }
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>Gets or sets the gain.</summary>
        /// <value>The gain.</value>
        public double Gain { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Node)
            {
                return ((Node)obj).Column == Column &&
                       ((Node)obj).Gain == Gain &&
                       ((Node)obj).Id == Id &&
                       ((Node)obj).IsLeaf == IsLeaf &&
                       ((Node)obj).Name == Name &&
                       ((Node)obj).Value == Value;
            }
            else
                return false;
        }
    }

    public class Edge : IEdge
    {
        /// <summary>Gets or sets the child identifier.</summary>
        /// <value>The child identifier.</value>
        public int ChildId { get; set; }
        /// <summary> Gets or sets the parent identifier.</summary>
        /// <value>The parent identifier.</value>
        public int ParentId { get; set; }
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Edge &&
                   ((Edge)obj).ChildId == ChildId &&
                   ((Edge)obj).ParentId == ParentId &&
                   ((Edge)obj).Min == Min &&
                   ((Edge)obj).Max == Max &&
                   ((Edge)obj).Discrete == Discrete &&
                   ((Edge)obj).Label == Label;
        }
    }

}
