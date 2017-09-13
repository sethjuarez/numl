using numl.Serialization;
using numl.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace numl
{
    public static class Register
    {
        /// <summary>
        /// Registration for numl to understand all of
        /// your types
        /// </summary>
        /// <param name="assemblies">The assembly.</param>
        public static void Assembly(params Assembly[] assemblies)
        {
            // register assemblies
            foreach (var a in assemblies)
                Ject.AddAssembly(a);
        }

        public static void Type<T>()
        {
            Assembly(typeof(T).GetTypeInfo().Assembly);
        }
    }
}
