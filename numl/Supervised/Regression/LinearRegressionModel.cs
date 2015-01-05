using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using System.Xml;
using numl.Utils;
using numl.Model;

namespace numl.Supervised.Regression
{
    /// <summary>
    /// Linear Regression model
    /// </summary>
    [Serializable]
    public class LinearRegressionModel : Model
    {
        /// <summary>
        /// Theta parameters vector mapping X to y.
        /// </summary>
        public Vector Theta { get; set; }

        /// <summary>
        /// A row vector of the feature averages
        /// </summary>
        private Vector FeatureAverages { get; set; }

        /// <summary>
        /// A row vector of the standard deviation for each feature
        /// </summary>
        private Vector FeatureStandardDeviations { get; set; }

        private Vector Normalise(Vector y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                y[i] = PreProcessing.FeatureNormalizer.FeatureScale(y[i], this.FeatureAverages[i], this.FeatureStandardDeviations[i]);
            }

            return y.Insert(0, 1.0d);
        }

        /// <summary>
        /// Initialises a new LinearRegressionModel object
        /// </summary>
        public LinearRegressionModel() { }
        /// <summary>
        /// Initialises a new LinearRegressionModel object
        /// </summary>
        /// <param name="featureAverages">The feature averages for use in scaling test case features</param>
        /// <param name="featureSdv">The feature standard deviations for use in scaling test case features</param>
        public LinearRegressionModel(Vector featureAverages, Vector featureSdv)
        {
            this.FeatureAverages = featureAverages;
            this.FeatureStandardDeviations = featureSdv;
        }

        /// <summary>
        /// Create a prediction based on the learned Theta values and the supplied test item.
        /// </summary>
        /// <param name="y">Training record</param>
        /// <returns></returns>
        public override double Predict(Vector y)
        {
            y = this.Normalise(y);

            return y.Dot(Theta);
        }

        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement();

            Descriptor = Xml.Read<Descriptor>(reader);
            Theta = Xml.Read<Vector>(reader);
            FeatureAverages = Xml.Read<Vector>(reader);
            FeatureStandardDeviations = Xml.Read<Vector>(reader);
        }
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            Xml.Write<Descriptor>(writer, Descriptor);
            Xml.Write<Vector>(writer, Theta);
            Xml.Write<Vector>(writer, FeatureAverages);
            Xml.Write<Vector>(writer, FeatureStandardDeviations);
        }
    }
}
