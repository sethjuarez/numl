using System;
using System.IO;
using numl.Utils;
using System.Linq;
using NUnit.Framework;
using System.Diagnostics;
using System.Collections.Generic;

namespace numl.Tests.SerializationTests
{
    [SetUpFixture]
    public class BaseSerialization
    {
        private string GetPath()
        {
            var basePath = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), GetType().Name);
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            basePath += "\\{0}.json";
            return basePath;
        }

        internal void Serialize(object o)
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(GetPath(), caller);
            if (File.Exists(file))  File.Delete(file);
            JsonHelpers.Save(file, o);
        }

        internal T Deserialize<T>()
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(GetPath(), caller);
            return JsonHelpers.Load<T>(file);
        }
    }
}
