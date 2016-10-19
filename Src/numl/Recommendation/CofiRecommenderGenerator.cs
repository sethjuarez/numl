using System;
using numl.Math;
using System.Linq;
using numl.Supervised;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace numl.Recommendation
{
    /// <summary>
    /// Collaborative Filtering Recommender generator.
    /// </summary>
    public class CofiRecommenderGenerator : Generator
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
        /// Gets or sets the number of Collaborative Features to learn.
        /// <para>Each learned feature is independently obtained of other learned features.</para>
        /// </summary>
        public int CollaborativeFeatures { get; set; }

        /// <summary>
        /// Gets or sets the learning rate (alpha).
        /// </summary>
        public double LearningRate { get; set; }

        /// <summary>
        /// Gets or sets the regularisation term Lambda.
        /// </summary>
        public double Lambda { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of training iterations to perform when optimizing.
        /// </summary>
        public int MaxIterations { get; set; }

        /// <summary>
        /// Initialises a new Collaborative Filtering generator.
        /// </summary>
        public CofiRecommenderGenerator()
        {
            this.NormalizeFeatures = true;

            this.MaxIterations = 100;
            this.LearningRate = 0.1;

            this.FeatureNormalizer = new numl.Math.Normalization.ZeroMeanNormalizer();
        }

        /// <summary>
        /// Generates a new Collaborative Filtering model.
        /// </summary>
        /// <param name="X">Training matrix values.</param>
        /// <param name="y">Vector of entity identifiers.</param>
        /// <returns></returns>
        public override IModel Generate(Matrix X, Vector y)
        {
            this.Preprocess(X.Copy());

            // inputs are ratings from each user (X = entities x ratings), y = entity id.
            // create rating range in case we don't have one already
            if (this.Ratings == null)
                this.Ratings = new Range(X.Min(), X.Max());

            // indicator matrix of 1's where rating was provided otherwise 0's.
            Matrix R = X.ToBinary(f => this.Ratings.Test(f));

            // The mean needs to be values within rating range only.
            Vector mean = X.GetRows().Select(s => 
                                        s.Where(w => this.Ratings.Test(w)).Sum() / 
                                        s.Where(w => this.Ratings.Test(w)).Count()
                                    ).ToVector();

            // update feature averages before preprocessing features.
            this.FeatureProperties.Average = mean;

            this.Preprocess(X);

            // where references could be user ratings and entities are movies / books, etc.
            int references = X.Cols, entities = X.Rows;

            // initialize Theta parameters
            Matrix ThetaX = Matrix.Rand(entities, this.CollaborativeFeatures, -1d);
            Matrix ThetaY = Matrix.Rand(references, this.CollaborativeFeatures, -1d);

            numl.Math.Functions.Cost.ICostFunction costFunction = new numl.Math.Functions.Cost.CofiCostFunction()
            {
                CollaborativeFeatures = this.CollaborativeFeatures,
                Lambda = this.Lambda,
                R = R,
                Regularizer = null,
                X = ThetaX,
                Y = X.Unshape()
            };

            // we're optimising two params so combine them
            Vector Theta = Vector.Combine(ThetaX.Unshape(), ThetaY.Unshape());

            numl.Math.Optimization.Optimizer optimizer = new numl.Math.Optimization.Optimizer(Theta, this.MaxIterations, this.LearningRate)
            {
                CostFunction = costFunction
            };

            optimizer.Run();

            // extract the optimised parameter Theta
            ThetaX = optimizer.Properties.Theta.Slice(0, (ThetaX.Rows * ThetaX.Cols) - 1).Reshape(entities, VectorType.Row);
            ThetaY = optimizer.Properties.Theta.Slice(ThetaX.Rows * ThetaX.Cols, Theta.Length - 1).Reshape(references, VectorType.Row);

            // create reference mappings, each value is the original index.
            this.ReferenceFeatureMap = (this.ReferenceFeatureMap == null ? Vector.Create(references, i => i) : this.ReferenceFeatureMap);
            this.EntityFeatureMap = (this.EntityFeatureMap == null ? Vector.Create(entities, i => i) : this.EntityFeatureMap);

            return new CofiRecommenderModel()
            {
                Descriptor = this.Descriptor,
                NormalizeFeatures = this.NormalizeFeatures,
                FeatureNormalizer = this.FeatureNormalizer,
                FeatureProperties = this.FeatureProperties,
                Ratings = this.Ratings,
                ReferenceFeatureMap = this.ReferenceFeatureMap,
                EntityFeatureMap = this.EntityFeatureMap,
                Mu = mean,
                Y = y,
                Reference = X,
                ThetaX = ThetaX,
                ThetaY = ThetaY
            };
        }
    }
}
