using numl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace numl.Supervised.NaiveBayes
{
    public class Label 
    {
        public string Text { get; set; }

        public static Label Create(string text)
        {
            var l = new Label();
            l.Text = text;
            return l;
        }
    }
}