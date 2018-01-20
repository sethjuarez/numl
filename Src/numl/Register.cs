using numl.Serialization;
using numl.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoTensor;

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
/
        }

        public static void Type<T>()
        {
            Assembly(typeof(T).GetTypeInfo().Assembly);
        }
    }
}
