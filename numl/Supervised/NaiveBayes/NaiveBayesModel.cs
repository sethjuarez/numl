// file:	Supervised\NaiveBayes\NaiveBayesModel.cs
//
// summary:	Implements the naive bayes model class
using System;
using numl.Model;
using System.Xml;
using System.Linq;
using System.Xml.Schema;
using numl.Math.LinearAlgebra;
using System.Xml.Serialization;
using System.Collections.Generic;
using numl.Utils;
using numl.Features;
using numl.Preprocessing;

namespace numl.Supervised.NaiveBayes
{
    /// <summary>A data Model for the naive bayes.</summary>
    public class NaiveBayesModel : Model
    {
        /// <summary>Gets or sets the root.</summary>
        /// <value>The root.</value>
        public Measure Root { get; set; }
        /// <summary>Predicts the given o.</summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An object.</returns>
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
        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();

            this.NormalizeFeatures = bool.Parse(reader.GetAttribute(nameof(NormalizeFeatures)));

            var normalizer = Ject.FindType(reader.GetAttribute(nameof(FeatureNormalizer)));
            base.FeatureNormalizer = (IFeatureNormalizer)Activator.CreateInstance(normalizer);

            reader.ReadStartElement();

            Descriptor = Xml.Read<Descriptor>(reader);
            base.FeatureProperties = Xml.Read<FeatureProperties>(reader, null, false);
            Root = Xml.Read<Measure>(reader, nameof(Measure));
        }
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(nameof(NormalizeFeatures), this.NormalizeFeatures.ToString());
            writer.WriteAttributeString(nameof(FeatureNormalizer), FeatureNormalizer.GetType().Name);

            Xml.Write<Descriptor>(writer, Descriptor);
            Xml.Write<FeatureProperties>(writer, base.FeatureProperties);
            Xml.Write<Measure>(writer, Root, nameof(Measure));
        }
    }
}
