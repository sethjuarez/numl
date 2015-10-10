// file:	Model\DescriptorException.cs
//
// summary:	Implements the descriptor exception class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace numl.Model
{
    /// <summary>Descriptor Exception.</summary>
    public class DescriptorException : Exception
    {
        /// <summary>Default constructor.</summary>
        public DescriptorException()
        {
            
        }
        /// <summary>Specialised constructor for use only by derived classes.</summary>
        /// <param name="message">The message.</param>
        public DescriptorException(string message)
            : base(message)
        {
            
        }
        /// <summary>Specialised constructor for use only by derived classes.</summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DescriptorException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
