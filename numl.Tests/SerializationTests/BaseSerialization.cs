using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections.Generic;

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
            _basePath += "\\{0}.xml";
        }

        internal void Serialize(object o)
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(_basePath, caller);
            if (File.Exists(file))  File.Delete(file);
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            using (TextWriter tw = new StreamWriter(file))
                serializer.Serialize(tw, o);
        }

        internal T Deserialize<T>()
        {
            var caller = new StackFrame(1, true).GetMethod().Name;
            string file = string.Format(_basePath, caller);
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            using(TextReader tr = new StreamReader(file))
               return (T)deserializer.Deserialize(tr);
        }
    }
}
