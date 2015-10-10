using System;
using System.Linq;
using System.Xml;

using numl.Utils;
using numl.Model;
using numl.Math.Functions;
using numl.Math.LinearAlgebra;
using numl.Supervised.Classification;
using numl.Preprocessing;
using numl.Features;
using numl.Math.Kernels;

namespace numl.Supervised.Regression
{
    /// <summary>
    /// A SVM Model object
    /// </summary>
    public class SVMModel : Model, IClassifier
    {
        /// <summary>
        /// Gets or sets the Alpha values.
        /// </summary>
        public Vector Alpha { get; set; }

        /// <summary>
        /// Gets or sets the Bias value.
        /// </summary>
        public double Bias { get; set; }

        /// <summary>
        /// Gets or sets the learned Weights of the support vectors.
        /// </summary>
        public Vector Theta { get; set; }

        /// <summary>
        /// Gets or sets the optimal training data Matrix from the original set.
        /// </summary>
        public Matrix X { get; set; }

        /// <summary>
        /// Gets or sets the optimal label Vector of positive and negative examples (+1 / -1 form).
        /// </summary>
        public Vector Y { get; set; }

        /// <summary>
        /// Gets or sets the Kernel function to use for computing support vectors.
        /// </summary>
        public IKernel KernelFunction { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SVMModel() { }

        /// <summary>
        /// Computes the probability of the prediction being True.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double PredictRaw(Vector x)
        {
            double prediction = 0d;

            x = (this.NormalizeFeatures ?
                                this.FeatureNormalizer.Normalize(x, this.FeatureProperties)
                                : x);

            if (this.KernelFunction.IsLinear)
            {
                prediction = this.Theta.Dot(x) + this.Bias;
            }
            else
            {
                for (int j = 0; j < this.X.Rows; j++)
                {
                    prediction = prediction + this.Alpha[j] * this.Y[j] * this.KernelFunction.Compute(this.X[j, VectorType.Row], x);
                }
                prediction += this.Bias;
            }

            return prediction;
        }

        /// <summary>
        /// Create a prediction from the supplied test item.
        /// </summary>
        /// <param name="x">Training record</param>
        /// <returns></returns>
        public override double Predict(Vector x)
        {
            return this.PredictRaw(x) >= 0d ? 1.0d : 0.0d;
        }

        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is
        /// deserialized.</param>
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();

            Bias = double.Parse(reader.GetAttribute(nameof(Bias)));

            var similarity = Ject.FindType(reader.GetAttribute(nameof(KernelFunction)));
            KernelFunction = (IKernel)Activator.CreateInstance(similarity);

            this.NormalizeFeatures = bool.Parse(reader.GetAttribute(nameof(NormalizeFeatures)));

            var normalizer = Ject.FindType(reader.GetAttribute(nameof(FeatureNormalizer)));
            base.FeatureNormalizer = (IFeatureNormalizer)Activator.CreateInstance(normalizer);

            reader.ReadStartElement();

            Descriptor = Xml.Read<Descriptor>(reader);
            base.FeatureProperties = Xml.Read<FeatureProperties>(reader, null, false);
            Alpha = Xml.Read<Vector>(reader);
            Theta = Xml.Read<Vector>(reader);
            X = Xml.Read<Matrix>(reader);
            Y = Xml.Read<Vector>(reader);
        }

        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is
        /// serialized.</param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(nameof(Bias), Bias.ToString());
            writer.WriteAttributeString(nameof(KernelFunction), KernelFunction.GetType().Name);
            writer.WriteAttributeString(nameof(NormalizeFeatures), this.NormalizeFeatures.ToString());
            writer.WriteAttributeString(nameof(FeatureNormalizer), FeatureNormalizer.GetType().Name);

            Xml.Write<Descriptor>(writer, Descriptor);
            Xml.Write<FeatureProperties>(writer, base.FeatureProperties);
            Xml.Write<Vector>(writer, Alpha);
            Xml.Write<Vector>(writer, Theta);
            Xml.Write<Matrix>(writer, X);
            Xml.Write<Vector>(writer, Y);
        }
    }
}
