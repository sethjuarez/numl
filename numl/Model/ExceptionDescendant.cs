using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Model
{
    public class ExceptionDescendant : Exception
    {
        public ExceptionDescendant()
        {

        }
        public ExceptionDescendant(string message)
            : base(message)
        {

        }
        public ExceptionDescendant(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        protected ExceptionDescendant(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
