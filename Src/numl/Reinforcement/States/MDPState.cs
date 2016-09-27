using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.AI;
using numl.Math.LinearAlgebra;

namespace numl.Reinforcement
{
    /// <summary>
    /// MDPState class.
    /// </summary>
    public class MDPState : State<MDPSuccessorState>, IMDPState
    {
        private Vector _Vector = null;

        /// <summary>
        /// Gets or sets the state feature vector.
        /// </summary>
        public Vector Features
        {
            get
            {
                if (_Vector == null)
                    _Vector = new Vector(new double[] { this.Id });
                return _Vector;
            }
            set
            {
                this._Vector = value;
            }
        }

        /// <summary>
        /// Initializes a new MDPState object.
        /// </summary>
        /// <param name="id">State identifier.</param>
        public MDPState(int id) : base(id)
        {
            this.Id = id;
            this.Successors = new HashSet<MDPSuccessorState>();
        }
    }
}
