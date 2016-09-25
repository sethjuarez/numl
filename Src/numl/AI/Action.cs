using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.Utils;

namespace numl.AI
{
    /// <summary>
    /// Action class.
    /// </summary>
    public class Action : IAction
    {
        private static int _Id = -1;

        /// <summary>
        /// Gets or sets the action identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the probability of this action executing correctly.
        /// <para>The default is deterministic (i.e. Probability of 1.0).</para>
        /// </summary>
        public double Probability { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier, i.e. the start state.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Gets or sets the child identifier, i.e. the transition state.
        /// </summary>
        public int ChildId { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Action()
        {
            this.Id = (++_Id);
            this.Probability = 1.0;
        }

        /// <summary>
        /// Initializes a new Action.
        /// </summary>
        /// <param name="id">Action identifier.</param>
        /// <param name="name">Name of the action.</param>
        public Action(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        /// <summary>
        /// Creates a unique action identifier from the action value.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static int GetActionId(double action)
        {
            string s = action.ToString();

            if (s.Contains("."))
            {
                int idx = s.IndexOf(".");
                s = s.Substring(0, idx) + s.Substring(idx + 1, action.GetPrecision());
            }

            return int.Parse(s);
        }

        /// <summary>
        /// Returns the current hash code value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Determines whether this object is equal to <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">Object to test equality for.</param>
        /// <returns>Boolean.</returns>
        public override bool Equals(object obj)
        {
            return (obj as IAction != null && ((Action)obj).Id == this.Id);
        }

        /// <summary>
        /// Determines the sort order of the current action relative to the specified action.
        /// </summary>
        /// <param name="obj">Action to comapre.</param>
        /// <returns></returns>
        public virtual int CompareTo(object obj)
        {
            return ActionComparer.Compare(this, obj as IAction);
        }
    }
}
