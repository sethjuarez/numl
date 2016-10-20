using numl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace numl.Supervised.NaiveBayes
{
    public class Label : IEdge
    {
        public string Text { get; set; }
        public int ChildId { get; set; }

        public int ParentId { get; set; }

        public static Label Create(IVertex parent, IVertex child, string text)
        {
            var l = new Label();
            l.Text = text;
            l.ParentId = parent.Id;
            l.ChildId = child.Id;
            return l;
        }
    }
}
