using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using numl.Utils;
using System.Collections;
namespace numl.Serialization
{
    public struct JsonProperty
    {
        public string Name;
        public object Value;
    }

    public static class Serializer
    {

        private static readonly List<ISerializer> _serializers;
        static Serializer()
        {
            // type magic to selfr register all available
            // ISerializers
            var serializers =
                from t in typeof(ISerializer).Assembly.GetTypes()
                where typeof(ISerializer).IsAssignableFrom(t) &&
                      t != typeof(ISerializer)
                select (ISerializer)Activator.CreateInstance(t);

            _serializers = new List<ISerializer>(serializers);
        }

        internal static void AddSerializer(params ISerializer[] serializers)
        {
            _serializers.AddRange(serializers);
        }

        //begin-array     = ws %x5B ws  ; [ left square bracket
        private const int BEGIN_ARRAY = '[';
        //begin-object    = ws %x7B ws; { left curly bracket
        private const int BEGIN_OBJECT = '{';
        //end-array       = ws %x5D ws; ] right square bracket
        private const int END_ARRAY = ']';
        //end-object      = ws %x7D ws; } right curly bracket
        private const int END_OBJECT = '}';
        //name-separator  = ws %x3A ws; : colon
        private const int COLON = ':';
        //value-separator = ws %x2C ws; , comma
        private const int COMMA = ',';
        // "
        private const int QUOTATION = '"';
        // \
        private const int ESCAPE = '\\';

        private readonly static char[] FALSE = new[] { 'f', 'a', 'l', 's', 'e' };
        private readonly static char[] TRUE = new[] { 't', 'r', 'u', 'e' };
        private readonly static char[] NULL = new[] { 'n', 'u', 'l', 'l' };
        private readonly static char[] WHITESPACE = new[] { ' ', '\t', '\n', '\r' };
        private readonly static char[] NUMBER = new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.', '-', '+', 'e', 'E' };

        public static string[] ToStringArray(this object[] array)
        {
            return array.ForEach(o => o.ToString()).ToArray();
        }

        public static T[] ToArray<T>(this object o)
        {
            return ((object[])o).ForEach(i => (T)i).ToArray();
        }

        public static string[] ToStringArray(this object o)
        {
            return ((object[])o).ToStringArray();
        }

