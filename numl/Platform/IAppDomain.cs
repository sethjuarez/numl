using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Platform
{
    /// <summary>
    /// When implemented in a derived platform, returns a list of currently loaded assemblies
    /// </summary>
    public interface IAppDomain
    {
        /// <summary>
        /// Returns a list of assemblies loaded in the current domain
        /// </summary>
        /// <returns></returns>
        IEnumerable<IAssembly> GetAssemblies();
    }
}
