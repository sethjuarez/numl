using System;
using System.Linq;
using System.Collections.Generic;

namespace numl.Data
{
    public class Network : Graph
    {
        public IVertex[] In { get; set; }
        public IVertex[] Out { get; set; }
    }
}
