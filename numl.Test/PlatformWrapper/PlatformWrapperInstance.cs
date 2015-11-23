using numl.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace numl.Tests.PlatformWrapper
{
    public class PlatformWrapperInstance : IAppDomain
    {
        IEnumerable<IAssembly> IAppDomain.GetAssemblies()
        {
            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                yield return new AssemblyWrapper(assembly);
            }
        }
    }

    public class AssemblyWrapper : IAssembly
    {
        private Assembly _assembly;
        public AssemblyWrapper(Assembly assembly)
        {
            _assembly = assembly;
        }

        public string GetName()
        {
            return _assembly.GetName().Name;
        }
        public string GetFullName()
        {
            return _assembly.FullName;
        }
        public IEnumerable<Type> GetTypes()
        {
            return _assembly.GetTypes();
        }
    }
}
