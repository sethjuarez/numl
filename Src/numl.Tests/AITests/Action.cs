using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using numl.AI;

namespace numl.Tests.AITests
{
    public class Action : AI.Action
    {
        public Action(string name)
        {
            Name = name;
        }
    }
}
