using System;
using System.Linq;
using numl.Math.Kernels;
using numl.Math.LinearAlgebra;
using numl.Supervised.Classification;
using numl.Utils;

namespace numl.Supervised.SVM
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

            this.Preprocess(x);

            if (KernelFunction.IsLinear)
            {
                prediction = Theta.Dot(x) + Bias;
            }
            else
            {
                for (int j = 0; j < X.Rows; j++)
                {
                    prediction = prediction + Alpha[j] * Y[j] * KernelFunction.Compute(X[j, VectorType.Row], x);
                }
                prediction += Bias;
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
            return PredictRaw(x) >= 0d ? Ject.DefaultTruthValue : Ject.DefaultFalseValue;
        }
    }
}
