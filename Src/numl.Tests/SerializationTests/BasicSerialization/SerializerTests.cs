using System;
using System.IO;
using System.Linq;
using Xunit;
using numl.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using System.Globalization;

namespace numl.Tests.SerializationTests.BasicSerialization
{
    [Trait("Category", "Serialization")]
    public class SerializerTests
    {
        /// <summary>
        /// Wraps string into Reader
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>StreamReader.</returns>
        public static JsonReader JsonReaderFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            var sr = new StreamReader(stream);
            return new JsonReader(sr);
        }


        public static string JsonStringFromObject(object o)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter wr = new JsonWriter(sw);
            wr.Write(o);
            var s = Regex.Replace(sb.ToString(), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
            return s;
        }

        [Fact]
        public void LiteralTest()
        {
            var fl = JsonReaderFromString("false").Read();
            Assert.Equal(false, fl);
            var tr = JsonReaderFromString("true").Read();
            Assert.Equal(true, tr);
            var nl = JsonReaderFromString("null").Read();
            Assert.Equal(null, nl);
        }

        [Fact]
        public void NumberTest()
        {
            var a = JsonReaderFromString(System.Math.PI.ToString("r", CultureInfo.InvariantCulture)).Read();
            Assert.Equal(System.Math.PI, a);
            var b = JsonReaderFromString((-1 * System.Math.PI).ToString("r", CultureInfo.InvariantCulture)).Read();
            Assert.Equal(-1 * System.Math.PI, b);
            var c = JsonReaderFromString((4354).ToString()).Read();
            Assert.Equal(4354d, c);
            var d = JsonReaderFromString((-4354).ToString()).Read();
            Assert.Equal(-4354d, d);
            var e = JsonReaderFromString((double.MinValue).ToString("r", CultureInfo.InvariantCulture)).Read();
            Assert.Equal(double.MinValue, e);
            var f = JsonReaderFromString((double.MaxValue).ToString("r", CultureInfo.InvariantCulture)).Read();
            Assert.Equal(double.MaxValue, f);
        }

        public void Test<T>(string s, T a)
        {
            var experiment = JsonReaderFromString($"\"{s}\"").Read();
            Assert.Equal(a, experiment);
        }


        [Fact]
        public void StringTest()
        {
            Test("little string example", "little string example");
            Test("with \\\"escape\\\" thingy", "with \"escape\" thingy");
            Test("others \\t \\n \\r \\b \\f test", "others \t \n \r \b \f test");
            Test("unicode \\u00A3 test", "unicode \u00A3 test");
            Test("slashes \\\\ \\/ test", "slashes \\ / test");
        }

        [Fact]
        public void ArrayTest()
        {
            Action<string, object[]> test = (s, a) =>
            {
                var experiment = JsonReaderFromString(s).Read();
                Assert.Equal(a, experiment);
            };

            test("[1   , 2,   3,   4,    5]", new object[] { 1d, 2d, 3d, 4d, 5d});
            test($"[{System.Math.PI.ToString("r", CultureInfo.InvariantCulture)},{(-1 * System.Math.PI).ToString("r", CultureInfo.InvariantCulture)}]",
                   new object[] { System.Math.PI, -1 * System.Math.PI });

            var arr = "[\"little string example\",";
            arr += "\"with \\\"escape\\\" thingy\",";
            arr += "\"others \\t \\n \\r \\b \\f test\",";
            arr += "\"unicode \\u00A3 test\",";
            arr += "\"slashes \\\\ \\/ test\"]";

            var truth = new object[]
            {
                "little string example",
                "with \"escape\" thingy",
                "others \t \n \r \b \f test",
                "unicode \u00A3 test",
                "slashes \\ / test"
            };

            test(arr, truth);

            test("[true   , false,   null,   true ]", new object[] { true, false, null, true });
        }

        private void ObjectTest(string s, Dictionary<string, object> truth)
        {
            var d = JsonReaderFromString(s).Read() as Dictionary<string, object>;
            Assert.NotNull(d);
            d.Keys.Should().Contain(truth.Keys);
            foreach (var key in d.Keys)
                Assert.Equal(truth[key], d[key]);
        }

        [Fact]
        public void SimpleObjectTest()
        {
            
            var s1 = "{\n\t\"prop1\"  :  123213,\n\t\"prop2\" : \"simple string\"\n}";
            var d1 = new Dictionary<string, object>()
            {
                {"prop1", 123213d },
                {"prop2", "simple string" } 
            };

            ObjectTest(s1, d1);
        }

        [Fact]
        public void SimpleObjectTestWithArrays()
        {
            var s2 = "{\n\t\"prop1\"  :  123213,\n\t\"prop2\" : \"simple string\",\n\t\"prop3\" : [1 ,  2]\n}";
            var d2 = new Dictionary<string, object>()
            {
                {"prop1", 123213d },
                {"prop2", "simple string" },
                {"prop3", new object[] { 1d, 2d } }
            };

            ObjectTest(s2, d2);
        }

        [Fact]
        public void SimpleSerializationTests()
        {
            var s = "Super Interest String!";
            var ss = JsonStringFromObject(s);
            Assert.Equal($"\"{s}\"", ss);


            double x = double.MinValue;
            var xo = JsonStringFromObject(x);
            Assert.Equal(x.ToString("r", CultureInfo.InvariantCulture), xo);
        }

        [Fact]
        public void SimpleArraySerializationTests()
        {
            var x1 = new[] { 1, 2, 3, 4, 5, 6, 7 };
            var x1o = JsonStringFromObject(x1);
            var x1s = "[1,2,3,4,5,6,7]";
            Assert.Equal(x1s, x1o);

            var x2 = new[] { "a", "b", "c", "d", "e", "f", "g" };
            var x2o = JsonStringFromObject(x2);
            var x2s = "[\"a\",\"b\",\"c\",\"d\",\"e\",\"f\",\"g\"]";
            Assert.Equal(x2s, x2o);
        }

        [Fact]
        public void SimpleObjectSerializationTests()
        {
            var val = double.MaxValue;
            var valS = val.ToString("r", CultureInfo.InvariantCulture);

            var x1 = new { a = "one", b = val, c = false };
            var x1o = JsonStringFromObject(x1);
            var x1s = $"{{\"a\":\"one\",\"b\":{valS},\"c\":false}}";
            Assert.Equal(x1s, x1o);

            var x2 = new { a = "one", b = val, c = false, x = x1 };
            var x2o = JsonStringFromObject(x2);
            var x2s = $"{{\"a\":\"one\",\"b\":{valS},\"c\":false,\"x\":{{\"a\":\"one\",\"b\":{valS},\"c\":false}}}}";
            Assert.Equal(x2s, x2o);
        }
    }
}
