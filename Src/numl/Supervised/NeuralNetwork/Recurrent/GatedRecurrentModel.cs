using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using numl.Math.LinearAlgebra;
using numl.Math.Functions;

namespace numl.Supervised.NeuralNetwork.Recurrent
{
    /// <summary>
    /// Gated Recurrent Neural Network model.
    /// </summary>
    public class GatedRecurrentModel : Model, ISequenceModel
    {
        /// <summary>Gets or sets the network.</summary>
        /// <value>The network.</value>
        public Network Network { get; set; }

        /// <summary>
        /// Gets or sets the output layer function (i.e. Softmax).
        /// </summary>
        public IFunction OutputFunction { get; set; }

        /// <summary>
        /// Predicts the next label for the given input.
        /// </summary>
        /// <param name="x">State.</param>
        /// <returns></returns>
        public override double Predict(Vector x)
        {
            this.Preprocess(x);

            this.Network.Forward(x);
            // predict the next item
            Vector output = Network.Out.Select(n => n.Output).ToVector();

            return (this.OutputFunction != null ? this.OutputFunction.Compute(output).Max() : output.Max());
        }

        /// <summary>
        /// Predicts the next sequence label for the given input.
        /// </summary>
        /// <param name="x">State.</param>
        /// <returns></returns>
        public Vector PredictSequence(Vector x)
        {
            this.Preprocess(x);

            this.Network.Forward(x);
            // predict the next sequence
            Vector output = Network.Out.Select(n => n.Output).ToVector();

            return (this.OutputFunction != null ? this.OutputFunction.Compute(output) : output);
        }
    }
}
