using System;
using System.IO;
using numl.Utils;
using System.Linq;
using NUnit.Framework;
using System.Diagnostics;
using System.Collections.Generic;
using numl.Supervised;
using numl.Serialization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace numl.Tests.SerializationTests
{
    public class BaseSerialization
    {
        [OneTimeSetUp]
        public void RegisterTypes()
        {
            // Need to register external assemblies
            Register.Assembly(GetType().GetTypeInfo().Assembly);
        }

        internal static string GetPath(Type t)
        {
            var basePath = String.Format("{0}\\{1}\\{2}", 
                Directory.GetCurrentDirectory(),
                "TestResults", 
                t.Name);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            basePath += "\\{0}.json";
            return basePath;
        }

        internal void Serialize(object o)
        {
            var caller = GetCaller();
            string file = string.Format(GetPath(GetType()), caller);
            if (File.Exists(file)) File.Delete(file);

            using (var fs = new FileStream(file, FileMode.CreateNew))
            using (var f = new StreamWriter(fs))
                new JsonWriter(f).Write(o);
        }

        internal T Deserialize<T>()
        {
            var caller = GetCaller();
            string file = string.Format(GetPath(GetType()), caller);

            using (var fs = new FileStream(file, FileMode.Open))
            using (var f = new StreamReader(fs))
            {
                var val = new JsonReader(f).Read();
                return (T)val;
            }
        }

        internal object Deserialize()
        {
            var caller = GetCaller();
            string file = string.Format(GetPath(GetType()), caller);

            using (var fs = new FileStream(file, FileMode.Open))
            using (var f = new StreamReader(fs))
                return new JsonReader(f).Read();
        }

        internal JsonWriter GetWriter()
        {
            var caller = GetCaller();
            string file = string.Format(GetPath(GetType()), caller);
            if (File.Exists(file)) File.Delete(file);

            var fs = new FileStream(file, FileMode.CreateNew);
            var f = new StreamWriter(fs);
            return new JsonWriter(f);
        }

        internal JsonReader GetReader()
        {
            var caller = GetCaller();
            string file = string.Format(GetPath(GetType()), caller);

            var fs = new FileStream(file, FileMode.Open);
            var f = new StreamReader(fs);
            return new JsonReader(f);
        }


        internal string GetCaller()
        {
            var stack = Environment.StackTrace.Split('\n')
                            .Select(s => s.Trim())
                            .SkipWhile(s => !s.Contains(GetType().GetTypeInfo().Name))
                            .ToArray();

            Regex regex = new Regex(@".\.(.*)\(");
            var match = regex.Match(stack[0]);
            var method = match.Groups[1].Value.Split('.').Last();
            return method;
        }
    }
}
