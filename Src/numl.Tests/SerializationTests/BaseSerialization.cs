using System;
using System.IO;
using numl.Utils;
using System.Linq;
using NUnit.Framework;
using System.Diagnostics;
using System.Collections.Generic;
using numl.Supervised;
using numl.Serialization;

namespace numl.Tests.SerializationTests
{
    [SetUpFixture]
    public class BaseSerialization
    {
        internal string GetPath()
        {
            var basePath = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), GetType().Name);
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            basePath += "\\{0}.json";
            return basePath;
        }

        internal void SerializeWith<T>(object o)
            where T : ISerializer
        {
            var serializer = Activator.CreateInstance<T>();
            if (!serializer.CanConvert(o.GetType()))
                throw new InvalidOperationException("Bad serializer!");
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(GetPath(), caller);
            if (File.Exists(file)) File.Delete(file);
            using (var f = new StreamWriter(file, false))
                serializer.Write(f, o);
        }

        internal object DeserializeWith<T>()
            where T : ISerializer
        {
            var serializer = Activator.CreateInstance<T>();
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(GetPath(), caller);
            using (var f = new StreamReader(file))
                return serializer.Deserialize(f);
        }

        internal void Serialize(object o)
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(GetPath(), caller);
            if (File.Exists(file))  File.Delete(file);
            numl.Serialization.SerializationHelpers.Save(file, o);
        }

        internal T Deserialize<T>()
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(GetPath(), caller);
            return numl.Serialization.SerializationHelpers.Load<T>(file);
        }
    }
}
