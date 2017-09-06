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

        }

        internal string GetPath([CallerMemberName]string caller = "")
        {
            var basePath = Path.Combine(new[]{
                Path.GetTempPath(),
                "numl.Tests",
                GetType().Name
            });

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            return Path.Combine(basePath, $"{caller}.json");
        }

        internal T SaveAndLoad<T>(T o, string file)
        {
            JsonWriter.Save(o, file);
            return JsonReader.Read<T>(file);
        }

        internal T SaveAndLoadJson<T>(T o)
        {
            var json = JsonWriter.SaveJson(o);
            return JsonReader.ReadJson<T>(json);
        }
    }
}
