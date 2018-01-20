using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.Math.LinearAlgebra;
using AutoTensor;
using numl.AI;

namespace numl.Reinforcement
{
    /// <summary>
    /// IReinforcementGenerator interface.
    /// </summary>
    public interface IReinforcementGenerator : IModelBase
    {
        /// <summary>
        /// Gets or sets the transition state descriptor.
        /// <para>A transition state descriptor is used for non temporal states, i.e. continuous states such as stock ticker data.</para>
        /// </summary>
        Descriptor TransitionDescriptor { get; set; }

        /// <summary>
        /// Generates an <see cref="IReinforcementModel"/> from the given continous state examples.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <param name="examples">The examples.</param>
        /// <returns>IReinforcementModel.</returns>
        IReinforcementModel Generate(Descriptor descriptor, IEnumerable<object> examples);

        /// <summary>
        /// Generates an <see cref="IReinforcementModel"/> from the given continuous state examples.
        /// </summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="descriptor">Descriptor</param>
        /// <param name="examples">The examples.</param>
        /// <returns>IReinforcementModel.</returns>
        IReinforcementModel Generate<T>(Descriptor descriptor, IEnumerable<T> examples) where T : class;

        /// <summary>
        /// Generates an <see cref="IReinforcementModel"/> based on a set of examples and transitions.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="examples1">Example training set of states and actions.</param>
        /// <param name="examples2">Corresponding training set where each item represents a transition from <paramref name="examples1"/>.</param>
        /// <returns>IReinforcementModel.</returns>
        IReinforcementModel Generate(IEnumerable<object> examples1, IEnumerable<object> examples2);

        /// <summary>
        /// Generate an <see cref="IReinforcementModel"/> based on a set of examples and transitions.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="descriptor">The description.</param>
        /// <param name="examples1">Example training set or state/action pairs.</param>
        /// <param name="transitionDescriptor">(Optional) Descriptor for extracting transition state/reward information from <paramref name="examples2"/> set.</param>
        /// <param name="examples2">(Optional) Corresponding training set where each item represents a transition state from <paramref name="examples1"/> and a reward label.</param>
        /// <returns>IReinforcementModel.</returns>
        IReinforcementModel Generate(Descriptor descriptor, IEnumerable<object> examples1, Descriptor transitionDescriptor, IEnumerable<object> examples2);

        /// <summary>
        /// Generates a <see cref="IReinforcementModel"/> based on states/actions with transitions and rewards.
        /// </summary>
        /// <param name="X1">Initial State matrix.</param>
        /// <param name="y">Action label vector.</param>
        /// <param name="X2">Transition State matrix.</param>
        /// <param name="r">Reward values.</param>
        /// <returns>IReinforcementModel.</returns>
        IReinforcementModel Generate(Matrix X1, Vector y, Matrix X2, Vector r);
    }
}
