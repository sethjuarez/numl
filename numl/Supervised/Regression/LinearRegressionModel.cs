using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using System.Xml;
using numl.Utils;
using numl.Model;
using System.Xml.Serialization;
using numl.Features;
using numl.Preprocessing;

namespace numl.Supervised.Regression
{
    /// <summary>
    /// Linear Regression model
    /// </summary>
    public class LinearRegressionModel : Model
    {
        /// <summary>
        /// Theta parameters vector mapping X to y.
        /// </summary>
        [XmlAttribute("Theta")]
        public Vector Theta { get; set; }

        /// <summary>
        /// Initialises a new LinearRegressionModel object
        /// </summary>
        public LinearRegressionModel() { }

        /// <summary>
        /// Create a prediction based on the learned Theta values and the supplied test item.
        /// </summary>
        /// <param name="x">Training record</param>
        /// <returns></returns>
        public override double Predict(Vector x)
        {
            Vector xCopy = (this.NormalizeFeatures ?
                                this.FeatureNormalizer.Normalize(x, this.FeatureProperties)
                                : x);

            return xCopy.Insert(0, 1.0, false).Dot(Theta);
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
            Theta = Xml.Read<Vector>(reader, nameof(Theta));
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
            Xml.Write<Vector>(writer, Theta, nameof(Theta));
        }
    }
}
