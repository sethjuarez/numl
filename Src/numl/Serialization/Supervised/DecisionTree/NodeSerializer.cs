using numl.Supervised.DecisionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numl.Serialization.Supervised.DecisionTree
{
    public class NodeSerializer : JsonSerializer<Node>
    {
        public override object Read(JsonReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Write(JsonWriter writer, object value)
        {
            throw new NotImplementedException();
        }
    }
}
