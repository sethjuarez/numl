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

    public static class Serializer
    {

        private static readonly List<ISerializer> _serializers;
        static Serializer()
        {
            _serializers = new List<ISerializer>();
            _serializers.Add(new MatrixSerializer());
            _serializers.Add(new VectorSerializer());
        }

        public static void AddSerializer(ISerializer serializer)
        {
            _serializers.Add(serializer);
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

        public static void Write(TextWriter writer, object value)
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
                else if (_serializers.Where(s => s.CanConvert(type)).Count() > 0)
                {
                    var serializer = _serializers
                                        .Where(s => s.CanConvert(type))
                                        .First();
                    serializer.Write(writer, value);
                }
                else if (value is IEnumerable)
                {
                    var c = value as IEnumerable;
                    WriteArray(writer, c);
                }
                else
                {
                    SerializeObject(writer, value, type);
                }
            }
        }

        public static void WriteBeginArray(this TextWriter stream)
        {
            stream.Write((char)BEGIN_ARRAY);
        }

        public static void WriteEndArray(this TextWriter stream)
        {
            stream.Write((char)END_ARRAY);
        }

        private static void WriteArray(this TextWriter writer,
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

        public static void WriteEndObject(this TextWriter stream)
        {
            stream.Write((char)END_OBJECT);
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
        private static void SerializeObject(TextWriter stream, object o, Type type)
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
        public static object Parse(TextReader reader)
        {
            // A JSON value MUST be an object, array, number, or string, 
            // or one of the following three literal names
            //    false null true

            // eat whitespace
            while (WHITESPACE.Contains((char)reader.Peek()))
                reader.Read();

            if (reader.Peek() == -1)
                return null;

            // eat whitespace
            while (WHITESPACE.Contains((char)reader.Peek()))
                reader.Read();

            var next = reader.Peek();

            if (next == BEGIN_OBJECT)
                return ParseObject(reader);
            else if (next == BEGIN_ARRAY)
                return ParseArray(reader);
            else if (next == QUOTATION)
                return ParseString(reader);
            else if (char.IsNumber((char)next) || next == '-')
                return ParseNumber(reader);
            else if (next == 'f' || next == 'n' || next == 't')
                return ParseLiteral(reader);
            else
                throw new InvalidOperationException("Unexpected token encountered while parsing json");
        }
        private static object ParseObject(TextReader sr)
        {
            if (sr.Read() == BEGIN_OBJECT)
            {
                var obj = new Dictionary<string, object>();
                int token = 0;
                while (token != END_OBJECT)
                {
                    while (WHITESPACE.Contains((char)sr.Peek()))
                        sr.Read();

                    string name = ParseString(sr);

                    while (WHITESPACE.Contains((char)sr.Peek()))
                        sr.Read();

                    if (sr.Read() != COLON)
                        throw new InvalidOperationException("Unexpected token");

                    while (WHITESPACE.Contains((char)sr.Peek()))
                        sr.Read();

                    if (obj.ContainsKey(name))
                        throw new InvalidOperationException("Key already exists");

                    obj[name] = Parse(sr);

                    while (WHITESPACE.Contains((char)sr.Peek()))
                        sr.Read();

                    token = sr.Read();
                    if (token != COMMA && token != END_OBJECT)
                        throw new InvalidOperationException("Unexpected token!");
                }

                while (WHITESPACE.Contains((char)sr.Peek()))
                    sr.Read();

                return obj;
            }
            else
                throw new InvalidOperationException("Unexpected token");
        }
        private static object ParseArray(TextReader sr)
        {
            if (sr.Read() == BEGIN_ARRAY)
            {
                List<object> array = new List<object>();
                int token = 0;
                do
                {
                    while (WHITESPACE.Contains((char)sr.Peek()))
                        sr.Read();

                    array.Add(Parse(sr));

                    while (WHITESPACE.Contains((char)sr.Peek()))
                        sr.Read();

                    token = sr.Read();
                    if (token != COMMA && token != END_ARRAY)
                        throw new InvalidOperationException("Unexpected token!");
                }
                while (token != END_ARRAY);

                while (WHITESPACE.Contains((char)sr.Peek()))
                    sr.Read();

                return array.ToArray();
            }
            else
                throw new InvalidOperationException("Unexpected token!");
        }
        private static string ParseString(TextReader sr)
        {
            if (sr.Read() == QUOTATION)
            {
                StringBuilder sb = new StringBuilder();

                int cur = sr.Read();
                while (cur != QUOTATION)
                {
                    if (cur == ESCAPE)
                    {
                        cur = sr.Read();
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
                                var hex = new string(new[] { (char)sr.Read(), (char)sr.Read(), (char)sr.Read(), (char)sr.Read() });
                                sb.Append((char)ushort.Parse(hex, NumberStyles.HexNumber));
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected token encountered!");

                        }
                    }
                    else
                        sb.Append((char)cur);
                    cur = sr.Read();
                }

                while (WHITESPACE.Contains((char)sr.Peek()))
                    sr.Read();

                return sb.ToString();
            }
            else
                throw new InvalidOperationException("Unexpected token in string");
        }
        private static double ParseNumber(TextReader sr)
        {
            // TODO: This maybe could be faster...
            StringBuilder sb = new StringBuilder();
            while (sr.Peek() > -1 && NUMBER.Contains((char)sr.Peek()))
                sb.Append((char)sr.Read());
            return double.Parse(sb.ToString());
        }
        private static object ParseLiteral(TextReader sr)
        {
            var next = sr.Peek();
            switch (next)
            {
                case 'f':
                    for (int i = 0; i < FALSE.Length; i++)
                        if (sr.Read() != FALSE[i])
                            throw new InvalidOperationException("Unexpected token!");
                    return false;
                case 'n':
                    for (int i = 0; i < NULL.Length; i++)
                        if (sr.Read() != NULL[i])
                            throw new InvalidOperationException("Unexpected token!");
                    return null;
                case 't':
                    for (int i = 0; i < TRUE.Length; i++)
                        if (sr.Read() != TRUE[i])
                            throw new InvalidOperationException("Unexpected token!");
                    return true;
                default:
                    throw new InvalidOperationException("Unexpected token!");

            }
        }
    }
}
