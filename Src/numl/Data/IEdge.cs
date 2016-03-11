using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Data
{
    public interface IEdge
    {
        int ParentId { get; set; }

        int ChildId { get; set; }
    }
}
