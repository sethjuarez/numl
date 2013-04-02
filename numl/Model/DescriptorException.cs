using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace numl.Model
{
    public class DescriptorException : Exception
    {
        public DescriptorException()
        {
            
        }
        public DescriptorException(string message)
            : base(message)
        {
            
        }
        public DescriptorException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
        protected DescriptorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
