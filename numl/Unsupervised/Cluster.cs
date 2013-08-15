using System;
using System.Linq;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Unsupervised
{
    public class Cluster
    {
        public int Id { get; set; }
        internal IEnumerable<Vector> Points { get; set; }
        public object[] Members { get; set; }
        public Cluster[] Children { get; set; }
        public Vector Center { get; set; }

        public Cluster()
        {
        }

        public Cluster(int id, Cluster left, Cluster right )
        {
            Id = id;
            Children = new Cluster[] { left, right };
            Points = left.Points.Concat(right.Points);
            // maybe only need item at leaves
            Members = left.Members.Concat(right.Members).ToArray();
        }

        public Cluster(int id, IEnumerable<Cluster> children)
        {
            Id = id;
            Children = children.ToArray();
        }

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
