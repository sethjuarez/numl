using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Data
{
    /// <summary>
    /// IEdge interface.
    /// </summary>
    public interface IEdge
    {
        /// <summary>
        /// Gets or sets the connecting Parent vertex identifier.
        /// </summary>
        int ParentId { get; set; }

        /// <summary>
        /// Gets or sets the connecting Child vertex identifier.
        /// </summary>
        int ChildId { get; set; }
    }
}
