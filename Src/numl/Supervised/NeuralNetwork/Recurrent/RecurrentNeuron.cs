using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using numl.Math.LinearAlgebra;
using numl.Math.Functions;
using numl.Math.Probability;
using numl.Supervised.NeuralNetwork.Optimization;
using numl.Utils;

namespace numl.Supervised.NeuralNetwork.Recurrent
{
    /// <summary>
    /// An individual Gated Recurrent Neuron
    /// </summary>
    public class RecurrentNeuron : Neuron
    {
        public const string TimeStepLabel = "TimeStep";

        protected double Htm1 = 0, HtP = 0, DRx = 0, DRh = 0, DZx = 0, DZh = 0, DHh = 0;

        #region Fields

        /// <summary>
        /// Gets or sets a map of H state deltas from previous time steps.
        /// </summary>
        protected Dictionary<int, double> DeltaH { get; set; } = new Dictionary<int, double>();

        /// <summary>
        /// Gets or sets a map of output state values for the current sequence.
        /// </summary>
        protected Dictionary<int, double> StatesH { get; set; } = new Dictionary<int, double>();

        /// <summary>
        /// Gets or sets a map of candidate state values for the current sequence.
        /// </summary>
        protected Dictionary<int, double> StatesHP { get; set; } = new Dictionary<int, double>();

        /// <summary>
        /// Gets or sets a map of input response values for the current sequence.
        /// </summary>
        protected Dictionary<int, double> StatesI { get; set; } = new Dictionary<int, double>();

        /// <summary>
        /// Gets or sets a map of reset gate response values for the current sequence.
        /// </summary>
        protected Dictionary<int, double> StatesR { get; set; } = new Dictionary<int, double>();

        /// <summary>
        /// Gets or sets a map of update gate response values for the current sequence.
        /// </summary>
        protected Dictionary<int, double> StatesZ { get; set; } = new Dictionary<int, double>();

        #endregion

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
        /// Gets or sets the Recurrent weight value.
        /// </summary>
        public double Hh { get; set; }

        /// <summary>
        /// Gets or sets the reset gate function.
        /// </summary>
        public IFunction ResetGate { get; set; }

        /// <summary>
        /// Gets or sets the update (memory) gate function.
        /// </summary>
        public IFunction UpdateGate { get; set; }

        /// <summary>
        /// Initializes a new Recurrent Neuron.
        /// </summary>
        public RecurrentNeuron() : base()
        {
            this.H = 0d;
            this.Rb = 0d;
            this.Zb = 0d;
            this.Zx = Edge.GetWeight(0.05);
            this.Zh = Edge.GetWeight(0.05);
            this.Rx = Edge.GetWeight(0.05);
            this.Rh = Edge.GetWeight(0.05);
            this.Hh = Edge.GetWeight(0.05);

            if (this.ResetGate == null)
                this.ResetGate = new Math.Functions.SteepLogistic();
            if (this.UpdateGate == null)
                this.UpdateGate = new Math.Functions.SteepLogistic();
            if (this.ActivationFunction == null)
                this.ActivationFunction = new Math.Functions.Tanh();
        }

        /// <summary>
        /// Stores state information prior to computing the error derivatives.
        /// </summary>
        /// <param name="properties">Network training properties object.</param>
        public void State(NetworkTrainingProperties properties)
        {
            this.StatesH[(int) properties[TimeStepLabel]] = this.Output;
            this.StatesHP[(int) properties[TimeStepLabel]] = this.HtP;
            this.StatesI[(int) properties[TimeStepLabel]] = this.Input;
            this.StatesR[(int) properties[TimeStepLabel]] = this.R;
            this.StatesZ[(int) properties[TimeStepLabel]] = this.Z;
        }

        /// <summary>
        /// Evaluates the state.
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            if (base.In.Count > 0)
            {
                this.Input = this.In.Sum(e => e.Weight * e.Source.Evaluate());

                // is hidden unit - apply memory states
                this.R = this.ResetGate.Compute((this.Rx * this.Input) + (this.Rh * this.H) + this.Rb);
                this.Z = this.UpdateGate.Compute((this.Zx * this.Input) + (this.Zh * this.H) + this.Zb);

                this.HtP = this.ActivationFunction.Compute(this.Input + this.R * (this.Hh * this.H));

                this.H = (1.0 - this.Z) * this.H + this.Z * this.HtP;

                this.Output = H;
            }
            else
            {
                Output = ActivationFunction.Compute(Input);
            }

            return this.Output;
        }