        public static void Write(this TextWriter writer, object value)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                var type = value.GetType();
                if (type == typeof(bool))
                {
                    writer.Write(((bool)value).ToString().ToLower());
                }
                else if (Ject.CanUseSimpleType(type))
                {
                    if (value is string)
                    {
                        var s = value.ToString()
                                 .Replace("\t", "\\t")
                                 .Replace("\n", "\\n")
                                 .Replace("\r", "\\r");

                        writer.Write($"\"{s}\"");
                    }
                    else
                        writer.Write(Ject.Convert(value).ToString("r"));
                }
                else if (HasSerializer(type))
                {
                    var serializer = GetSerializer(type);

                    serializer.Write(writer, value);
                }
                else if (value is IEnumerable)
                {
                    var c = value as IEnumerable;
                    WriteArray(writer, c);
                }
                else
                {
                    WriteObject(writer, value, type);
                }
            }
        }

        private static ISerializer GetSerializer(Type type)
        {
            var q = _serializers.Where(s => s.CanConvert(type));

            return q.Last();
        }

        private static bool HasSerializer(Type type)
        {
            return _serializers.Where(s => s.CanConvert(type))
                               .Count() > 0;
        }

        private static void WriteBeginArray(this TextWriter writer)
        {
            writer.Write((char)BEGIN_ARRAY);
        }

        private static void WriteEndArray(this TextWriter writer)
        {
            writer.Write((char)END_ARRAY);
        }

        public static void WriteArray(this TextWriter writer,
            IEnumerable c, ISerializer serializer = null)
        {
            writer.WriteBeginArray();
            bool first = true;
            foreach (var item in c)
            {
                if (!first)
                    writer.Write($"{(char)COMMA} ");

                if (serializer != null && serializer.CanConvert(item.GetType()))
                    serializer.Write(writer, item);
                else
                    Write(writer, item);

                first = false;
            }
            writer.WriteEndArray();
        }

        public static void WriteNull(this TextWriter stream)
        {
            stream.Write(new string(NULL));
        }

        public static void WriteStartObject(this TextWriter writer)
        {
            writer.Write((char)BEGIN_OBJECT);
        }

        public static void ReadStartObject(this TextReader reader)
        {
            reader.EatWhitespace();
            reader.ReadToken(BEGIN_OBJECT);
        }

        public static void WriteEndObject(this TextWriter stream)
        {
            stream.Write((char)END_OBJECT);
        }

        public static void ReadEndObject(this TextReader reader)
        {
            reader.EatWhitespace();
            reader.ReadToken(END_OBJECT);
        }

        public static void WriteProperty(this TextWriter writer,
            string name, object val, ISerializer serializer = null)
        {
            Write(writer, name);
            writer.Write($" {(char)COLON} ");
            if (serializer != null && serializer.CanConvert(val.GetType()))
                serializer.Write(writer, val);
            else
                Write(writer, val);
        }

        public static JsonProperty ReadProperty(this TextReader reader, ISerializer serializer = null)
        {
            JsonProperty p = new JsonProperty();
            reader.EatWhitespace();

            p.Name = reader.ReadString();

            reader.EatWhitespace();

            reader.ReadToken(COLON);

            reader.EatWhitespace();

            if (serializer == null)
                p.Value = Read(reader);
            else
                p.Value = serializer.Read(reader);

            return p;
        }

        public static void WriteNextProperty(this TextWriter writer,
            string name, object val, ISerializer serializer = null)
        {
            writer.Write($" {(char)COMMA} ");
            WriteProperty(writer, name, val, serializer);
        }

        public static JsonProperty ReadNextProperty(this TextReader reader, ISerializer serializer = null)
        {
            reader.EatWhitespace();
            reader.ReadToken(COMMA);
            return reader.ReadProperty(serializer);
        }

        public static void WriteArrayProperty(this TextWriter writer,
            string name, IEnumerable val, ISerializer serializer = null)
        {
            Write(writer, name);
            writer.Write($" {(char)COLON} ");
            if (serializer != null && serializer.CanConvert(val.GetType()))
                Serializer.WriteArray(writer, val, serializer);
            else
                Write(writer, val);
        }

        public static JsonProperty ReadArrayProperty(this TextReader reader, ISerializer serializer = null)
        {
            JsonProperty p = new JsonProperty();

            reader.EatWhitespace();

            p.Name = reader.ReadString();

            reader.EatWhitespace();

            reader.ReadToken(COLON);

            reader.EatWhitespace();

            p.Value = reader.ReadArray(serializer);

            return p;
        }

        public static void WriteNextArrayProperty(this TextWriter writer,
            string name, IEnumerable val, ISerializer serializer = null)
        {
            writer.Write($" {(char)COMMA} ");
            WriteArrayProperty(writer, name, val, serializer);
        }

        public static JsonProperty ReadNextArrayProperty(this TextReader reader, ISerializer serializer = null)
        {
            reader.EatWhitespace();
            reader.ReadToken(COMMA);
            return reader.ReadArrayProperty(serializer);
        }

        private static void WriteObject(TextWriter stream, object o, Type type)
        {
            stream.WriteStartObject();
            bool first = true;
            foreach (var pi in type.GetProperties())
            {
                if (!first)
                    stream.Write($"{(char)COMMA} ");

                stream.WriteProperty(pi.Name, pi.GetValue(o));

                first = false;
            }
            stream.WriteEndObject();
        }

        /// <summary>
        /// Parses the specified stream from json.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.InvalidOperationException">Unexpected token encountered while parsing json</exception>
        public static object Read(TextReader reader)
        {
            // A JSON value MUST be an object, array, number, or string, 
            // or one of the following three literal names
            //    false null true

            reader.EatWhitespace();

            if (reader.Peek() == -1)
                return null;

            reader.EatWhitespace();

            var next = reader.Peek();

            if (next == BEGIN_OBJECT)
                return ReadObject(reader);
            else if (next == BEGIN_ARRAY)
                return ReadArray(reader);
            else if (next == QUOTATION)
                return ReadString(reader);
            else if (char.IsNumber((char)next) || next == '-')
                return ReadNumber(reader);
            else if (next == 'f' || next == 'n' || next == 't')
                return ReadLiteral(reader);
            else
                throw new InvalidOperationException("Unexpected token encountered while parsing json");
        }
        private static object ReadObject(TextReader reader)
        {
            if (reader.Read() == BEGIN_OBJECT)
            {
                var obj = new Dictionary<string, object>();
                int token = 0;
                while (token != END_OBJECT)
                {
                    reader.EatWhitespace();

                    string name = ReadString(reader);

                    reader.EatWhitespace();

                    if (reader.Read() != COLON)
                        throw new InvalidOperationException("Unexpected token");

                    reader.EatWhitespace();

                    if (obj.ContainsKey(name))
                        throw new InvalidOperationException("Key already exists");

                    obj[name] = Read(reader);

                    reader.EatWhitespace();

                    token = reader.Read();
                    if (token != COMMA && token != END_OBJECT)
                        throw new InvalidOperationException("Unexpected token!");
                }

                reader.EatWhitespace();

                return obj;
            }
            else
                throw new InvalidOperationException("Unexpected token");
        }
        public static bool IsNull(this TextReader reader)
        {
            reader.EatWhitespace();
            return reader.Peek() == NULL[0] && reader.ReadLiteral() == null;
        }

        public static object[] ReadArray(this TextReader reader, ISerializer serializer = null)
        {
            reader.EatWhitespace();
            if (reader.Read() == BEGIN_ARRAY)
            {
                List<object> array = new List<object>();
                int token = 0;
                do
                {
                    reader.EatWhitespace();

                    // empty array situation
                    if(reader.Peek() == END_ARRAY)
                    {
                        reader.Read();
                        break;
                    }

                    if (serializer == null)
                        array.Add(Read(reader));
                    else
                        array.Add(serializer.Read(reader));

                    reader.EatWhitespace();

                    token = reader.Read();
                    if (token != COMMA && token != END_ARRAY)
                        throw new InvalidOperationException("Unexpected token!");
                }
                while (token != END_ARRAY);

                reader.EatWhitespace();

                return array.ToArray();
            }
            else
                throw new InvalidOperationException("Unexpected token!");
        }
        public static string ReadString(this TextReader reader)
        {
            if (reader.Read() == QUOTATION)
            {
                StringBuilder sb = new StringBuilder();

                int cur = reader.Read();
                while (cur != QUOTATION)
                {
                    if (cur == ESCAPE)
                    {
                        cur = reader.Read();
                        switch (cur)
                        {
                            case '\\':
                                sb.Append('\\');
                                break;
                            case '/':
                                sb.Append('/');
                                break;
                            case 'b':
                                sb.Append('\b');
                                break;
                            case 'f':
                                sb.Append('\f');
                                break;
                            case 'n':
                                sb.Append('\n');
                                break;
                            case 'r':
                                sb.Append('\r');
                                break;
                            case 't':
                                sb.Append('\t');
                                break;
                            case '"':
                                sb.Append('"');
                                break;
                            case 'u':
                                var hex = new string(new[] { (char)reader.Read(), (char)reader.Read(), (char)reader.Read(), (char)reader.Read() });
                                sb.Append((char)ushort.Parse(hex, NumberStyles.HexNumber));
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected token encountered!");

                        }
                    }
                    else
                        sb.Append((char)cur);
                    cur = reader.Read();
                }

                reader.EatWhitespace();

                return sb.ToString();
            }
            else
                throw new InvalidOperationException("Unexpected token in string");
        }
        public static double ReadNumber(this TextReader reader)
        {
            // TODO: This maybe could be faster...
            StringBuilder sb = new StringBuilder();
            while (reader.Peek() > -1 && NUMBER.Contains((char)reader.Peek()))
                sb.Append((char)reader.Read());
            return double.Parse(sb.ToString());
        }
        public static object ReadLiteral(this TextReader reader)
        {
            var next = reader.Peek();
            switch (next)
            {
                case 'f':
                    for (int i = 0; i < FALSE.Length; i++)
                        if (reader.Read() != FALSE[i])
                            throw new InvalidOperationException("Unexpected token!");
                    return false;
                case 'n':
                    for (int i = 0; i < NULL.Length; i++)
                        if (reader.Read() != NULL[i])
                            throw new InvalidOperationException("Unexpected token!");
                    return null;
                case 't':
                    for (int i = 0; i < TRUE.Length; i++)
                        if (reader.Read() != TRUE[i])
                            throw new InvalidOperationException("Unexpected token!");
                    return true;
                default:
                    throw new InvalidOperationException("Unexpected token!");

            }
        }

        private static void ReadToken(this TextReader reader, int token)
        {
            if (reader.Read() != token)
                throw new InvalidOperationException($"Invalid token (expected {token})!");
        }

        private static void EatWhitespace(this TextReader reader)
        {
            while (WHITESPACE.Contains((char)reader.Peek()))
                reader.Read();
        }
    }
}
