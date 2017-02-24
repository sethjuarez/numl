using System;
using System.IO;
using numl.Utils;
using System.Linq;
using Xunit;
using System.Diagnostics;
using System.Collections.Generic;
using numl.Supervised;
using numl.Serialization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace numl.Tests.SerializationTests
{
    public class BaseSerialization
    {
        public BaseSerialization()
        {
            // Need to register external assemblies
            Register.Assembly(GetType().GetTypeInfo().Assembly);
        }

        internal static string GetPath(Type t)
        {
            var basePath = Path.Combine(new[]{
                Path.GetTempPath(),
                "numl.Tests",
                t.Name
            });

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            return basePath;
        }

        internal void Serialize(object o, [CallerMemberName]string caller = "")
        {
            string file = Path.Combine(GetPath(GetType()), $"{caller}.json");

            if (File.Exists(file)) File.Delete(file);

            using (var fs = new FileStream(file, FileMode.CreateNew))
            using (var f = new StreamWriter(fs))
                new JsonWriter(f).Write(o);
        }

        internal T Deserialize<T>([CallerMemberName]string caller = "")
        {
            string file = Path.Combine(GetPath(GetType()), $"{caller}.json");

            using (var fs = new FileStream(file, FileMode.Open))
            using (var f = new StreamReader(fs))
            {
                var val = new JsonReader(f).Read();
                return (T)val;
            }
        }

        internal object Deserialize([CallerMemberName]string caller = "")
        {
            string file = Path.Combine(GetPath(GetType()), $"{caller}.json");

            using (var fs = new FileStream(file, FileMode.Open))
            using (var f = new StreamReader(fs))
                return new JsonReader(f).Read();
        }

        internal JsonWriter GetWriter([CallerMemberName]string caller = "")
        {
            string file = Path.Combine(GetPath(GetType()), $"{caller}.json");
            if (File.Exists(file)) File.Delete(file);

            var fs = new FileStream(file, FileMode.CreateNew);
            var f = new StreamWriter(fs);
            return new JsonWriter(f);
        }

        internal JsonReader GetReader([CallerMemberName]string caller = "")
        {
            string file = Path.Combine(GetPath(GetType()), $"{caller}.json");

            var fs = new FileStream(file, FileMode.Open);
            var f = new StreamReader(fs);
            return new JsonReader(f);
        }
    }
}
