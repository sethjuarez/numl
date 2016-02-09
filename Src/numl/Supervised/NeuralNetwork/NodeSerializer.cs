using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using numl.Serialization;

namespace numl.Supervised.NeuralNetwork
{
    public class NodeSerializer : ISerializer
    {
        public bool CanConvert(Type type)
        {
            return typeof(Node).IsAssignableFrom(type);
        }

        public object Read(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public void Write(TextWriter writer, object value)
        {
            throw new NotImplementedException();
        }
    }
}
