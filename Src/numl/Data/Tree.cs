using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Data
{
    public class Tree : Graph<IVertex>
    {
        public IVertex Root { get; set; }
    }
}
