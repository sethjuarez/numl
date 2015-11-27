// file:	Unsupervised\Cluster.cs
//
// summary:	Implements the cluster class
using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Unsupervised
{
    /// <summary>A cluster.</summary>
    public class Cluster
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
        /// <summary>Gets or sets the points.</summary>
        /// <value>The points.</value>
        internal IEnumerable<Vector> Points { get; set; }
        /// <summary>Gets or sets the members.</summary>
        /// <value>The members.</value>
        public object[] Members { get; set; }
        /// <summary>Gets or sets the children.</summary>
        /// <value>The children.</value>
        public Cluster[] Children { get; set; }
        /// <summary>Gets or sets the center.</summary>
        /// <value>The center.</value>
        public Vector Center { get; set; }

        /// <summary>Default constructor.</summary>
        public Cluster()
        {
        }
        /// <summary>Constructor.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        public Cluster(int id, Cluster left, Cluster right )
        {
            Id = id;
            Children = new Cluster[] { left, right };
            Points = left.Points.Concat(right.Points);
            // maybe only need item at leaves
            Members = left.Members.Concat(right.Members).ToArray();
        }
        /// <summary>Constructor.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="children">The children.</param>
        public Cluster(int id, IEnumerable<Cluster> children)
        {
            Id = id;
            Children = children.ToArray();
        }
        /// <summary>Indexer to get items within this collection using array index syntax.</summary>
        /// <param name="i">Zero-based index of the entry to access.</param>
        /// <returns>The indexed item.</returns>
        public Cluster this[int i]
        {
            get
            {
                if (i >= Children.Length)
                    throw new IndexOutOfRangeException();
                else
                    return Children[i];
            }
        }
    }
}
