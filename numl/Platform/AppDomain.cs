using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace numl.Platform
{
    /// <summary>
    /// The default AppDomain
    /// </summary>
    public class AppDomainWrapper
    {
        /// <summary>
        /// The default app domain instance
        /// </summary>
        public static IAppDomain Instance { get; set; }
    }
}
