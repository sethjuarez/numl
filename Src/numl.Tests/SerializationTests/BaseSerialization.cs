using System;
using System.IO;
using numl.Utils;
using System.Linq;
using NUnit.Framework;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml;

namespace numl.Tests.SerializationTests
{
    [TestFixture]
    public class BaseSerialization
    {
        private string _basePath;

        [TestFixtureSetUp]
        public void Setup()
        {
            _basePath = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), GetType().Name);
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
            _basePath += "\\{0}.json";
        }

        internal void Serialize(object o)
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(_basePath, caller);
            if (File.Exists(file))  File.Delete(file);
            JsonHelpers.Save(file, o);
        }

        internal T Deserialize<T>()
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(_basePath, caller);
            return JsonHelpers.Load<T>(file);
        }
    }
}
