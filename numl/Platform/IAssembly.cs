using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Platform
{
    /// <summary>
    /// Represents an Assembly in the current platform
    /// </summary>
    public interface IAssembly
    {
        /// <summary>
        /// Returns the local name of the current assembly
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// Returns the full name of the current assembly
        /// </summary>
        /// <returns></returns>
        string GetFullName();

        /// <summary>
        /// Returns a collection of known types from the assembly
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetTypes();
    }
}
