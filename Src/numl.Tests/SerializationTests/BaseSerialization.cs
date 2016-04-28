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
    public class BaseSerialization
    {
        [TestFixtureSetUp]
        public void RegisterTypes()
        {
            // Need to register external assemblies
            Register.Assembly(GetType().Assembly);
        }

        internal string GetPath()
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
            if (File.Exists(file)) File.Delete(file);
            using (var f = new StreamWriter(file, false))
                new JsonWriter(f).Write(o);
        }

        internal T Deserialize<T>()
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(GetPath(), caller);
            using (var f = new StreamReader(file))
            {
                var val = new JsonReader(f).Read();
                return (T)val;
            }
        }

        internal object Deserialize()
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(GetPath(), caller);
            using (var f = new StreamReader(file))
                return new JsonReader(f).Read();
        }

        internal JsonWriter GetWriter()
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(GetPath(), caller);
            if (File.Exists(file)) File.Delete(file);
            var f = new StreamWriter(file, false);
            return new JsonWriter(f);
        }

        internal JsonReader GetReader()
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(GetPath(), caller);
            var f = new StreamReader(file);
            return new JsonReader(f);
        }
    }
}
