using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.AI
{
    /// <summary>
    /// Interface IAdversarialState
    /// </summary>
    public interface IAdversarialState : IState
    {
        /// <summary>
        /// Gets the utility.
        /// </summary>
        /// <value>The utility.</value>
        double Utility { get; }
        /// <summary>
        /// Gets a value indicating whether this <see cref="IAdversarialState"/> is player.
        /// </summary>
        /// <value><c>true</c> if player; otherwise, <c>false</c>.</value>
        bool Player { get; }
        /// <summary>
        /// Resets this instance.
        /// </summary>
        /// <returns>IAdversarialState.</returns>
        IAdversarialState Reset();
    }
}
