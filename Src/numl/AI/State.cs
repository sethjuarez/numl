using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.Data;

namespace numl.AI
{
    /// <summary>
    /// State class.
    /// </summary>
    public class State<TSuccessor> : IState where TSuccessor : class, ISuccessor
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is terminal.
        /// </summary>
        /// <value><c>true</c> if this instance is terminal; otherwise, <c>false</c>.</value>
        public virtual bool IsTerminal
        {
            get
            {
                return !this.GetSuccessors().Any();
            }
        }

        /// <summary>
        /// Gets or sets the successor state set.
        /// </summary>
        public HashSet<TSuccessor> Successors { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id"></param>
        public State(int id)
        {
            this.Id = id;
            this.Successors = new HashSet<TSuccessor>();
        }

        /// <summary>
        /// Gets the successors.
        /// </summary>
        /// <returns>IEnumerable&lt;ISuccessor&gt;.</returns>
        public virtual IEnumerable<ISuccessor> GetSuccessors()
        {
            return this.Successors.ToArray();
        }

        /// <summary>
        /// Heuristics this instance.
        /// </summary>
        /// <returns>System.Double.</returns>
        public virtual double Heuristic()
        {
            return 0;
        }

        /// <summary>
        /// Determines whether [is equal to] [the specified state].
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if [is equal to] [the specified state]; otherwise, <c>false</c>.</returns>
        public virtual bool IsEqualTo(IVertex state)
        {
            return this.Id == state.Id;
        }

        /// <summary>
        /// Determines the sort order of the current state relative to the specified state.
        /// </summary>
        /// <param name="obj">State to compare.</param>
        /// <returns></returns>
        public virtual int CompareTo(object obj)
        {
            return StateComparer.Compare(this, obj as IState);
        }
    }
}
