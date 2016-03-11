using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Data
{
    public interface IVertex
    {
        int Id { get; set; }

        string Label { get; set; }

    }
}
