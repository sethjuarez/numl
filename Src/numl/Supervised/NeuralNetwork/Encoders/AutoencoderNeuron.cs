using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.Supervised.NeuralNetwork.Optimization;

namespace numl.Supervised.NeuralNetwork.Encoders
{
    /// <summary>
    /// An autoencoding node.
    /// </summary>
    public class AutoencoderNeuron : Neuron
    {
        protected double Mu { get; set; } = 0;

        /// <summary>
        /// Computes the divergence property w.r.t the current distribution.
        /// </summary>
        /// <param name="sparsity">Sparsity value.</param>
        /// <param name="weight">Sparsity weight control value.</param>
        /// <param name="mu">Average activation value.</param>
        /// <returns></returns>
        public static double Divergence(double sparsity, double weight, double mu)
        {
            return weight * (-(sparsity / mu) + ((1.0 - sparsity) / (1.0 - mu)));
        }

        /// <summary>Calculates and returns the Node's <see cref="Neuron.Output"/> value.</summary>
        /// <remarks>Input is equal to the weights multiplied by the source <see cref="Neuron"/>'s Output.</remarks>
        /// <returns>A double.</returns>
        public override double Evaluate()
        {
            if (In.Count > 0)
            {
                Input = In.Sum(e => e.Weight * e.Source.Evaluate());
            }

            Output = ActivationFunction.Compute(Input);

            return Output;
        }

        /// <summary>Calculates and returns the error derivative (<see cref="Neuron.Delta"/>) of this node.</summary>
        /// <param name="t">The double to process.</param>
        /// <returns>A double.</returns>
        public override double Error(double t, NetworkTrainingProperties properties)
        {
            _DeltaL = Delta;

            this.Mu = (1.0 / properties.Examples) * this.Output;

            if (Out.Count == 0)
                Delta = delta = -(t - Output);

            else
            {
                if (In.Count > 0 && Out.Count > 0)
                {
                    double hp = this.ActivationFunction.Derivative(this.Input, this.Output);
                    double divergence = AutoencoderNeuron.Divergence((double)properties[nameof(AutoencoderGenerator.Sparsity)], 
                                                                     (double)properties[nameof(AutoencoderGenerator.SparsityWeight)], 
                                                                     this.Mu);
                    delta = (Out.Sum(e => e.Weight * t) + divergence) * hp;
                }

                Delta = Out.Sum(s => s.Target.delta * this.Output);
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
            for (int edge = 0; edge < this.In.Count; edge++)
            {
                Delta = (1.0 / properties.Examples) * Delta;

                if (edge > 0)
                    Delta = Delta + ((properties.Lambda / properties.Examples) * this.In[edge].Weight);

                if (!this.Constrained)
                {
                    this.In[edge].Weight = networkTrainer.Update(this.In[edge].ParentId, this.In[edge].ChildId,
                                                                 nameof(Edge.Weight), this.In[edge].Weight, this.Delta, properties);
                }
                this.In[edge].Source.Update(properties, networkTrainer);
            }
        }

        public override void Reset(NetworkTrainingProperties properties)
        {
            this.Mu = 0;

            base.Reset(properties);
        }
    }
}
