using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.AI;

namespace numl.Reinforcement
{
    /// <summary>
    /// MDPSuccessorState class.
    /// </summary>
    public class MDPSuccessorState : IMDPSuccessor
    {
        /// <summary>
        /// Gets or sets the action to the next state.
        /// </summary>
        public IAction Action { get; set; }

        /// <summary>
        /// Gets or sets the cost associated with the action.
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// Gets or sets the successor MDP State.
        /// </summary>
        public IState State { get; set; }

        /// <summary>
        /// Gets or sets the reward for the successor state.
        /// </summary>
        public double Reward { get; set; }

        /// <summary>
        /// Initializes a new MDPSuccessorState.
        /// </summary>
        /// <param name="action">Action to the state.</param>
        /// <param name="cost">Cost of the transition state.</param>
        /// <param name="state">Transition state.</param>
        /// <param name="reward">Reward value.</param>
        public MDPSuccessorState(IAction action, double cost, IMDPState state, double reward)
        {
            this.Action = action;
            this.Cost = cost;
            this.State = state;
            this.Reward = reward;
        }

        /// <summary>
        /// Returns True if the supplied object equals the current object.
        /// </summary>
        /// <param name="obj">Object to test.</param>
        /// <returns>Boolean.</returns>
        public override bool Equals(object obj)
        {
            return this.GetHashCode() == ((IMDPSuccessor) obj).GetHashCode();
        }

        /// <summary>
        /// Returns the hash code for the current object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return int.Parse($"{State.Id}{Action.ChildId}");
        }
    }
}
