using System;
using numl.Model;
using System.Xml;
using System.Linq;
using System.Xml.Schema;
using numl.Math.LinearAlgebra;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace numl.Supervised.NaiveBayes
{
    public class NaiveBayesModel : Model
    {
        public Measure Root { get; set; }

        public override double Predict(Vector y)
        {
            if (Root == null || Descriptor == null)
                throw new InvalidOperationException("Invalid Model - Missing information");

            Vector lp = Vector.Zeros(Root.Probabilities.Length);
            for (int i = 0; i < Root.Probabilities.Length; i++)
            {
                Statistic stat = Root.Probabilities[i];
                lp[i] = System.Math.Log(stat.Probability);
                for (int j = 0; j < y.Length; j++)
                {
                    Measure conditional = stat.Conditionals[j];
                    var p = conditional.GetStatisticFor(y[j]);
                    // check for missing range, assign bad probability
                    lp[i] += System.Math.Log(p == null ? 10e-10 : p.Probability);
                }
            }
            var idx = lp.MaxIndex();
            return Root.Probabilities[idx].X.Min;
        }

        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement();
            Descriptor = ReadXml<Descriptor>(reader);
            Root = ReadXml<Measure>(reader);
        }

        public override void WriteXml(XmlWriter writer)
        {
            WriteXml<Descriptor>(writer, Descriptor);
            WriteXml<Measure>(writer, Root);
        }
    }
}
