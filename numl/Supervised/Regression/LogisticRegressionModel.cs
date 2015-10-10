using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

using numl.Utils;
using numl.Model;
using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using numl.Supervised.Classification;
using numl.Preprocessing;
using numl.Features;

namespace numl.Supervised.Regression
{
    /// <summary>
    /// A Logistic Regression Model object
    /// </summary>
    public class LogisticRegressionModel : Model, IClassifier
    {
        /// <summary>
        /// Theta parameters vector mapping X to y.
        /// </summary>
        public Vector Theta { get; set; }

        /// <summary>
        /// Logistic function
        /// </summary>
        public IFunction LogisticFunction { get; set; }

        /// <summary>
        /// The additional number of quadratic features to create as used in generating the model
        /// </summary>
        public int PolynomialFeatures { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogisticRegressionModel()
        {
            PolynomialFeatures = 0;
        }

        /// <summary>
        /// Computes the probability of the prediction being True.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double PredictRaw(Vector x)
        {
            Vector xCopy =  (this.NormalizeFeatures ? 
                                this.FeatureNormalizer.Normalize(x.IncreaseDimensions(this.PolynomialFeatures), this.FeatureProperties) 
                                : x.IncreaseDimensions(this.PolynomialFeatures));
            return this.LogisticFunction.Compute(xCopy.Insert(0, 1.0, false).Dot(Theta));
        }

        /// <summary>
        /// Create a prediction based on the learned Theta values and the supplied test item.
        /// </summary>
        /// <param name="x">Training record</param>
        /// <returns></returns>
        public override double Predict(Vector x)
        {
            return this.PredictRaw(x) >= 0.5d ? 1.0d : 0.0d;
        }

        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();

            var sigmoid = Ject.FindType(reader.GetAttribute(nameof(LogisticFunction)));
            LogisticFunction = (IFunction)Activator.CreateInstance(sigmoid);

            this.NormalizeFeatures = bool.Parse(reader.GetAttribute(nameof(NormalizeFeatures)));

            var normalizer = Ject.FindType(reader.GetAttribute(nameof(FeatureNormalizer)));
            base.FeatureNormalizer = (IFeatureNormalizer)Activator.CreateInstance(normalizer);

            reader.ReadStartElement();

            Descriptor = Xml.Read<Descriptor>(reader);
            base.FeatureProperties = Xml.Read<FeatureProperties>(reader, null, false);
            Theta = Xml.Read<Vector>(reader, nameof(Theta));
            PolynomialFeatures = Xml.Read<int>(reader, nameof(PolynomialFeatures));
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
            Xml.Write<int>(writer, PolynomialFeatures, nameof(PolynomialFeatures));
        }
    }
}