        /// <summary>
        /// Returns the error given the supplied error derivative.
        /// </summary>
        /// <param name="t">The error from the next layer.</param>
        /// <param name="properties">Network training properties object.</param>
        /// <returns></returns>
        public override double Error(double t, NetworkTrainingProperties properties)
        {
            //TODO: Return the correct error.
            _DeltaL = Delta;

            int timestep = (int) properties[TimeStepLabel];
            double ht = this.StatesH.ContainsKey(timestep) ? this.StatesH[timestep] : 0;

            if (Out.Count == 0)
                Delta = delta = -(t - ht);

            else
            {
                if (In.Count > 0 && Out.Count > 0)
                {
                    int seqlength = (int) properties[nameof(GatedRecurrentGenerator.SequenceLength)];

                    double htm1 = this.StatesH.ContainsKey(timestep - 1) ? this.StatesH[timestep - 1] : 0;
                    double input = this.StatesI[timestep];
                    double r = this.StatesR[timestep];
                    double z = this.StatesZ[timestep];

                    // seq mod
                    double seqmod = (1.0 / seqlength);

                    // dyhh = delta(htm1) = 1-Z, dyhz = delta(Z) = HtP
                    double dyhh = (1.0 - z), dyhz = this.StatesHP[timestep];
                    double dHtP = this.ActivationFunction.Derivative(input + r * (htm1 * this.Hh));

                    this.DHh = ((dHtP * dyhh) * htm1);
                    this.DHh = this.DeltaH[timestep] = this.DeltaH.GetValueOrDefault(timestep + 1, 0) + this.DHh;

                    double dr = this.ResetGate.Derivative((this.Rx * input) + (this.Rh * ht) + this.Rb);
                    double dz = this.UpdateGate.Derivative((this.Zx * input) + (this.Zh * ht) + this.Zb);

                    this.DRx = (seqmod * (dr * input)); this.DRh = (seqmod * (dr * ht));
                    this.DZx = (seqmod * (dz * input)); this.DZh = (seqmod * (dz * ht));

                    delta = Out.Sum(e => e.Weight * t);
                }

                this.Delta = Out.Sum(s => s.Target.delta * ht);
            }

            if (this.In.Count > 0)
            {
                for (int edge = 0; edge < this.In.Count; edge++)
                {
                    this.In[edge].Source.Error(this.Delta, properties);
                }
            }

            return Delta;
        }

        /// <summary>
        /// Propagates a weight update event upstream through the network.
        /// </summary>
        /// <param name="properties">Network training properties.</param>
        /// <param name="networkTrainer">Network training method.</param>
        public override void Update(NetworkTrainingProperties properties, INetworkTrainer networkTrainer)
        {
            double lm = (properties.Lambda / (int) properties[nameof(GatedRecurrentGenerator.SequenceLength)]);

            if (!this.Constrained)
            {
                this.Rx = networkTrainer.Update(this.NodeId, this.NodeId, nameof(this.Rx), this.Rx, this.DRx, properties);
                this.Rh = networkTrainer.Update(this.NodeId, this.NodeId, nameof(this.Rh), this.Rh, this.DRh, properties);

                this.Zx = networkTrainer.Update(this.NodeId, this.NodeId, nameof(this.Zx), this.Zx, this.DZx, properties);
                this.Zh = networkTrainer.Update(this.NodeId, this.NodeId, nameof(this.Zh), this.Zh, this.DZh, properties);

                this.Hh = networkTrainer.Update(this.NodeId, this.NodeId, nameof(this.Hh), this.Hh, this.DHh, properties);
            }

            for (int edge = 0; edge < this.In.Count; edge++)
            {
                Delta = (1.0 / properties.Examples) * Delta;

                if (!this.In[edge].Source.IsBias)
                    Delta = Delta + (lm * this.In[edge].Weight);

                if (!this.Constrained)
                {
                    this.In[edge].Weight = networkTrainer.Update(this.In[edge].ParentId, this.In[edge].ChildId, 
                                                                    nameof(Edge.Weight), this.In[edge].Weight, Delta, properties);
                }

                this.In[edge].Source.Update(properties, networkTrainer);
            }
        }

        /// <summary>
        /// Resets the state of the current neuron.
        /// </summary>
        /// <param name="properties">Network training properties.</param>
        public override void Reset(NetworkTrainingProperties properties)
        {
            this.H = 0; this.HtP = 0; this.DRx = 0; this.DRh = 0; this.DZx = 0; this.DZh = 0; this.DHh = 0;

            this.DeltaH = new Dictionary<int, double>();
            this.StatesH = new Dictionary<int, double>();

            base.Reset(properties);
        }
    }
}
