using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace numl.Exceptions
{
    public class InvalidDescriptionException : Exception
    {
        public InvalidDescriptionException()
        {
            
        }

        public InvalidDescriptionException(string message)
            : base(message)
        {
            
        }
        public InvalidDescriptionException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
        protected InvalidDescriptionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
         
    }
}
