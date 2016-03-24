using numl.Supervised.DecisionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Serialization.Supervised.DecisionTree
{
    public class NodeSerializer : JsonSerializer<Node> { }
    public class EdgeSerializer : JsonSerializer<Edge> { }

}
