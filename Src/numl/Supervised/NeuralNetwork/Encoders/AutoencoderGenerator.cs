using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Math.LinearAlgebra;
using numl.Math.Functions;
using numl.Math.Normalization;

namespace numl.Supervised.NeuralNetwork.Encoders
{
    /// <summary>
    /// An Autoencoding Network Generator.
    /// <para>By default this works as a (linear) denoising autoencoder.  If the features used are within [0, 1] range then try specifying a different Output Function such as Logistic or Tanh.</para>
    /// </summary>
    public class AutoencoderGenerator : NeuralNetworkGenerator, ISequenceGenerator
    {
        /// <summary>
        /// Gets or sets the density of the encoder. Higher values will learn the identity function more easily but may not denoise features as well.
        /// <para>When using a higher Density value than the number of inputs it is recommended to lower the <see cref="Sparsity"/> constraint value to be closer to zero.</para>
        /// </summary>
        public int Density { get; set; }

        /// <summary>
        /// Gets or sets the sparsity value.
        /// <para>This acts as a regularization weighting on <seealso cref="Edge.Weight"/>, constraining the weight from becoming excessively high.</para>
        /// </summary>
        public double Sparsity { get; set; }

        /// <summary>
        /// Gets or sets the weight of the <see cref="Sparsity"/> term.
        /// </summary>
        public double SparsityWeight { get; set; }

        /// <summary>
        /// Instantiates a new Autoencoder Generator object.
        /// </summary>
        public AutoencoderGenerator()
        {
            this.Density = -1;
            this.Sparsity = 0.2;
            this.SparsityWeight = 1.0;
            this.LearningRate = 0.01;
            this.Epsilon = double.NaN;
            this.Activation = new SteepLogistic();
            this.OutputFunction = new Ident();
            this.NormalizeFeatures = false;
        }

        /// <summary>
        /// Generates and returns a new Autoencoder model.
        /// </summary>
        /// <param name="X">The Matrix to process.</param>
        /// <param name="y">The original training label Vector (ignored).</param>
        /// <returns></returns>
        public override IModel Generate(Matrix X, Vector y)
        {
            return this.Generate(X, X);
        }

        public override ISequenceModel Generate(Matrix X, Matrix Y)
        {
            // dense autoencoders learn the approximation identity function so ignore labels.
            // the output layer is the number of columns in X

            // default hidden layer to 2/3 of the input
            this.Preprocess(X);

            if (this.Density <= 0) this.Density = (int) System.Math.Ceiling(X.Cols * (2.0 / 3.0));

            if (this.MaxIterations <= 0) MaxIterations = 400; // because Seth said so...

            Network network = Network.New().Create(X.Cols, X.Cols, this.Activation, this.OutputFunction,
                                        (i, j) => new AutoencoderNeuron(), epsilon: this.Epsilon, hiddenLayers: new int[] { this.Density });

            var model = new AutoencoderModel
            {
                Descriptor = Descriptor,
                NormalizeFeatures = base.NormalizeFeatures,
                FeatureNormalizer = base.FeatureNormalizer,
                FeatureProperties = base.FeatureProperties,
                Network = network,
                OutputFunction = this.OutputFunction
            };

            OnModelChanged(this, ModelEventArgs.Make(model, "Initialized"));

            NetworkTrainingProperties properties = NetworkTrainingProperties.Create(network, X.Rows, X.Cols, this.LearningRate,
                this.Lambda, this.MaxIterations, new { this.Density, this.Sparsity, this.SparsityWeight });

            for (int i = 0; i < this.MaxIterations; i++)
            {
                properties.Iteration = i;

                for (int x = 0; x < X.Rows; x++)
                {
                    network.Forward(X[x, VectorType.Row]);
                    //OnModelChanged(this, ModelEventArgs.Make(model, "Forward"));
                    network.Back(X[x, VectorType.Row], properties);
                }

                var result = String.Format("Run ({0}/{1}): {2}", i, MaxIterations, network.Cost);
                OnModelChanged(this, ModelEventArgs.Make(model, result));
            }

            return model;
        }
    }
}
