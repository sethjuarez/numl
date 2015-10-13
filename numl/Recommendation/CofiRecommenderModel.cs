using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using numl.Math.LinearAlgebra;
using numl.Model;
using numl.Features;
using numl.Preprocessing;
using numl.Utils;
using System.Xml.Serialization;
using numl.Math;

namespace numl.Recommendation
{
    /// <summary>
    /// Collaborative Filtering Recommender model.
    /// </summary>
    public class CofiRecommenderModel : Supervised.Model
    {
        /// <summary>
        /// Gets or sets the Range of the ratings, values outside of this will be treated as not provided.
        /// </summary>
        public Range Ratings { get; set; }

        /// <summary>
        /// Gets the Reference features mapping index of reference items and their corresponding col index.
        /// </summary>
        public Vector ReferenceFeatureMap { get; set; }

        /// <summary>
        /// Gets the Entity features mapping index of entity items and their corresponding row index.
        /// </summary>
        public Vector EntityFeatureMap { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier labels.
        /// </summary>
        public Vector Y { get; set; }

        /// <summary>
        /// Gets or sets the average rating of the source items for each target.
        /// </summary>
        public Vector Mu { get; set; }

        /// <summary>
        /// Get or sets the Theta matrix containing the weights for each source item / feature pair.
        /// <para>The source item is the learning source, such as a User Review / Rating.</para>
        /// </summary>
        public Matrix ThetaY { get; set; }

        /// <summary>
        /// Get or sets the Theta matrix containing the weights of each target item / feature pair.
        /// <para>The target item is the learning objective, such as a Movie or Product.</para>
        /// </summary>
        public Matrix ThetaX { get; set; }

        /// <summary>
        /// Gets or sets the rating matrix R.
        /// <para>Matrix of provided ratings, i.e. Book (m) ratings provided by Users (n) [m x n].</para>
        /// </summary>
        public Matrix Reference { get; set; }

        /// <summary>
        /// Gets the binary indicator matrix of references (i.e. ratings), where 1 indicates a value was provided otherwise 0.
        /// </summary>
        public Matrix R
        {
            get
            {
                return this.Reference.ToBinary(f => this.Ratings.Test(f));
            }
        }

        /// <summary>
        /// Initializes a new Collaborative Filtering recommender model.
        /// </summary>
        /// <param name="referenceMap">Vector of references and their corresponding column indexes in the Reference matrix.</param>
        /// <param name="entityMap">Vector of entities and their corresponding row indexes in the Reference matrix.</param>
        public CofiRecommenderModel(Vector referenceMap, Vector entityMap)
        {
            this.ReferenceFeatureMap = referenceMap;
            this.EntityFeatureMap = entityMap;
        }
        
        /// <summary>
        /// Not implemnted.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public override double Predict(Vector y)
        {
            // check the input vector and see if it exists in the ratings matrix
            int featureIndex = this.Reference.GetCols().Where(w => w == y).Select((s, i) => i).FirstOrDefault();

            if (featureIndex >= 0)
            {
                return this.Predict((int)this.ReferenceFeatureMap[featureIndex]).First();
            }
            else
            {
                throw new Exception("Feature ratings for this item were not found in the reference collection");
            }
        }

        /// <summary>
        /// Predicts all the recommendations of the Items for the supplied reference, i.e. a user.
        /// </summary>
        /// <param name="referenceId">Reference index to use for generating predictions.</param>
        /// <returns>Vector of predictions.</returns>
        public Vector Predict(int referenceId)
        {
            // [entities x features] * [references * features]
            var predictions = (this.ThetaX * this.ThetaY.T).Each((v, r, c) => v + this.Mu[r]);

            int[] indices = null;
            var sorted = predictions[(int)this.ReferenceFeatureMap[referenceId], VectorType.Col].Sort(false, out indices);

            return this.Y.Slice(indices, true);
        }

        /// <summary>
        /// Inserts a new source / reference feature (i.e. User rating items) into the existing model.
        /// </summary>
        /// <param name="item">A source feature object.</param>
        public void NewReference(object item)
        {
            if (this.Descriptor == null)
                throw new DescriptorException("A descriptor is required for inserting new references into the model.");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a new learning target entity feature item into the model, i.e. insert a new Movie into the existing model.
        /// </summary>
        /// <param name="item">A source entity object.</param>
        public void NewEntity(object item)
        {
            if (this.Descriptor == null)
                throw new DescriptorException("A descriptor is required for inserting new entities into the model.");

            throw new NotImplementedException();
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
            Mu = Xml.Read<Vector>(reader, nameof(Mu));
            ThetaY = Xml.Read<Matrix>(reader, nameof(ThetaY));
            ThetaX = Xml.Read<Matrix>(reader, nameof(ThetaX));
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
            Xml.Write<Vector>(writer, Mu, nameof(Mu));
            Xml.Write<Matrix>(writer, ThetaY, nameof(ThetaY));
            Xml.Write<Matrix>(writer, ThetaX, nameof(ThetaX));
        }
    }
}
