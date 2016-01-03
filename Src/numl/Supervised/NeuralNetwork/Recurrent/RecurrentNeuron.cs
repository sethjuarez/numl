using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using numl.Math.LinearAlgebra;
using numl.Math.Functions;
using numl.Math.Probability;

namespace numl.Supervised.NeuralNetwork.Recurrent
{
    /// <summary>
    /// An individual Gated Recurrent Neuron
    /// </summary>
    public class RecurrentNeuron : Node
    {
        /// <summary>
        /// Gets or Sets the hidden (internal) state of the neuron.
        /// </summary>
        public double H { get; set; }

        /// <summary>
        /// Gets or sets the current Reset state value.
        /// </summary>
        public double R { get; set; }

        /// <summary>
        /// Gets or sets the Reset gate bias value.
        /// </summary>
        public double Rb { get; set; }

        /// <summary>
        /// Gets or sets the Reset gate to Hidden state weight value.
        /// <para>This is equivalent to the Ur weight value.</para>
        /// </summary>
        public double Rh { get; set; }

        /// <summary>
        /// Gets or sets the Reset gate to Input weight value.
        /// <para>This is equivalent to the Wr value.</para>
        /// </summary>
        public double Rx { get; set; }

        /// <summary>
        /// Gets or sets the current Update state value.
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Gets or sets the Update gate bias value.
        /// </summary>
        public double Zb { get; set; }

        /// <summary>
        /// Gets or sets the Update gate to Hidden weight value.
        /// <para>This is equivalent to the Uz weight value.</para>
        /// </summary>
        public double Zh { get; set; }

        /// <summary>
        /// Gets or sets the Update gate to Input weight value.
        /// <para>This is equivalent to the Wz value.</para>
        /// </summary>
        public double Zx { get; set; }

        /// <summary>
        /// Gets or sets the reset gate function.
        /// </summary>
        public IFunction ResetGate { get; set; }

        /// <summary>
        /// Gets or sets the update (memory) gate function.
        /// </summary>
        public IFunction MemoryGate { get; set; }

        /// <summary>
        /// Initializes a new Recurrent Neuron.
        /// </summary>
        public RecurrentNeuron() : base()
        {
            this.H = 0d;
            this.Rb = 0d;
            this.Zb = 0d;
            this.Zx = Sampling.GetUniform();
            this.Zh = Sampling.GetUniform();
            this.Rx = Sampling.GetUniform();
            this.Rh = Sampling.GetUniform();

            if (this.ResetGate == null)
                this.ResetGate = new Math.Functions.SteepLogistic();
            if (this.MemoryGate == null)
                this.MemoryGate = new Math.Functions.SteepLogistic();
            if (this.ActivationFunction == null)
                this.ActivationFunction = new Math.Functions.Tanh();
        }

        /// <summary>
        /// Evaluates the state.
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            // guarantee updates to Input
            base.Evaluate();

            if (base.In.Count > 0)
            {
                // is hidden unit - apply memory states
                // Input is equal to combined input weights with bias values

                this.R = this.ResetGate.Compute((this.Rx * this.Input) + (this.Rh * this.H) + this.Rb);
                this.Z = this.MemoryGate.Compute((this.Zx * this.Input) + (this.Zh * this.H) + this.Zb);

                double htP = this.ActivationFunction.Compute(this.Input + (this.R * this.H));

                this.H = (1.0 - this.Z) * this.H + this.Z * htP;

                this.Output = (this.OutputFunction != null ? this.OutputFunction.Compute(this.H) : this.H);
            }

            return this.Output;
        }

        /// <summary>
        /// Returns the error given the supplied error derivative.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override double Error(double t)
        {
            //TODO: Return the correct error.
            base.Error(t);

            return this.Delta;
        }

        /// <summary>
        /// Updates the weights using the supplied <paramref name="learningRate" />
        /// </summary>
        /// <param name="properties">Network training properties.</param>
        public override void Update(NetworkTrainingProperties properties)
        {
            base.Update(properties);
        }
    }
}
